# TCC: Plataforma de Análise de Cromoterapia

## Descrição
Sistema web de apoio a um Trabalho de Conclusão de Curso (TCC) focado em cromoterapia. A plataforma atua como o backend de um jogo desenvolvido em engine externa, recebendo dados de estado emocional de pacientes após a exposição a diferentes cores, e oferece um dashboard analítico para psicólogos.

## Tecnologias
*   **Frontend:** Nuxt 3, Vuetify 3, TypeScript.
*   **Backend:** FastAPI, SQLAlchemy, Pydantic, Python 3.10+.
*   **Banco de Dados:** MySQL 8.0.

## Estrutura de Dados
O sistema garante a integridade e privacidade dos dados operando com a seguinte modelagem:
*   `Usuarios`: Psicólogos e Administradores (Acesso ao painel).
*   `Paciente`: Dados pessoais do paciente (Nome, Idade, Email, Observações).
*   `Pergunta`: Catálogo de perguntas com pesos em JSON.
*   `Resposta`: Registro da exposição contendo identificador anônimo do paciente (UUID), a cor da fase e o peso da resposta.
*   `Paciente_Psicologo`: Relacionamento entre profissionais e pacientes.

## Requisitos
*   Docker
*   Node.js 18+
*   Python 3.10+

## Como Executar Localmente

### 1. Banco de Dados
Inicie o contêiner do MySQL via Docker (mapeado para a porta 3307 caso a 3306 esteja em uso localmente):
```bash
docker run --name tcc-mysql -e MYSQL_ROOT_PASSWORD=admin -e MYSQL_DATABASE=tcc_db -p 3307:3306 -v tcc_mysql_data:/var/lib/mysql -d mysql:8.0
```

### 2. Backend (API)
Navegue até a pasta backend, configure o ambiente e inicie o servidor:

```bash
cd backend
python -m venv venv
source venv/bin/activate
pip install -r requirements.txt
uvicorn app.main:app --reload
```
A API estará disponível em http://localhost:8000. A documentação interativa pode ser acessada em http://localhost:8000/docs.

### 3. Frontend (Dashboard)
Navegue até a pasta frontend, instale as dependências e inicie o servidor de desenvolvimento:

```bash
cd frontend
npm install
npm run dev
```
O painel estará disponível em http://localhost:3000.