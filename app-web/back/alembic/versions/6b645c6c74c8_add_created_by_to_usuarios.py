"""add_created_by_to_usuarios

Revision ID: 6b645c6c74c8
Revises: 208c453ddc0b
Create Date: 2026-06-28 15:44:09.889963

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


revision: str = '6b645c6c74c8'
down_revision: Union[str, Sequence[str], None] = '208c453ddc0b'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.add_column('usuarios', sa.Column('created_by', sa.String(length=36), nullable=True))
    op.create_foreign_key(None, 'usuarios', 'usuarios', ['created_by'], ['id'])


def downgrade() -> None:
    """Downgrade schema."""
    op.drop_constraint(None, 'usuarios', type_='foreignkey')
    op.drop_column('usuarios', 'created_by')
