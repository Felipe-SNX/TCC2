from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.api.api import api_router

app = FastAPI(
    title="API TCC Chromotherapy",
    description="Backend para comunicação entre o jogo Unity e o Dashboard Nuxt",
    version="1.0.0"
)

# Configuração de CORS (importante para o Nuxt e Unity conseguirem acessar)
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"], # Atenção: Em produção, substitua "*" pelos domínios reais (ex: "http://localhost:3000")
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Inclui todas as rotas definidas no api_router
app.include_router(api_router, prefix="/api/v1")

@app.get("/")
def root():
    return {"message": "API TCC Chromotherapy rodando com sucesso!"}
