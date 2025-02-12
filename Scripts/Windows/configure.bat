:: install Microsoft.DotNet.SDK.8
winget install --silent --accept-source-agreements Microsoft.DotNet.SDK.8

:: install docker
winget install --silent --accept-source-agreements Docker.DockerDesktop
winget install --silent --accept-source-agreements Docker.DockerCLI

:: add pgdatabase
docker run -p 5432:5432 --name fcmspgdb -e POSTGRES_PASSWORD=password -e POSTGRES_USER=postgres -e POSTGRES_DB=fcms -d postgres

:: NodeJS
winget install --silent --accept-source-agreements OpenJS.NodeJS -v 20.0.0

:: typescript
winget install Microsoft.VisualStudio.Extensions.TypeScript
npm install typescript --dev-save

:: angular
npm install -g @angular/cli
