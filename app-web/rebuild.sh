#!/bin/bash

set -e

echo "======================================"
echo "  🔄 TCC2 - Rebuild Completo Docker"
echo "======================================"

# 1. Descobrir o PID real de cada container no sistema (ignora erros se não existirem)
echo ""
echo "🔍 [1/5] Buscando PIDs dos containers..."

PID_FRONT=$(sudo docker inspect -f '{{.State.Pid}}' tcc_front 2>/dev/null || echo "")
PID_BACK=$(sudo docker inspect -f '{{.State.Pid}}' tcc_back 2>/dev/null || echo "")
PID_DB=$(sudo docker inspect -f '{{.State.Pid}}' tcc_db 2>/dev/null || echo "")

echo "  PIDs encontrados -> Front: ${PID_FRONT:-N/A} | Back: ${PID_BACK:-N/A} | DB: ${PID_DB:-N/A}"

# 2. Matar os processos à força (sem perguntar pro Docker)
echo ""
echo "💀 [2/5] Encerrando processos à força..."

PIDS_TO_KILL=""
[ -n "$PID_FRONT" ] && [ "$PID_FRONT" != "0" ] && PIDS_TO_KILL="$PIDS_TO_KILL $PID_FRONT"
[ -n "$PID_BACK" ]  && [ "$PID_BACK"  != "0" ] && PIDS_TO_KILL="$PIDS_TO_KILL $PID_BACK"
[ -n "$PID_DB" ]    && [ "$PID_DB"    != "0" ] && PIDS_TO_KILL="$PIDS_TO_KILL $PID_DB"

if [ -n "$PIDS_TO_KILL" ]; then
    sudo kill -9 $PIDS_TO_KILL 2>/dev/null && echo "  Processos encerrados: $PIDS_TO_KILL" || echo "  Nenhum processo ativo para encerrar."
else
    echo "  Nenhum PID ativo encontrado. Pulando..."
fi

# 3. Remover containers (agora o Docker consegue, pois os processos já morreram)
echo ""
echo "🗑️  [3/5] Removendo containers..."
sudo docker rm -f tcc_front tcc_back tcc_db 2>/dev/null || echo "  Containers já removidos ou inexistentes."

# 4. Remover volume do banco e rede
echo ""
echo "🧹 [4/5] Limpando volumes e redes..."

docker volume rm app-web_tcc_mysql_data 2>/dev/null && echo "  Volume 'app-web_tcc_mysql_data' removido." || echo "  Volume não encontrado ou já removido."
docker network prune -f && echo "  Redes não utilizadas removidas."

# 5. Rebuild e subir tudo do zero
echo ""
echo "🚀 [5/5] Reconstruindo e subindo containers..."
docker-compose build --no-cache
docker-compose up

echo ""
echo "✅ Rebuild concluído!"
