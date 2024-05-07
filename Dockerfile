#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

LABEL io.openshift.expose-services="8080:http"
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get update && \
    apt-get upgrade -y && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./MeetingRoomObserver/MeetingRoomObserver.csproj", "./"]
RUN dotnet restore "MeetingRoomObserver.csproj"
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get update && \
    apt-get upgrade -y && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

COPY . .
WORKDIR "/src"
RUN dotnet build "MeetingRoomObserver/MeetingRoomObserver.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeetingRoomObserver/MeetingRoomObserver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeetingRoomObserver.dll"]