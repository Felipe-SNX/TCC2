---
trigger: always_on
---

# Backend Architecture and API Standards

## Stack
FastAPI
Python 3.10+
SQLAlchemy

## Architecture Rules
Strict RESTful API design.
All requests and responses must be validated using Pydantic models.
Use dependency injection for database sessions.
Never store or log plaintext patient identifiers or names.

## Endpoints
POST /api/responses
GET /api/dashboard/results
POST /api/auth/psychologist

## Security Standards
Implement rate limiting on the POST /api/responses endpoint.
Validate the patient PIN against the database before accepting data.
Psychologist endpoints must require JWT authorization.