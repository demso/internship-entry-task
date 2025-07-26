FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TickTackToe.Api/TickTackToe.Api.csproj", "TickTackToe.Api/"]
RUN dotnet restore "TickTackToe.Api/TickTackToe.Api.csproj"
COPY . .
WORKDIR "/src/TickTackToe.Api"
RUN dotnet build "./TickTackToe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TickTackToe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ./TickTackToe.API/appsettings.json /app/appsettings.json
COPY ./TickTackToe.API/appsettings.Development.json /app/appsettings.Development.json

ENTRYPOINT ["dotnet", "TickTackToe.Api.dll"]
