:: install Microsoft.DotNet.SDK.8
winget install Microsoft.DotNet.SDK.8

:: install docker
winget install Docker.DockerDesktop
winget install Docker.DockerCLI

:: add pgdatabase
docker run -p 5432:5432 --name fcmspgdb -e POSTGRES_PASSWORD=password -e POSTGRES_USER=postgres -e POSTGRES_DB=fcms -d postgres

:: NodeJS
winget install OpenJS.NodeJS

:: typescript
winget install Microsoft.VisualStudio.Extensions.TypeScript
npm install typescript --dev-save

:: angular
npm install -g @angular/cli
