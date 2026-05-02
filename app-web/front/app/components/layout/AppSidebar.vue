<template>
  <v-navigation-drawer
    expand-on-hover
    rail
    app
    color="primary"
    style="z-index: 900"
  >
    <v-list>
      <v-list-item
        prepend-icon="mdi-account-circle"
        title="Meu Nome"
        subtitle="meu.email@exemplo.com"
        class="mb-2"
      >
        <template v-slot:append>
          <v-menu>
            <template v-slot:activator="{ props }">
              <v-btn icon="mdi-dots-vertical" variant="text" v-bind="props"></v-btn>
            </template>
            <v-list>
              <v-list-item prepend-icon="mdi-account-edit" title="Editar Dados" @click="editData"></v-list-item>
              <v-list-item prepend-icon="mdi-logout" title="Sair" @click="logout"></v-list-item>
            </v-list>
          </v-menu>
        </template>
      </v-list-item>
    </v-list>

    <v-divider></v-divider>

    <v-list density="compact" nav>
      <v-list-item prepend-icon="mdi-account-group" title="Pacientes" to="/pacientes"></v-list-item>
      <v-list-item prepend-icon="mdi-shield-account" title="Usuários" to="/usuarios"></v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { navigateTo } from '#imports'

const editData = () => {
  // Lógica de edição (placeholder)
  console.log('Editar dados clicado')
}

const logout = () => {
  // Lógica de sair
  navigateTo('/login')
}
</script>
