"""ativo_perguntas_pacientes

Revision ID: 0dd2019bd3b3
Revises: e4f26e91e071
Create Date: 2026-05-05 20:13:17.951749

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '0dd2019bd3b3'
down_revision: Union[str, Sequence[str], None] = 'e4f26e91e071'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.add_column('perguntas', sa.Column('ativo', sa.Boolean(), nullable=False, server_default=sa.text('1')))


def downgrade() -> None:
    """Downgrade schema."""
    op.drop_column('perguntas', 'ativo')
