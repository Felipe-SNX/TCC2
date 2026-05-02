# Database Schema

Table: Usuarios
- id (UUID, Primary Key)
- role (Enum: 'PSICOLOGO', 'ADMIN', Default: 'PSICOLOGO')
- nome (String)
- email (String, Unique)
- senha (String, Hash)
- ativo (Boolean, Default: False)

Table: Paciente
- id (UUID, Primary Key)
- nome (String)
- idade (Integer)
- genero (Enum: 'M', 'F')
- email (String, Unique)
- observacoes (Text)
- created_at (Timestamp)
- updated_at (Timestamp)
- created_by (UUID, Foreign Key -> Usuarios)
- updated_by (UUID, Foreign Key -> Usuarios)

Table: Paciente_Psicologo
- id_paciente (UUID, Foreign Key -> Paciente)
- id_usuario (UUID, Foreign Key -> Usuarios)
- Primary Key (id_paciente, id_usuario)

Table: Pergunta
- id (UUID, Primary Key)
- pergunta (String)
- alternativas (JSON)
- created_at (Timestamp)
- updated_at (Timestamp)
- created_by (UUID, Foreign Key -> Usuarios)
- updated_by (UUID, Foreign Key -> Usuarios)

Table: Resposta
- id (UUID, Primary Key)
- id_paciente (UUID, Foreign Key -> Paciente)
- resposta (Integer)
- cor (String)
- created_at (Timestamp)