from fastapi import APIRouter
from app.api.endpoints import jogo, pacientes, usuarios, perguntas, auth

api_router = APIRouter()

# Autenticação
api_router.include_router(auth.router, prefix="/auth", tags=["auth"])

# O jogo envia dados aqui
api_router.include_router(jogo.router, prefix="/jogo", tags=["jogo"])

# O Dashboard consome daqui
api_router.include_router(pacientes.router, prefix="/pacientes", tags=["pacientes"])
api_router.include_router(usuarios.router, prefix="/usuarios", tags=["usuarios"])
api_router.include_router(perguntas.router, prefix="/perguntas", tags=["perguntas"])
