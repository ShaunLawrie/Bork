FROM mcr.microsoft.com/dotnet/core/sdk:1.1 as build

COPY ./Bork.Api/*.csproj /app/Bork.Api/
COPY ./Bork.Contracts/*.csproj /app/Bork.Contracts/

WORKDIR /app/Bork.Api/
RUN dotnet restore

COPY ./Bork.Api/ /app/Bork.Api/
COPY ./Bork.Contracts/ /app/Bork.Contracts/

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/sdk:1.1 AS runtime
WORKDIR /app
COPY --from=build /app/Bork.Api/out ./
ENTRYPOINT ["dotnet", "Bork.Api.dll"]