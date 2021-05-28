@echo off

for /f %%a in ('docker container ls --filter name^=sqlserver --quiet') do (
    docker container kill "%%a"
)

docker run --rm --detach ^
    --name sqlserver ^
    --publish 5432:5432 ^
    --env "ACCEPT_EULA=Y" ^
    --env "SA_PASSWORD=Password12!" ^
    mcr.microsoft.com/mssql/server:2017-latest
