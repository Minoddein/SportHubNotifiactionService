FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SportHubNotificationService/SportHubNotificationService.csproj", "src/SportHubNotificationService/"]
RUN dotnet restore "src/SportHubNotificationService/SportHubNotificationService.csproj"
COPY . .
WORKDIR "/src/src/SportHubNotificationService"
RUN dotnet build "SportHubNotificationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SportHubNotificationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SportHubNotificationService.dll"]
