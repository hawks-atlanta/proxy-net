#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["proxy-net.csproj", "."]

#ENV: Use the build arguments passed from Docker Compose as environment variables
ARG ASPNETCORE_URLS
ARG SERVICE_URL
ARG ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_URLS=$ASPNETCORE_URLS
ENV SERVICE_URL=$SERVICE_URL
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT

RUN dotnet restore "./proxy-net.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "proxy-net.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "proxy-net.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "proxy-net.dll"]