project = "borkweb"

app "borkweb" {
  build {
    use "docker" {
      dockerfile = "Bork.Web.dockerfile"
      context = ".."
    }
  }
  config {
    env = {
      "ConnectionStrings:DefaultConnection" = "User ID=pguser;Password=pgpass;Host=postgres_db;Port=5432;Database=borkweb;Pooling=true;"
      "BorkApiAddress" = "http://borkapi:55969"
      "ASPNETCORE_URLS" = "http://0.0.0.0:58200"
      "ASPNETCORE_ENVIRONMENT" = "Production"
    }
  }
  deploy {
    use "docker" {
      service_port = 58200
    }
  }
}