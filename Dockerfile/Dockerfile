# Sử dụng image .NET phù hợp
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Fast-Food.sln", "./"]
COPY ["Fast-Food/", "Fast-Food/"]
RUN dotnet restore "./Fast-Food.sln"
RUN dotnet publish "Fast-Food/Fast-Food.csproj" -c Release -o /app/publish

# Chạy ứng dụng
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fast-Food.dll"]
