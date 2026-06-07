"""
seed.py — Seed centralizado do banco de dados.

Ordem de execução:
  1. Usuários  (ADMIN e PSICOLOGO)
  2. Pacientes (com PIN único de 6 dígitos)
  3. Respostas (20-30 respostas falsas por paciente, últimos 30 dias)
"""

import sys
import os
import random
import string
from datetime import datetime, timedelta

# Garante que o diretório raiz do backend esteja no path
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from app.db.session import SessionLocal
from app.models.schema import Usuario, Paciente, Resposta
from app.core.security import get_password_hash


# ─────────────────────────────────────────────
# Helpers
# ─────────────────────────────────────────────

CORES = [
    "vermelho", "verde", "amarelo"
]

def data_aleatoria_ultimos_30_dias() -> datetime:
    """Retorna um datetime aleatório dentro dos últimos 30 dias."""
    dias_atras = random.randint(0, 29)
    segundos_atras = random.randint(0, 86399)  # 0 a 23h59m59s
    return datetime.utcnow() - timedelta(days=dias_atras, seconds=segundos_atras)

def gerar_pin_unico(db) -> str:
    """Gera um PIN numérico de 6 dígitos único na tabela de pacientes."""
    while True:
        pin = "".join(random.choices(string.digits, k=6))
        existe = db.query(Paciente).filter(Paciente.pin == pin).first()
        if not existe:
            return pin


# ─────────────────────────────────────────────
# 1. Usuários
# ─────────────────────────────────────────────

def seed_usuarios(db):
    print("\n📋 [1/3] Criando usuários...")

    users_to_create = [
        {
            "nome": "Administrador",
            "email": "admin@admin.com",
            "role": "ADMIN",
            "senha": "123",
        },
        {
            "nome": "Psicólogo",
            "email": "psico@psico.com",
            "role": "PSICOLOGO",
            "senha": "123",
        },
    ]

    for user_data in users_to_create:
        if db.query(Usuario).filter(Usuario.email == user_data["email"]).first():
            print(f"  ⚠️  Usuário {user_data['email']} já existe — pulando.")
            continue

        novo = Usuario(
            nome=user_data["nome"],
            email=user_data["email"],
            role=user_data["role"],
            senha=get_password_hash(user_data["senha"]),
            ativo=True,
        )
        db.add(novo)
        print(f"  ✅ {user_data['nome']} ({user_data['role']}) criado.")

    db.commit()
    print("  Usuários OK.")


# ─────────────────────────────────────────────
# 2. Pacientes
# ─────────────────────────────────────────────

def seed_pacientes(db):
    print("\n👥 [2/3] Criando pacientes...")

    psicologos = db.query(Usuario).filter(Usuario.role == "PSICOLOGO").all()
    if not psicologos:
        print("  ❌ Nenhum psicólogo encontrado — pulando criação de pacientes.")
        return

    nomes_masculinos = [
        "Gabriel", "Lucas", "Mateus", "João", "Pedro",
        "Felipe", "Enzo", "Guilherme", "Rafael", "Gustavo",
    ]
    nomes_femininos = [
        "Ana", "Julia", "Beatriz", "Maria", "Alice",
        "Laura", "Sophia", "Valentina", "Heloisa", "Manuela",
    ]
    sobrenomes = [
        "Silva", "Santos", "Oliveira", "Souza", "Rodrigues",
        "Ferreira", "Alves", "Pereira", "Lima", "Gomes",
    ]
    observacoes_templates = [
        "Apresenta sintomas de ansiedade leve em situações sociais.",
        "Demonstra interesse em atividades lúdicas durante as sessões.",
        "Relata melhora no sono após início do acompanhamento.",
        "Foco em desenvolvimento de inteligência emocional.",
        "Paciente participativo e comunicativo.",
        "Apresenta dificuldades de concentração em tarefas escolares.",
        "Em fase de observação quanto a oscilações de humor.",
        "Busca auxílio para lidar com luto recente.",
        "Trabalhando técnicas de relaxamento e mindfulness.",
        "Paciente demonstra evolução constante no tratamento.",
        "Relata dificuldades de relacionamento no ambiente de trabalho.",
        "Demonstra grande resiliência diante de desafios pessoais.",
        "Foco em melhorar a autoestima e autoconfiança.",
        "Paciente apresenta quadros ocasionais de estresse agudo.",
    ]

    total_criado = 0
    for psico in psicologos:
        print(f"  → Psicólogo: {psico.nome} ({psico.email})")
        for _ in range(40):
            sexo = random.choice(["M", "F"])
            nome = random.choice(nomes_masculinos if sexo == "M" else nomes_femininos)
            sobrenome = f"{random.choice(sobrenomes)} {random.choice(sobrenomes)}"
            nome_completo = f"{nome} {sobrenome}"

            # E-mail único
            email = f"{nome.lower()}.{sobrenome.split()[0].lower()}.{random.randint(100, 9999)}@exemplo.com"
            while db.query(Paciente).filter(Paciente.email == email).first():
                email = f"{nome.lower()}.{sobrenome.split()[0].lower()}.{random.randint(10000, 99999)}@exemplo.com"

            # PIN único de 6 dígitos
            pin = gerar_pin_unico(db)

            novo_paciente = Paciente(
                nome=nome_completo,
                idade=random.randint(6, 75),
                email=email,
                pin=pin,
                observacoes=random.choice(observacoes_templates),
                created_by=psico.id,
                updated_by=psico.id,
            )
            db.add(novo_paciente)
            db.flush()  # gera o ID antes de criar respostas
            total_criado += 1

    db.commit()
    print(f"  ✅ {total_criado} pacientes criados com sucesso.")


# ─────────────────────────────────────────────
# 3. Respostas
# ─────────────────────────────────────────────

def seed_respostas(db):
    print("\n📊 [3/3] Criando respostas falsas para os pacientes...")

    pacientes = db.query(Paciente).all()
    if not pacientes:
        print("  ❌ Nenhum paciente encontrado — pulando criação de respostas.")
        return

    # Verifica se já existem respostas para não duplicar em re-execuções
    total_existente = db.query(Resposta).count()
    if total_existente > 0:
        print(f"  ⚠️  Já existem {total_existente} respostas no banco — pulando.")
        return

    total_criado = 0
    for paciente in pacientes:
        qtd = random.randint(20, 30)
        for _ in range(qtd):
            nova_resposta = Resposta(
                id_paciente=paciente.id,
                resposta=random.randint(1, 5),
                cor=random.choice(CORES),
                created_at=data_aleatoria_ultimos_30_dias(),
            )
            db.add(nova_resposta)
            total_criado += 1

        # Flush em lote para não sobrecarregar a sessão
        db.flush()

    db.commit()
    print(f"  ✅ {total_criado} respostas criadas com sucesso.")


# ─────────────────────────────────────────────
# Orquestrador principal
# ─────────────────────────────────────────────

def run_seed():
    print("=" * 50)
    print("🌱  Iniciando seed do banco de dados...")
    print("=" * 50)

    db = SessionLocal()
    try:
        seed_usuarios(db)
        seed_pacientes(db)
        seed_respostas(db)
    except Exception as e:
        db.rollback()
        print(f"\n❌ Erro durante o seed: {e}")
        raise
    finally:
        db.close()

    print("\n" + "=" * 50)
    print("✅  Seed concluído com sucesso!")
    print("=" * 50)


if __name__ == "__main__":
    run_seed()
