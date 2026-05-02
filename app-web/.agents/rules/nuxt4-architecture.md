---
trigger: always_on
---

# Nuxt 4 Architecture Context

## Diretório Base de Aplicação (`app/`)
Diferente das versões anteriores do Nuxt onde as pastas de código ficavam na raiz, a arquitetura Nuxt 4 exige que a aplicação central fique isolada dentro do diretório `app/`.

### Estrutura de Pastas Padrão:
```text
front/
├── app/                  # Código principal da aplicação (Nuxt 4 Standard)
│   ├── assets/           # Arquivos estáticos compilados (SCSS, fontes)
│   ├── components/       # Componentes Vue (dummies e visuais)
│   ├── composables/      # Lógica reaproveitável (Composition API)
│   ├── layouts/          # Layouts base (ex: admin, psicologo, default)
│   ├── pages/            # Rotas da aplicação (file-based routing)
│   ├── plugins/          # Plugins Nuxt (ex: setup do Vuetify)
│   ├── utils/            # Funções utilitárias globais
│   └── app.vue           # Entrypoint da aplicação Vue
├── public/               # Arquivos estáticos expostos diretamente na raiz web
├── server/               # (Se necessário) Rotas de API internas do Nuxt (Nitro)
├── nuxt.config.ts        # Configuração do Nuxt e módulos (Vuetify, etc)
└── package.json          # Dependências
```

## Regras Críticas para Nuxt 4 neste Projeto:
1. **Nunca** crie as pastas `pages/`, `components/`, `layouts/` na raiz do projeto `front/`. Elas devem residir estritamente dentro de `front/app/`.
2. O arquivo `app.vue` já foi migrado e deve permanecer em `front/app/app.vue`.
3. A configuração do Vuetify 4 já está ativa no `nuxt.config.ts` através do `vite-plugin-vuetify` e não requer pastas redundantes na raiz.
4. Auto-imports nativos do Nuxt 4 englobam tudo dentro de `app/components/`, `app/composables/` e `app/utils/`. Não utilize imports manuais desnecessários nestes casos.
