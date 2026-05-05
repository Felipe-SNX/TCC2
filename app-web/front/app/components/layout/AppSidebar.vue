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
        :title="userName"
        :subtitle="userEmail"
        class="mb-2"
      >
        <template v-slot:append>
          <v-menu>
            <template v-slot:activator="{ props }">
              <v-btn
                icon="mdi-dots-vertical"
                variant="text"
                v-bind="props"
              ></v-btn>
            </template>
            <v-list>
              <v-list-item
                prepend-icon="mdi-account-edit"
                title="Editar Dados"
                @click="editData"
              ></v-list-item>
              <v-list-item
                prepend-icon="mdi-logout"
                title="Sair"
                @click="logout"
              ></v-list-item>
            </v-list>
          </v-menu>
        </template>
      </v-list-item>
    </v-list>

    <v-divider></v-divider>

    <v-list density="compact" nav>
      <v-list-item
        prepend-icon="mdi-account-group"
        title="Pacientes"
        to="/pacientes"
      ></v-list-item>
      <v-list-item
        prepend-icon="mdi-help-circle-outline"
        title="Perguntas"
        to="/perguntas"
      ></v-list-item>
      <v-list-item
        v-if="userCookie?.role === 'ADMIN'"
        prepend-icon="mdi-shield-account"
        title="Usuários"
        to="/usuarios"
      ></v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { navigateTo, useCookie } from "#imports";

const tokenCookie = useCookie("access_token");
const userCookie = useCookie<{
  id: string;
  nome: string;
  email: string;
  role: string;
} | null>("user_data");

const userName = computed(() => userCookie.value?.nome || "Usuário");
const userEmail = computed(() => userCookie.value?.email || "");

const editData = () => {
  navigateTo("/perfil");
};

const logout = () => {
  tokenCookie.value = null;
  userCookie.value = null;
  navigateTo("/login");
};
</script>
