from fastapi import APIRouter, Depends, HTTPException, status, Query
from sqlalchemy.orm import Session
from typing import List
from app.api.dependencies import get_db, get_current_user, RoleChecker
from app.schemas.paciente import PacienteCreate, PacienteUpdate, PacienteResponse, PacientePaginatedResponse
from app.schemas.resposta import RespostaResponse
from app.models.schema import Usuario
from app.crud import crud_paciente, crud_resposta

router = APIRouter()
allow_psicologo_admin = RoleChecker(["ADMIN", "PSICOLOGO"])

@router.post("/", response_model=PacienteResponse, status_code=status.HTTP_201_CREATED)
def criar_paciente(paciente_in: PacienteCreate, db: Session = Depends(get_db), current_user: Usuario = Depends(get_current_user)):
    paciente_existente = crud_paciente.get_paciente_by_email(db, email=paciente_in.email)
    if paciente_existente:
        raise HTTPException(status_code=400, detail="Email já cadastrado.")
    return crud_paciente.create_paciente(db=db, paciente=paciente_in, user_id=current_user.id)

@router.get("/", response_model=PacientePaginatedResponse)
def listar_pacientes(
    page: int = Query(1, ge=1), 
    items_per_page: int = Query(25, ge=1, le=100),
    search: str = Query(None, description="Buscar por nome ou e-mail"),
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(get_current_user)
):
    skip = (page - 1) * items_per_page
    items = crud_paciente.get_pacientes(db=db, skip=skip, limit=items_per_page, user_id=current_user.id, user_role=current_user.role, user_created_by=current_user.created_by, search=search)
    total = crud_paciente.get_pacientes_count(db=db, user_id=current_user.id, user_role=current_user.role, user_created_by=current_user.created_by, search=search)
    return {"items": items, "total": total}

@router.get("/{paciente_id}", response_model=PacienteResponse)
def obter_paciente(paciente_id: str, db: Session = Depends(get_db), current_user: Usuario = Depends(allow_psicologo_admin)):
    paciente = crud_paciente.get_paciente(db, paciente_id=paciente_id)
    if not paciente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado.")
    if current_user.role == "PSICOLOGO" and paciente.created_by != current_user.id:
        raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Este paciente pertence a outro psicólogo.")
    if current_user.role == "ADMIN" and current_user.created_by is not None:
        visible_user_ids = [current_user.id, current_user.created_by]
        # Allow if created by current user, or created by a user visible to current user
        # crud_paciente is handling this logic, but for individual access we do a manual check.
        # It's easier to check if the paciente's creator is in the visible list.
        # visible list: users created by current_user or by current_user's creator
        from app.models.schema import Usuario
        visible_users = db.query(Usuario.id).filter(Usuario.created_by.in_([current_user.id, current_user.created_by])).all()
        valid_creator_ids = [current_user.id] + [r[0] for r in visible_users]
        
        if paciente.created_by not in valid_creator_ids:
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Você não tem permissão para visualizar este paciente.")
            
    return paciente

@router.put("/{paciente_id}", response_model=PacienteResponse)
def atualizar_paciente(
    paciente_id: str,
    paciente_in: PacienteUpdate,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    paciente_existente = crud_paciente.get_paciente(db, paciente_id=paciente_id)
    if not paciente_existente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado.")
    if current_user.role == "PSICOLOGO" and paciente_existente.created_by != current_user.id:
        raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Este paciente pertence a outro psicólogo.")
    if current_user.role == "ADMIN" and current_user.created_by is not None:
        from app.models.schema import Usuario
        visible_users = db.query(Usuario.id).filter(Usuario.created_by.in_([current_user.id, current_user.created_by])).all()
        valid_creator_ids = [current_user.id] + [r[0] for r in visible_users]
        
        if paciente_existente.created_by not in valid_creator_ids:
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Você não tem permissão para editar este paciente.")
    return crud_paciente.update_paciente(db=db, paciente_id=paciente_id, paciente_in=paciente_in, user_id=current_user.id)

@router.delete("/{paciente_id}", status_code=status.HTTP_204_NO_CONTENT)
def excluir_paciente(
    paciente_id: str,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    paciente_existente = crud_paciente.get_paciente(db, paciente_id=paciente_id)
    if not paciente_existente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado.")
    if current_user.role == "PSICOLOGO" and paciente_existente.created_by != current_user.id:
        raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Este paciente pertence a outro psicólogo.")
    if current_user.role == "ADMIN" and current_user.created_by is not None:
        from app.models.schema import Usuario
        visible_users = db.query(Usuario.id).filter(Usuario.created_by.in_([current_user.id, current_user.created_by])).all()
        valid_creator_ids = [current_user.id] + [r[0] for r in visible_users]
        
        if paciente_existente.created_by not in valid_creator_ids:
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Você não tem permissão para excluir este paciente.")
    crud_paciente.delete_paciente(db=db, paciente_id=paciente_id)

@router.patch("/{paciente_id}/pin", response_model=PacienteResponse)
def renovar_pin_paciente(
    paciente_id: str,
    db: Session = Depends(get_db),
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    """
    Gera um novo PIN aleatório para o paciente.
    """
    paciente_existente = crud_paciente.get_paciente(db, paciente_id=paciente_id)
    if not paciente_existente:
        raise HTTPException(status_code=404, detail="Paciente não encontrado.")
    if current_user.role == "PSICOLOGO" and paciente_existente.created_by != current_user.id:
        raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Este paciente pertence a outro psicólogo.")
    if current_user.role == "ADMIN" and current_user.created_by is not None:
        from app.models.schema import Usuario
        visible_users = db.query(Usuario.id).filter(Usuario.created_by.in_([current_user.id, current_user.created_by])).all()
        valid_creator_ids = [current_user.id] + [r[0] for r in visible_users]
        
        if paciente_existente.created_by not in valid_creator_ids:
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Você não tem permissão para editar este paciente.")
    
    paciente_atualizado = crud_paciente.regenerate_pin(db=db, paciente_id=paciente_id)
    return paciente_atualizado

@router.get("/{paciente_id}/respostas", response_model=List[RespostaResponse])
def listar_respostas_do_paciente(
    paciente_id: str, 
    db: Session = Depends(get_db), 
    current_user: Usuario = Depends(allow_psicologo_admin)
):
    """
    Endpoint para o psicólogo ver o histórico de respostas do paciente.
    """
    if current_user.role == "PSICOLOGO":
        paciente = crud_paciente.get_paciente(db, paciente_id=paciente_id)
        if not paciente or paciente.created_by != current_user.id:
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Este paciente pertence a outro psicólogo.")
    elif current_user.role == "ADMIN" and current_user.created_by is not None:
        paciente = crud_paciente.get_paciente(db, paciente_id=paciente_id)
        if not paciente:
             raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Paciente não encontrado.")
        from app.models.schema import Usuario
        visible_users = db.query(Usuario.id).filter(Usuario.created_by.in_([current_user.id, current_user.created_by])).all()
        valid_creator_ids = [current_user.id] + [r[0] for r in visible_users]
        
        if paciente.created_by not in valid_creator_ids:
            raise HTTPException(status_code=status.HTTP_403_FORBIDDEN, detail="Acesso negado. Você não tem permissão para visualizar este paciente.")
    return crud_resposta.get_respostas_by_paciente(db, paciente_id=paciente_id)
