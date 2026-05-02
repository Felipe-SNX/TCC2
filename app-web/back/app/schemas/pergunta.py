from pydantic import BaseModel, ConfigDict
from typing import Any, Dict, List, Optional
from datetime import datetime

class PerguntaBase(BaseModel):
    pergunta: str
    alternativas: List[Dict[str, Any]] | Dict[str, Any] | List[str]

class PerguntaCreate(PerguntaBase):
    pass

class PerguntaUpdate(BaseModel):
    pergunta: Optional[str] = None
    alternativas: Optional[Any] = None

class PerguntaResponse(PerguntaBase):
    id: str
    created_at: datetime
    updated_at: datetime
    created_by: Optional[str] = None
    updated_by: Optional[str] = None

    model_config = ConfigDict(from_attributes=True)
