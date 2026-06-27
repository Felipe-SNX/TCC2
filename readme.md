# 🎨 TCC

Este projeto é um artefato de software desenvolvido como **Trabalho de Conclusão de Curso (TCC)** para o curso de Tecnologia em Análise e Desenvolvimento de Sistemas na **Universidade Federal do Paraná (UFPR)**.

O sistema explora o uso de conceitos da **Psicologia das Cores** e da **Teoria das Cores** como um recurso auxiliar no gerenciamento de sintomas de ansiedade e depressão.

---

## 🎯 Objetivo do Projeto

Investigar como a exposição a estímulos visuais e paletas de cores específicas, integradas à mecânica de um **jogo de plataforma 2D**, pode influenciar o estado de humor e promover sensações de calma ou relaxamento nos jogadores.

> **Importante:** O jogo e este sistema **não substituem tratamento clínico profissional**. Eles funcionam como uma ferramenta complementar e de apoio ao autocuidado, fornecendo dados analíticos para profissionais da psicologia.

---

## 🧩 Arquitetura do Sistema

O ecossistema do projeto é dividido em 3 camadas principais:

1. **Client (Jogo 2D - Externo):** Onde o paciente interage. Ao longo da gameplay, após a exposição a estímulos cromáticos, o jogo coleta respostas de humor (escala de 1 a 5) e envia para a API de forma anônima ou vinculada ao e-mail.
2. **Backend (app-web/back):** Responsável por receber os dados do jogo, autenticar os profissionais da saúde e gerenciar o armazenamento no banco de dados, garantindo as validações de dados necessárias.
3. **Frontend (app-web/front):** Interface administrativa onde **Psicólogos** autenticados acompanham a evolução dos pacientes, filtram o histórico de respostas por cor e realizam análises clínicas. **Administradores** gerenciam os acessos da plataforma.

---

## 🚀 Tecnologias Utilizadas

### Backend (API)

- **Framework:** [FastAPI](https://fastapi.tiangolo.com/) (Python 3.10+)
- **ORM & Banco de Dados:** SQLAlchemy e suporte a banco relacional (MySQL). Uso de Alembic para Migrations.
- **Autenticação:** JWT (JSON Web Tokens) com hashing de senhas bcrypt (`passlib`).

### Frontend (Dashboard)

- **Framework:** [Nuxt 4](https://nuxt.com/) (Vue 3 + TypeScript) utilizando a arquitetura focada no diretório `/app`.
- **UI & Estilização:** [Vuetify 4](https://vuetifyjs.com/) + Material Design Icons (MDI). Layouts e utilitários CSS.

---

## 📁 Estrutura de Pastas

```text
TCC2/
├── app-web/
│   ├── back/                   # API FastAPI
│   │   ├── app/
│   │   │   ├── api/            # Endpoints (Routes) e Dependências (RoleChecker, get_db)
│   │   │   ├── core/           # Configurações globais e segurança (JWT, configs)
│   │   │   ├── crud/           # Lógica de interação direta com o Banco de Dados
│   │   │   ├── db/             # Conexão SQLAlchemy e Sessões
│   │   │   ├── models/         # Modelos das tabelas (Schema Relacional)
│   │   │   └── schemas/        # Schemas Pydantic (Validação de Input/Output)
│   │   ├── alembic/            # Gerenciamento de versões do BD (Migrations)
│   │   └── main.py             # Entrypoint da API
│   │
│   ├── front/                  # Dashboard Nuxt 4
│   │   ├── app/                # Código principal da aplicação
│   │   │   ├── components/     # Componentes visuais burros (UI pura)
│   │   │   ├── composables/    # Funções de composição e estados (ex: useSnackbar)
│   │   │   ├── pages/          # Páginas inteligentes conectadas as rotas do app
│   │   │   ├── plugins/        # Setup inicial de libs (Vuetify, Axios)
│   │   │   └── services/       # Abstração para requisições externas à API
│   │   └── nuxt.config.ts      # Configurações raiz do Frontend
└── readme.md
```

---

## ⚙️ Como Executar o Projeto Localmente

### Pré-requisitos

- Python 3.10 ou superior
- Node.js 18+ (recomendado Node 24+)
- NPM

### 1. Rodando o Backend (API)

```bash
cd app-web/back

# Crie e ative um ambiente virtual
python3 -m venv venv
source venv/bin/activate  # No Windows use: venv\Scripts\activate

# Instale as dependências
pip install -r requirements.txt

# (Opcional) Execute as migrations caso seja o primeiro uso
alembic upgrade head

# Execute o servidor localmente
uvicorn app.main:app --reload --port 8000
```

> A API estará rodando em `http://localhost:8000`. Acesse a documentação interativa em `http://localhost:8000/docs`.

### 2. Rodando o Frontend (Dashboard)

```bash
cd app-web/front

# Instale as dependências
npm install

# Inicie o servidor de desenvolvimento
npm run dev
```

> O front estará acessível em `http://localhost:3000`.
