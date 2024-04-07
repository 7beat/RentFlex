#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RentFlex.Web/RentFlex.Web.csproj", "RentFlex.Web/"]
COPY ["RentFlex.Infrastructure/RentFlex.Infrastructure.csproj", "RentFlex.Infrastructure/"]
COPY ["RentFlex.Application/RentFlex.Application.csproj", "RentFlex.Application/"]
COPY ["RentFlex.Domain/RentFlex.Domain.csproj", "RentFlex.Domain/"]
COPY ["RentFlex.Utility/RentFlex.Utility.csproj", "RentFlex.Utility/"]
RUN dotnet restore "./RentFlex.Web/RentFlex.Web.csproj"
COPY . .
WORKDIR "/src/RentFlex.Web"
RUN dotnet build "./RentFlex.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RentFlex.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
USER root
RUN mkdir -p /app/uploads/images \
    && chown -R app:app /app/uploads/images \
    && chmod -R 755 /app/uploads/images
USER app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RentFlex.Web.dll"]