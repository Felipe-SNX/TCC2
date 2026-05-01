---
trigger: always_on
---

# Frontend Architecture and UI Standards

## Stack
Nuxt 3
Vuetify 3
TypeScript

## Architecture Rules
Use the native Nuxt 3 directory structure.
Pages must only handle routing and high-level component composition.
Extract reusable UI elements into the components/ directory.
State management must use Nuxt's native useState or Pinia.
All API calls must use Nuxt's native $fetch or useFetch.

## UI/UX Standards
The dashboard is for psychologists. Prioritize data readability and high contrast.
Use Vuetify data tables for tabular data.
Use Vuetify cards for summary metrics.
Avoid unnecessary animations.
Ensure responsive design for desktop and tablet viewports.

## Authentication Context
Patient flow: Input 4-6 digit PIN. No email or password inputs.
Psychologist flow: Standard email/password or OAuth authentication.