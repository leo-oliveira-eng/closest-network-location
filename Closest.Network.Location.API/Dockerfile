#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Closest.Network.Location.API/Closest.Network.Location.API.csproj", "Closest.Network.Location.API/"]
RUN dotnet restore "Closest.Network.Location.API/Closest.Network.Location.API.csproj"
COPY . .
WORKDIR "/src/Closest.Network.Location.API"
RUN dotnet build "Closest.Network.Location.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Closest.Network.Location.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Closest.Network.Location.API.dll"]