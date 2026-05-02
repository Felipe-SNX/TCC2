import sys
import os
import random

# Adiciona a raiz do backend no path para evitar erro de modulo "app" não encontrado
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from app.db.session import SessionLocal
from app.models.schema import Usuario, Paciente, PacientePsicologo

def seed_pacientes():
    db = SessionLocal()
    try:
        psicologos = db.query(Usuario).filter(Usuario.role == "PSICOLOGO").all()
        
        if not psicologos:
            print("Nenhum psicólogo encontrado para associar os pacientes!")
            return

        nomes_masculinos = ["Gabriel", "Lucas", "Mateus", "João", "Pedro", "Felipe", "Enzo", "Guilherme", "Rafael", "Gustavo"]
        nomes_femininos = ["Ana", "Julia", "Beatriz", "Maria", "Alice", "Laura", "Sophia", "Valentina", "Heloisa", "Manuela"]
        sobrenomes = ["Silva", "Santos", "Oliveira", "Souza", "Rodrigues", "Ferreira", "Alves", "Pereira", "Lima", "Gomes"]
        
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
            "Paciente apresenta quadros ocasionais de estresse agudo."
        ]

        total_criado = 0
        for psico in psicologos:
            print(f"Criando pacientes para o psicólogo: {psico.nome} ({psico.email})")
            for i in range(40):
                sexo = random.choice(["M", "F"])
                nome = random.choice(nomes_masculinos if sexo == "M" else nomes_femininos)
                sobrenome = f"{random.choice(sobrenomes)} {random.choice(sobrenomes)}"
                nome_completo = f"{nome} {sobrenome}"
                # Gera um email único baseado no nome e um número aleatório
                email = f"{nome.lower()}.{sobrenome.split()[0].lower()}.{random.randint(100, 9999)}@exemplo.com"
                
                # Garante que o e-mail seja único se já existir no banco
                while db.query(Paciente).filter(Paciente.email == email).first():
                    email = f"{nome.lower()}.{sobrenome.split()[0].lower()}.{random.randint(10000, 99999)}@exemplo.com"

                novo_paciente = Paciente(
                    nome=nome_completo,
                    idade=random.randint(6, 75),
                    email=email,
                    observacoes=random.choice(observacoes_templates),
                    created_by=psico.id,
                    updated_by=psico.id
                )
                db.add(novo_paciente)
                db.flush() # Necessário para gerar o ID do paciente antes da associação

                # Associa na tabela muitos-para-muitos
                relacao = PacientePsicologo(
                    id_paciente=novo_paciente.id,
                    id_usuario=psico.id
                )
                db.add(relacao)
                total_criado += 1
            
        db.commit()
        print(f"\nSucesso! {total_criado} pacientes foram criados e associados corretamente.")
    except Exception as e:
        db.rollback()
        print(f"Erro ao criar os pacientes: {e}")
    finally:
        db.close()

if __name__ == "__main__":
    seed_pacientes()
