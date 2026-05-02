"""active_user_column

Revision ID: e4f26e91e071
Revises: 4839145c0544
Create Date: 2026-05-02 19:46:21.174414

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'e4f26e91e071'
down_revision: Union[str, Sequence[str], None] = '4839145c0544'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    # Adiciona a coluna ativo com default false
    op.add_column('usuarios', sa.Column('ativo', sa.Boolean(), server_default=sa.text('0'), nullable=False))
    
    # Define o valor default da coluna role como 'PSICOLOGO'
    op.alter_column('usuarios', 'role', server_default='PSICOLOGO')


def downgrade() -> None:
    """Downgrade schema."""
    # Remove o valor default da coluna role
    op.alter_column('usuarios', 'role', server_default=None)
    
    # Remove a coluna ativo
    op.drop_column('usuarios', 'ativo')
