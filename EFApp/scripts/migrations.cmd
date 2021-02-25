@echo off

RD /s /q "%~dp0..\EFApp\Migrations"

dotnet ef migrations add Main ^
    --project "%~dp0..\EFApp"

cmd /c %~dp0.\postgres.cmd

dotnet ef database update ^
    --project "%~dp0..\EFApp"