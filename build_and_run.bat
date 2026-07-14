@echo off
chcp 65001 >nul
echo ========================================
echo Modbus SCADA 监控系统 - 自动构建脚本
echo ========================================
echo.

REM 设置项目路径
set PROJECT_DIR=%~dp0
set SOLUTION_FILE=%PROJECT_DIR%ModbusScada.sln
set CONFIG=Debug
set EXE_PATH=%PROJECT_DIR%bin\%CONFIG%\ModbusScada.exe

echo [1/3] 查找 MSBuild...

REM 尝试直接使用 msbuild（如果已在 PATH 中）
where msbuild >nul 2>&1
if %ERRORLEVEL% equ 0 (
    set MSBUILD_PATH=msbuild
    echo ✓ 在系统 PATH 中找到 MSBuild
    goto :build
)

REM 尝试查找 Visual Studio 2022
if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    echo ✓ 找到 Visual Studio 2022 Community 的 MSBuild
    goto :build
)

if exist "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    echo ✓ 找到 Visual Studio 2022 Professional 的 MSBuild
    goto :build
)

if exist "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
    echo ✓ 找到 Visual Studio 2022 Enterprise 的 MSBuild
    goto :build
)

REM 尝试查找 Visual Studio 2019
if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
    echo ✓ 找到 Visual Studio 2019 Community 的 MSBuild
    goto :build
)

if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
    echo ✓ 找到 Visual Studio 2019 Professional 的 MSBuild
    goto :build
)

if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
    echo ✓ 找到 Visual Studio 2019 Enterprise 的 MSBuild
    goto :build
)

REM 尝试通过 vswhere 工具查找（VS 2017+ 自带）
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
    for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
        if exist "%%i\MSBuild\Current\Bin\MSBuild.exe" (
            set MSBUILD_PATH="%%i\MSBuild\Current\Bin\MSBuild.exe"
            echo ✓ 通过 vswhere 找到最新版的 MSBuild
            goto :build
        )
    )
)

echo 错误: 未找到 MSBuild
echo.
echo 请确保已安装 Visual Studio，或使用以下方法之一：
echo   1. 在 "Developer PowerShell for VS" 中运行此脚本
echo   2. 将 MSBuild 添加到系统 PATH 环境变量
echo   3. 手动指定 MSBuild 路径
echo.
pause
exit /b 1

:build
echo.
echo [2/3] 清理并重新生成项目...
%MSBUILD_PATH% "%SOLUTION_FILE%" /t:Clean /p:Configuration=%CONFIG% /v:quiet
if %ERRORLEVEL% neq 0 (
    echo 错误: 清理项目失败
    pause
    exit /b 1
)

%MSBUILD_PATH% "%SOLUTION_FILE%" /t:Rebuild /p:Configuration=%CONFIG% /v:minimal
if %ERRORLEVEL% neq 0 (
    echo 错误: 重新生成项目失败
    pause
    exit /b 1
)
echo ✓ 项目重新生成成功
echo.

echo [3/3] 检查可执行文件...
if not exist "%EXE_PATH%" (
    echo 错误: 未找到可执行文件 %EXE_PATH%
    pause
    exit /b 1
)
echo ✓ 可执行文件已就绪
echo.

echo ========================================
echo 构建完成！正在启动程序...
echo ========================================
echo.

start "" "%EXE_PATH%"

echo 程序已启动
echo.
timeout /t 2 >nul
exit /b 0