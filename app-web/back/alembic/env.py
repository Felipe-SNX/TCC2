import os
import sys
from logging.config import fileConfig
from sqlalchemy import engine_from_config
from sqlalchemy import pool
from alembic import context
from dotenv import load_dotenv

# 1. Configura o caminho para o Alembic enxergar a pasta 'app'
sys.path.insert(0, os.path.dirname(os.path.dirname(__file__)))

# 2. Importa a Base e todas as tabelas para o autogenerate mapeá-las
from app.db.base import Base
from app.models.schema import Usuario, Paciente, PacientePsicologo, Pergunta, Resposta

# 3. Força a leitura do arquivo .env a partir da raiz do backend
env_path = os.path.join(os.path.dirname(os.path.dirname(__file__)), '.env')
load_dotenv(dotenv_path=env_path)

# Carrega a configuração do alembic.ini
config = context.config

# 4. Extrai a URL do banco do .env e injeta no Alembic
db_url = os.getenv("DATABASE_URL")
if not db_url:
    raise ValueError("A variável DATABASE_URL não foi encontrada. Verifique o arquivo .env.")

config.set_main_option("sqlalchemy.url", db_url)

# Configura o sistema de logs
if config.config_file_name is not None:
    fileConfig(config.config_file_name)

# 5. Define os metadados alvo (Isso é o que faz o Alembic comparar o código com o banco)
target_metadata = Base.metadata

def run_migrations_offline() -> None:
    """Executa as migrações em modo offline (sem conectar ao banco)."""
    url = config.get_main_option("sqlalchemy.url")
    context.configure(
        url=url,
        target_metadata=target_metadata,
        literal_binds=True,
        dialect_opts={"paramstyle": "named"},
    )

    with context.begin_transaction():
        context.run_migrations()

def run_migrations_online() -> None:
    """Executa as migrações em modo online (conectando ao banco)."""
    connectable = engine_from_config(
        config.get_section(config.config_ini_section, {}),
        prefix="sqlalchemy.",
        poolclass=pool.NullPool,
    )

    with connectable.connect() as connection:
        context.configure(
            connection=connection, target_metadata=target_metadata
        )

        with context.begin_transaction():
            context.run_migrations()

if context.is_offline_mode():
    run_migrations_offline()
else:
    run_migrations_online()