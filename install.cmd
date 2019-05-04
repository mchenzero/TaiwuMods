@echo off

setlocal EnableDelayedExpansion

set SD=%~dp0
set SD=!SD:~0,-1!

:main
(
  call :find_game_dir || exit /b 1

  if "%1" == "/all" (
    for /r "%SD%\src" %%f in (*.csproj) do (
      call :install_mod "%%~nf" || exit /b 1
    )
  ) else if not "%1" == "" (
    call :install_mod "%1"
    exit /b !errorlevel!
  ) else (
    echo Usage: %0 ^<ModName^>
    echo        %0 /all
  )
)
exit /b

:find_game_dir
(
  if not "%TAIWU_DIR%" == "" (
    if exist "%TAIWU_DIR%\UnityPlayer.dll" (
      set GAME_DIR=!TAIWU_DIR!
    ) else (
      echo Game directory is not found. Did you set correct TAIWU_DIR environment variable?>&2
      exit /b 1
    )
  ) else (
    if exist "C:\Program Files\Steam\steamapps\common\The Scroll Of Taiwu\UnityPlayer.dll" (
      set GAME_DIR=C:\Program Files\Steam\steamapps\common\The Scroll Of Taiwu
    ) else if exist "C:\Program Files (x86)\Steam\steamapps\common\The Scroll Of Taiwu\UnityPlayer.dll" (
      set GAME_DIR=C:\Program Files ^(x86^)\Steam\steamapps\common\The Scroll Of Taiwu
    ) else (
      echo Game directory is not found. Set TAIWU_DIR environment variable to the game's installation directory.>&2
      exit /b 1
    )
  )
)
exit /b

:install_mod
(
  set MOD_NAME=

  for %%f in (!SD!\src\%1\*.csproj) do (
    set MOD_NAME=%%~nf
    set PROJ_DIR=%%~dpf
    set PROJ_DIR=!PROJ_DIR:~0,-1!
  )

  if "!MOD_NAME!" == "" (
    echo Mod "%1" is not found.>&2
    exit /b 1
  )

  dotnet build -c Release "!PROJ_DIR!" || exit /b 1

  for /f %%s in ('findstr /r "TargetFramework" "!PROJ_DIR!\!MOD_NAME!.csproj"') do (
    set TMPSTR=%%s
    set TMPSTR=!TMPSTR:*TargetFramework^>=!
    for /f "delims=<;" %%t in ("!TMPSTR!") do (
      set TARGET_FRAMEWORK=%%t
    )
  )

  set RELEASE_DIR=!PROJ_DIR!\bin\Release\!TARGET_FRAMEWORK!

  set MODS_DIR=!GAME_DIR!\Mods

  if not exist "!MODS_DIR!" (
    mkdir "!MODS_DIR!"
  )

  set INSTALL_DIR=!MODS_DIR!\!MOD_NAME!

  if not exist "!INSTALL_DIR!" (
    mkdir "!INSTALL_DIR!"
  )

  copy "!RELEASE_DIR!\Info.json" "!INSTALL_DIR!" > nul
  if !errorlevel! == 0 (
    echo Info.json -^> !INSTALL_DIR!\Info.json
  ) else (
    echo Failed to copy Info.json -^> !INSTALL_DIR!\Info.json>&2
    exit /b 1
  )

  copy "!RELEASE_DIR!\!MOD_NAME!.dll" "!INSTALL_DIR!" > nul
  if !errorlevel! == 0 (
    echo !MOD_NAME!.dll -^> !INSTALL_DIR!\!MOD_NAME!.dll
  ) else (
    echo Failed to copy !MOD_NAME!.dll -^> !INSTALL_DIR!\!MOD_NAME!.dll>&2
    exit /b 1
  )
)
exit /b
