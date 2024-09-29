FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Application/HoneywellWeb/HoneywellWeb.csproj", "src/Application/HoneywellWeb/"]
COPY ["src/Domain/Honeywell.Models/Honeywell.Models.csproj", "src/Domain/Honeywell.Models/"]
COPY ["src/Infrastructure/Honeywell.Utility/Honeywell.Utility.csproj", "src/Infrastructure/Honeywell.Utility/"]
COPY ["src/Domain/Honeywell.DataAccess/Honeywell.DataAccess.csproj", "src/Domain/Honeywell.DataAccess/"]
COPY ["src/Application/Service.Contracts/Service.Contracts.csproj", "src/Application/Service.Contracts/"]
COPY ["src/Infrastructure/Service/Service.csproj", "src/Infrastructure/Service/"]
RUN dotnet restore "src/Application/HoneywellWeb/HoneywellWeb.csproj"
COPY . .
WORKDIR "/src/src/Application/HoneywellWeb"
RUN dotnet build "HoneywellWeb.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HoneywellWeb.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HoneywellWeb.dll"]
