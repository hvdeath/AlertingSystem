FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Alerting.Api/Alerting.Api.csproj", "src/Alerting.Api/"]
RUN dotnet restore "src/Alerting.Api/Alerting.Api.csproj"
COPY . .
WORKDIR "/src/src/Alerting.Api"
RUN dotnet publish "Alerting.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Alerting.Api.dll"]
