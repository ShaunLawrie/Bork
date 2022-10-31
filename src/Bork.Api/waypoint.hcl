project = "borkapi"

app "borkapi" {
  build {
    use "docker" {
      context = ".."
      dockerfile = "Bork.Api.dockerfile"
    }
  }
  config {
    env = {
      "ConnectionStrings:DefaultConnection" = "User ID=pguser;Password=pgpass;Host=postgres_db;Port=5432;Database=BorkApi;Pooling=true;"
      "RabbitMQ:Host" = "rabbitmq"
      "RabbitMQ:Port" = "5672"
      "RabbitMQ:QueueName" = "NotificationCreated"
      "ASPNETCORE_URLS" = "http://0.0.0.0:55969"
    }
  }
  deploy {
    use "docker" {
      service_port = 55969
    }
  }
}