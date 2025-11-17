``` shell
sudo apt install dotnet-sdk-8.0 dotnet8 dotnet-sdk-dbg-8.0
cd TbspRpg_Monolith/TbspRpgDataLayer
dotnet add package Microsoft.EntityFrameworkCore
dotnet tool install --global dotnet-ef --version 8.0.0
Add PATH="$HOME/.dotnet/tools:/usr/pgadmin4/bin/:$PATH" to ~/.bashrc
```

Install Postgres 18
```shell
sudo apt update
sudo apt install curl gnupg2 -y
curl -fsSL https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo gpg --dearmor -o /etc/apt/trusted.gpg.d/apt.postgresql.org.gpg
echo "deb http://apt.postgresql.org/pub/repos/apt/ $(lsb_release -cs)-pgdg main" | sudo tee /etc/apt/sources.list.d/pgdg.list
sudo apt install postgresql-18 postgresql-client-18 postgresql-doc-18
sudo curl https://www.pgadmin.org/static/packages_pgadmin_org.pub | sudo apt-key add -
sudo sh -c 'echo "deb https://ftp.postgresql.org/pub/pgadmin/pgadmin4/apt/$(lsb_release -cs) pgadmin4 main" > /etc/apt/sources.list.d/pgadmin4.list'
sudo apt install pgadmin4-desktop
```

Setup the database
```shell
sudo systemctl enable postgresql
sudo -u postgres psql
CREATE USER tbsprpg_api WITH ENCRYPTED PASSWORD 'tbsprpg_api';
CREATE DATABASE tbsprpgapi;
GRANT ALL PRIVILEGES ON DATABASE "tbsprpgapi" to tbsprpg_api;
\c tbsprpgapi
GRANT ALL PRIVILEGES ON SCHEMA public TO tbsprpg_api;
```

Configure pgadmin4, add new server

Populate the database
```shell

```
