---
trigger: always_on
---

# Frontend Architecture and UI Standards (Nuxt 4 & Vuetify 4)

## Stack & Language
- **Framework:** Nuxt 4 (utilizando novos padrões de diretórios e camadas).
- **UI Framework:** Vuetify 4.
- **Icons:** Material Design Icons (MDI).
- **Language:** TypeScript.
- **IDIOMA OBRIGATÓRIO:** Todas as respostas, explicações e comentários de código devem ser em **Português do Brasil (pt-BR)**.
- **COMMITS:** Use estritamente Conventional Commits em **Português**.

## Architecture Rules (Componentization)
- **Componentes Dummies:** Todos os componentes na pasta `components/` devem ser estritamente burros (dummies). Eles apenas exibem informações via `props` e notificam ações via `emits`.
- **Lógica Centralizada:** Chamadas de API (`$fetch`, `useFetch`), manipulação de estado (Pinia/useState) e lógica de negócio devem ficar exclusivamente no **componente pai** ou nas **páginas**.
- Use as novas convenções de pastas do Nuxt 4 para separação de preocupações.

## UI/UX & Styling Standards
- **MDI Icons:** Use exclusivamente ícones do Material Design Icons (MDI) seguindo a sintaxe do Vuetify (ex: `v-icon="mdi-account"`).
- **Vuetify First:** Use componentes do Vuetify 4 para todos os elementos de interface.
- **Zero Custom CSS:** É proibido o uso de blocos `<style>` ou CSS puro. 
- **Utility Classes:** Use as propriedades de classe utilitárias do Vuetify diretamente nos componentes (ex: `d-flex`, `pa-4`, `mt-2`, `text-h5`, `elevation-2`) para controlar layout e espaçamento.
- Dashboard para psicólogos: Foco em contraste e legibilidade de dados.

## Authentication Context
- Pacientes: Login via PIN de 4-6 dígitos.
- Psicólogos: Login via Email/Senha ou OAuth.