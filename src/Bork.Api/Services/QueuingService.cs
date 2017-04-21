using System;
using Microsoft.Extensions.Configuration;
using NLog;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Bork.Api.Services
{
    public class QueuingService : IQueuingService
    {
        private readonly IConnection _connection;
        private readonly ILogger _logger;
        private readonly string _queueName;

        public QueuingService(IConfiguration config,
            ILogger logger)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"],
                Port = int.Parse(config["RabbitMQ:Port"])
            };
            _connection = connectionFactory.CreateConnection();
            _queueName = config["RabbitMQ:QueueName"];
            _logger = logger;

            DeclareQueue(_queueName);
        }

        public bool Send(object message)
        {
            try
            {
                var encodedMessage = ObjectToUtf8ByteArray(message);
                using (var channel = _connection.CreateModel())
                {
                    var props = channel.CreateBasicProperties();
                    props.Persistent = true;

                    channel.BasicPublish(exchange: "",
                        routingKey: _queueName,
                        basicProperties: props,
                        body: encodedMessage);
                }

                _logger.Info("Message was sent to RabbitMQ");
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to send message to RabbitMQ");
                return false;
            }
            
        }

        private void DeclareQueue(string queueName)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
        }

        #region Disposal
        protected virtual void Dispose(bool disposing)
        {
            _connection.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Serialization
        private static byte[] ObjectToUtf8ByteArray(object objectToEncode)
        {
            var serialized = JsonConvert.SerializeObject(objectToEncode);
            return Encoding.UTF8.GetBytes(serialized);
        }
        #endregion
    }
}
