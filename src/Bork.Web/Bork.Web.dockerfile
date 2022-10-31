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