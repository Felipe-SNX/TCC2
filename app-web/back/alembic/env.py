import os
import sys
from logging.config import fileConfig
from sqlalchemy import engine_from_config
from sqlalchemy import pool
from alembic import context
from dotenv import load_dotenv

sys.path.insert(0, os.path.dirname(os.path.dirname(__file__)))

from app.db.base import Base
from app.models.schema import Usuario, Paciente, PacientePsicologo, Pergunta, Resposta

load_dotenv()

config = context.config

config.set_main_option("sqlalchemy.url", os.getenv("DATABASE_URL"))

if config.config_file_name is not None:
    fileConfig(config.config_file_name)

target_metadata = Base.metadata

