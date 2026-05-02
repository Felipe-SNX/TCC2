from pydantic import BaseModel, ConfigDict
from datetime import datetime

class RespostaBase(BaseModel):
    id_paciente: str
    resposta: int
    cor: str

class RespostaCreate(RespostaBase):
    pass

# Schema específico para receber os dados do Jogo Unity
class RespostaGameCreate(BaseModel):
    email_paciente: str
    resposta: int
    cor: str

class RespostaResponse(RespostaBase):
    id: str
    created_at: datetime

    model_config = ConfigDict(from_attributes=True)
