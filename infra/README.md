Remove idiot files:
```pwsh
Get-ChildItem -Recurse -Include ("bin", "obj") | Remove-Item -Force
```

Install a waypoint server:
```pwsh
waypoint install --platform=docker --accept-tos
```

Check out waypoint server:
```pwsh
Start-Process https://localhost:9702
```

Add docker-compose for local infra:
```docker
version: '3.5'

name: "bork-infra"

services:
  postgres:
    container_name: "postgres_db"
    image: "postgres:13.0"
    environment:
      POSTGRES_USER: "pguser"
      POSTGRES_PASSWORD: "pgpass"
      PGDATA: "/data/postgres"
    volumes:
       - "postgres:/data/postgres"
       - "./postgres_init:/docker-entrypoint-initdb.d"
    ports:
      - "127.0.0.1:5432:5432"
  
  pgadmin:
    container_name: "pgadmin"
    image: "dpage/pgadmin4"
    environment:
      PGADMIN_DEFAULT_EMAIL: "pgadmin@pgadmin.pgadmin"
      PGADMIN_DEFAULT_PASSWORD: "pgadminpass"
      PGADMIN_CONFIG_SERVER_MODE: "False"
    volumes:
       - "pgadmin:/var/lib/pgadmin"
    ports:
      - "127.0.0.1:8080:80"

  rabbitmq:
    image: "rabbitmq:3-management-alpine"
    container_name: "rabbitmq"
    ports:
      - "127.0.0.1:5672:5672"
      - "127.0.0.1:15672:15672"
    volumes:
      - "rabbitmq_data:/var/lib/rabbitmq/"
      - "rabbitmq_logs:/var/log/rabbitmq/"

volumes:
  postgres:
  pgadmin:
  rabbitmq_data:
  rabbitmq_logs:
  
networks:
  default:
    name: "waypoint"
    external: true
```

Bootstrap the database with an init script:
```sql
CREATE DATABASE borkapi;
CREATE DATABASE borkweb;
CREATE DATABASE borknotification;
```

Add waypoint init template
```pwsh
cd src
waypoint init
<#
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
#>
```

Add dockerfiles to build old dotnet projects because buildpacks don't work:
```dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:1.1 as build

COPY ./Bork.Web/*.csproj /app/Bork.Web/
COPY ./Bork.Contracts/*.csproj /app/Bork.Contracts/

WORKDIR /app/Bork.Web/
RUN dotnet restore

COPY ./Bork.Web/ /app/Bork.Web/
COPY ./Bork.Contracts/ /app/Bork.Contracts/

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/sdk:1.1 AS runtime
WORKDIR /app
COPY --from=build /app/Bork.Web/out ./
ENTRYPOINT ["dotnet", "Bork.Web.dll"]
```


