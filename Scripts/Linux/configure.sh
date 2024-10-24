# install Microsoft.DotNet.SDK.8
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

apt update

apt install -y dotnet-host
apt install -y dotnet-sdk-8.0

# install docker
apt install -y ca-certificates curl
install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
chmod a+r /etc/apt/keyrings/docker.asc

# Add the repository to Apt sources:
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
apt update

apt-get install -y  docker-ce docker-ce-cli

# add pgdatabase
docker run -p 5432:5432 --name fcmspgdb -e POSTGRES_PASSWORD=password -e POSTGRES_USER=postgres -e POSTGRES_DB=fcms -d postgres

# NodeJS
apt install -y nodejs

# typescript
npm install -g typescript

:: angular
npm install -g @angular/cli
