---
trigger: always_on
---

# Project Context: Chromotherapy TCC Dashboard

## Core Objective
This project is part of a university thesis evaluating the psychological effects of chromotherapy through a digital game. The system collects emotional responses from patients after exposure to specific colors and displays this data to psychologists for clinical analysis.

## Architecture Ecosystem
- **Client (Game):** An external game engine. It sends HTTP POST requests with the patient's email, the color displayed, and an integer representing the emotional state (1-5).
- **Backend (API):** FastAPI + MySQL. Acts as a secure bridge, validating data and mapping the patient's email to a system UUID to ensure data privacy (LGPD compliance).
- **Frontend (Dashboard):** Nuxt 3 + Vuetify. A web interface where psychologists log in to view patient evolution, filter responses by color, and analyze the psychological impact.

## System Actors
- **Patient:** Interacts only with the game. Identified by email.
- **Psychologist:** Interacts with the web dashboard. Has authentication credentials.
- **Admin:** Manages psychologist accounts.

## Critical Directives
- Simplicity over over-engineering.
- Strict RESTful patterns.
- High contrast and readability in the dashboard UI.