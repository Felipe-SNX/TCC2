"""add colectables

Revision ID: 208c453ddc0b
Revises: 4dfabb67b33c
Create Date: 2026-06-08 01:16:54.870194

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = '208c453ddc0b'
down_revision: Union[str, Sequence[str], None] = '4dfabb67b33c'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    op.add_column('respostas', sa.Column('colectables', sa.Integer(), nullable=False, server_default='0'))


def downgrade() -> None:
    """Downgrade schema."""
    op.drop_column('respostas', 'colectables')
