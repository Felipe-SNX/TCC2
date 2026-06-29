@echo off
chcp 65001 >nul
echo ======================================
echo   🔄 TCC2 - Rebuild Completo Docker
echo ======================================

echo.
echo 🔍 [1 e 2/5] Encerrando e removendo containers a forca...
docker rm -f tcc_front tcc_back tcc_db >nul 2>&1
echo   Tentativa de encerramento enviada.

echo.
echo 🗑️  [3 e 4/5] Limpando volumes, containers parados e redes...
docker volume rm app-web_tcc_mysql_data >nul 2>&1
docker container prune -f >nul 2>&1
echo   Containers parados e volume do banco limpos.
docker network prune -f >nul 2>&1
echo   Redes nao utilizadas removidas.

echo.
echo 🚀 [5/5] Reconstruindo containers...

:: Verifica se a versão V2 (plugin) está disponível
docker compose version >nul 2>&1
if %ERRORLEVEL% EQU 0 goto USE_V2

:: Se chegou aqui, não achou o V2, vai tentar o V1 antigo
:USE_V1
echo   Usando docker-compose v1 (antigo)...
docker-compose build --no-cache
docker-compose up
goto END

:: Pula para cá se o V2 funcionar
:USE_V2
echo   Usando Docker Compose v2 (plugin moderno)...
docker compose build --no-cache
docker compose up

:END
echo.
echo ✅ Script finalizado.
pause