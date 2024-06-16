@echo off
setlocal enabledelayedexpansion

cd /d %~dp0

for /r %%i in (*.csproj) do (
    :: Biến %%i chứa đường dẫn đầy đủ tới file .csproj
    echo Đang xây dựng dự án %%i ...
    dotnet build "%%i" -c Release
    if errorlevel 1 (
        echo Lỗi trong quá trình xây dựng dự án %%i
        exit /b 1
    )
)

pause