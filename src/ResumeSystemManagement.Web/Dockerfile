FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ResumeSystemManagement.Web/ResumeSystemManagement.Web.csproj", "src/ResumeSystemManagement.Web/"]
COPY ["src/ResumeSystemManagement.Application/ResumeSystemManagement.Application.csproj", "src/ResumeSystemManagement.Application/"]
COPY ["src/ResumeSystemManagement.Core/ResumeSystemManagement.Core.csproj", "src/ResumeSystemManagement.Core/"]
COPY ["src/ResumeSystemManagement.Infrastructure/ResumeSystemManagement.Infrastructure.csproj", "src/ResumeSystemManagement.Infrastructure/"]
RUN dotnet restore "src/ResumeSystemManagement.Web/ResumeSystemManagement.Web.csproj"
COPY . .
WORKDIR "/src/src/ResumeSystemManagement.Web"
RUN dotnet build "./ResumeSystemManagement.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ResumeSystemManagement.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResumeSystemManagement.Web.dll"]
