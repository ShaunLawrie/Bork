using Microsoft.Extensions.Configuration;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Bork.Contracts;
using Newtonsoft.Json;

namespace Bork.Notifications.Services
{
    public class QueuingService : IQueuingService
    {
        private readonly IConnection _connection;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly IDictionary<string, IModel> _consumerChannels; // consumerTag, channel
        private readonly string _queueName;
        public bool Finished { get; }

        public QueuingService(IConfiguration config,
            IEmailService emailService,
            ILogger logger)
        {
            Finished = false;
            var connectionFactory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"],
                Port = int.Parse(config["RabbitMQ:Port"])
            };
            _connection = connectionFactory.CreateConnection();
            _queueName = config["RabbitMQ:QueueName"];
            _emailService = emailService;
            _logger = logger;
            _consumerChannels = new Dictionary<string, IModel>();

            DeclareQueue(_queueName);
        }

        private void DeclareQueue(string queueName)
        {
            _logger.Info("Creating notification queue if it doesn't exist.");
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
        }

        public long Count()
        {
            using (var channel = _connection.CreateModel())
            {
                return channel.MessageCount(_queueName);
            }
        }

        public void ConfigureEventHandlers(int concurrency)
        {
            for (var i = 1; i <= concurrency; i++)
            {
                var channel = _connection.CreateModel();
                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                // The event handler for a message being consumed
                consumer.Received += HandleNotification;
                consumer.ConsumerCancelled += HandleCancel;

                // Register the consumer with the current channel
                var consumerTag = channel.BasicConsume(queue: _queueName,
                    noAck: false,
                    consumer: consumer);

                _consumerChannels.Add(consumerTag, channel);

                _logger.Info($"Configured Consumer{i} for queue '{_queueName}'");
            }
        }

        private void HandleCancel(object sender, ConsumerEventArgs e)
        {
            _logger.Warn($"Channel '{e.ConsumerTag}' has received a cancel command so channel is being closed");
            _consumerChannels[e.ConsumerTag].Close();
        }

        private void HandleNotification(object sender, BasicDeliverEventArgs eventArgs)
        {
            _logger.Info($"Consumer with tag '{eventArgs.ConsumerTag}' on thread '{Thread.CurrentThread.ManagedThreadId}' is consuming from '{_queueName}'");

            try
            {
                var notification = Utf8ByteArrayToObject<NotificationMessage>(eventArgs.Body);
                _emailService.SendEmail(notification);

                // Acknowledge the message has been consumed
                _consumerChannels[eventArgs.ConsumerTag]
                    .BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to send email");

                try
                {
                    // Reject the message because something failed
                    _consumerChannels[eventArgs.ConsumerTag]
                        .BasicReject(deliveryTag: eventArgs.DeliveryTag, requeue: true);
                }
                catch (Exception ie)
                {
                    _logger.Error(ie, "Failed to tell RabbitMQ to requeue the message");
                }
            }
        }

        public void Shutdown()
        {
            _logger.Info("QueuingService is getting shut down");
            foreach (var channel in _consumerChannels)
            {
                channel.Value.BasicCancel(channel.Key);
            }

            // Give the channels some time to close up
            SpinWait.SpinUntil(() => _consumerChannels.All(channel => channel.Value.IsClosed),
                TimeSpan.FromSeconds(30));

            _connection.Close();
        }

        #region Disposal
        protected virtual void Dispose(bool disposing)
        {
            // Was originally handling service shutdown here
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            _logger.Info("QueuingService has been disposed");
        }
        #endregion

        #region Serialization
        private static T Utf8ByteArrayToObject<T>(byte[] byteArray)
        {
            var objectString = Encoding.UTF8.GetString(byteArray);
            return JsonConvert.DeserializeObject<T>(objectString);
        }
        #endregion
    }
}
