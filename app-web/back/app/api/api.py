from fastapi import APIRouter
from app.api.endpoints import jogo, pacientes, usuarios, auth

api_router = APIRouter()

api_router.include_router(auth.router, prefix="/auth", tags=["auth"])

api_router.include_router(jogo.router, prefix="/jogo", tags=["jogo"])

api_router.include_router(pacientes.router, prefix="/pacientes", tags=["pacientes"])
api_router.include_router(usuarios.router, prefix="/usuarios", tags=["usuarios"])
