#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Discout.API/Discout.API.csproj", "Discout.API/"]
RUN dotnet restore "Discout.API/Discout.API.csproj"
COPY . .
WORKDIR "/src/Discout.API"
RUN dotnet build "Discout.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discout.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discout.API.dll"]
