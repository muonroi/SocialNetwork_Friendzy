@echo off
setlocal enabledelayedexpansion

:: Di chuyển tới thư mục hiện tại của script
cd /d %~dp0

:: Xóa Docker build cache
echo Xóa Docker build cache...
docker builder prune --all --force
if errorlevel 1 (
    echo Lỗi trong quá trình xóa Docker build cache
    exit /b 1
)

:: Xóa tất cả Docker images
echo Xóa tất cả Docker images...
docker rmi $(docker images -q) --force
if errorlevel 1 (
    echo Lỗi trong quá trình xóa Docker images
    exit /b 1
)

:: Xóa tất cả Docker containers
echo Xóa tất cả Docker containers...
docker rm $(docker ps -a -q) --force
if errorlevel 1 (
    echo Lỗi trong quá trình xóa Docker containers
    exit /b 1
)

:: Xóa tất cả Docker volumes (nếu cần)
echo Xóa tất cả Docker volumes...
docker volume rm $(docker volume ls -q) --force
if errorlevel 1 (
    echo Lỗi trong quá trình xóa Docker volumes
    exit /b 1
)

:: Tìm và build tất cả các dự án .csproj
for /r %%i in (*.csproj) do (
    :: Biến %%i chứa đường dẫn đầy đủ tới file .csproj
    echo Đang xây dựng dự án %%i ...
    dotnet build "%%i" -c Release
    if errorlevel 1 (
        echo Lỗi trong quá trình xây dựng dự án %%i
        exit /b 1
    )
)

:: Chạy docker-compose để xây dựng lại từ đầu
echo Chạy docker-compose để xây dựng lại từ đầu...
docker-compose -f ./docker-compose.yml -f ./docker-compose.override.yml up -d --remove-orphans --build
if errorlevel 1 (
    echo Lỗi trong quá trình chạy docker-compose
    exit /b 1
)

pause
