FROM mcr.microsoft.com/devcontainers/dotnet:7.0-jammy

# RUN curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg \
#     sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg

# RUN sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-$(lsb_release -cs)-prod $(lsb_release -cs) main" > /etc/apt/sources.list.d/dotnetdev.list'

RUN sudo apt-get update 

## Install Redis client toolings
RUN su vscode -c "umask 0002 && sudo apt-get install redis-tools -y 2>&1"

## Install Azure Function Core Tools
# RUN sudo apt-get install azure-functions-core-tools-4

# RUN sudo apt-get install dotnet-sdk-7.0 -y
## Install Dotnet 7.0
# env DOTNET_ROOT=/usr/share/dotnet
# env PATH=$PATH:usr/share/dotnet
# env DOTNET_SDK_MAJOR_VERSION=7.0

# RUN wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
#     && chmod +x ./dotnet-install.sh \
#     && ./dotnet-install.sh --channel $DOTNET_SDK_MAJOR_VERSION --install-dir $DOTNET_ROOT 