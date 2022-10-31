FROM mcr.microsoft.com/dotnet/core/sdk:1.1 as build

COPY ./Bork.Notifications/*.csproj /app/Bork.Notifications/
COPY ./Bork.Contracts/*.csproj /app/Bork.Contracts/

WORKDIR /app/Bork.Notifications/
RUN dotnet restore

COPY ./Bork.Notifications/ /app/Bork.Notifications/
COPY ./Bork.Contracts/ /app/Bork.Contracts/

# Overwrite the default appsettings with new docker ones
COPY ./Bork.Notifications/AppSettings.Docker.json /app/Bork.Notifications/AppSettings.json

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/sdk:1.1 AS runtime
WORKDIR /app
COPY --from=build /app/Bork.Notifications/out ./
ENTRYPOINT ["dotnet", "Bork.Notifications.dll"]