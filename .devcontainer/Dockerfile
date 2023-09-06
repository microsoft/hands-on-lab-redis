FROM mcr.microsoft.com/devcontainers/dotnet:latest

RUN sudo apt-get update 

## Install Redis client toolings
RUN su vscode -c "umask 0002 && sudo apt-get install redis-tools -y 2>&1"

