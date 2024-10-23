:: install Microsoft.DotNet.SDK.8
winget install Microsoft.DotNet.SDK.8

:: install docker
winget install Docker.DockerDesktop
winget install Docker.DockerCLI

:: add pgdatabase
docker run --name fcmspgdb -e POSTGRES_PASSWORD=mysecretpassword -d postgresdb

:: NodeJS
winget install OpenJS.NodeJS

:: typescript
winget install Microsoft.VisualStudio.Extensions.TypeScript
npm install typescript --dev-save

:: angular
npm install -g @angular/cli
