@echo off
:: Tắt echo để không hiển thị các lệnh
setlocal enabledelayedexpansion

:: Thay đổi thư mục làm việc hiện tại sang thư mục chứa file batch
cd /d %~dp0

:: Tìm tất cả các file .csproj trong thư mục hiện tại và các thư mục con
for /r %%i in (*.csproj) do (
    :: Biến %%i chứa đường dẫn đầy đủ tới file .csproj
    echo Đang xây dựng dự án %%i ...
    dotnet build "%%i" -c Release
    if errorlevel 1 (
        echo Lỗi trong quá trình xây dựng dự án %%i
        exit /b 1
    )
)

echo Hoàn thành xây dựng tất cả các dự án.
pause