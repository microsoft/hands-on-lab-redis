#!/bin/sh 

# Prepare static web app assets
npm install -g @azure/static-web-apps-cli

# Prepare functions assets
dotnet restore src/functions/Functions.csproj

# Prepare catalog-api assets
cd src/catalog-api 
dotnet restore && dotnet dev-certs https --trust