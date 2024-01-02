ARG FROM_REPO=mcr.microsoft.com/dotnet
ARG DOTNET_SDK_IMAGE=sdk
ARG ASPNETCORE_RUNTIME_IMAGE=aspnet
ARG DOTNET_SDK_VERSION=6.0
ARG DOTNET_RUNTIME_VERSION=6.0

##
## sdk-image - Has the .netcore SDK required to build and run tests
##
FROM ${FROM_REPO}/${DOTNET_SDK_IMAGE}:${DOTNET_SDK_VERSION} as sdk-image

##
## aspnetcore-image - Has the .netcore runtime, required to run the service
##
FROM ${FROM_REPO}/${ASPNETCORE_RUNTIME_IMAGE}:${DOTNET_RUNTIME_VERSION} as aspnet-image

##
## Building
##
FROM sdk-image AS build

RUN mkdir -p /home/ffuser/app
WORKDIR /home/ffuser/app

# 1 - Copy sln and csprojs to make restore faster
COPY --chown=ffuser:ffuser PowerPlantService.sln PowerPlantService.sln

# 1.1 Application csprojs
COPY --chown=ffuser:ffuser src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

# 1.2 - Restore nuget packages
RUN dotnet restore PowerPlantService.sln

# 2 - Copy all files
COPY --chown=ffuser:ffuser . .

# 3 - Build
RUN dotnet build --configuration Release PowerPlantService.sln

RUN mkdir /home/ffuser/reports

# Publish
FROM build AS publish
WORKDIR /home/ffuser/app/src/Presentation
RUN dotnet publish "Presentation.csproj" --no-build -o /home/ffuser/app/publish -c Release

# Service Image
FROM aspnet-image AS final

WORKDIR /home/ffuser/app
COPY --from=publish /home/ffuser/app/publish .
ENV ASPNETCORE_URLS=http://+:8888 \
    ASPNETCORE_ENVIRONMENT=Development \
    ASPNETCORE_hostBuilder__reloadConfigOnChange=false

EXPOSE 8888
ENTRYPOINT ["dotnet", "Presentation.dll"]