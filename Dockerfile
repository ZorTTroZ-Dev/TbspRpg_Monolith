FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine

WORKDIR /app

#copy the code in
COPY ./TbspRpgApi /app

#build the site
RUN dotnet restore

#run the site
#RUN dotnet watch run --project ./app.csproj
ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8000"]
