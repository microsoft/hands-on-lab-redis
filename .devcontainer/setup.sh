#!/bin/sh 

# Prepare static web app assets
npm install -g @azure/static-web-apps-cli

# Prepare functions assets
dotnet restore src/cache-refresh-func/CacheRefresh.Func.csproj
dotnet restore src/history-func/History.Api.csproj

# Prepare catalog-api assets
cd src/catalog-api 
dotnet restore && dotnet dev-certs https --trust