FROM mcr.microsoft.com/devcontainers/dotnet:6.0-jammy

RUN sudo apt-get update 

## Install Redis client toolings
RUN su vscode -c "umask 0002 && sudo apt-get install redis-tools -y 2>&1"

# RUN sudo apt-get install dotnet-sdk-7.0 -y
## Install Dotnet 7.0
env DOTNET_ROOT=/usr/share/dotnet
env PATH=$PATH:usr/share/dotnet
env DOTNET_SDK_MAJOR_VERSION=7.0

RUN wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
    && chmod +x ./dotnet-install.sh \
    && ./dotnet-install.sh --channel $DOTNET_SDK_MAJOR_VERSION --install-dir $DOTNET_ROOT 