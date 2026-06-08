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
          <v-btn
            icon
            variant="text"
            @click="toggleTheme"
            :title="isDark ? 'Mudar para modo claro' : 'Mudar para modo escuro'"
          >
            <v-icon>{{ isDark ? 'mdi-weather-night' : 'mdi-weather-sunny' }}</v-icon>
          </v-btn>
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
        v-if="userCookie?.role === 'ADMIN'"
        prepend-icon="mdi-shield-account"
        title="Usuários"
        to="/usuarios"
      ></v-list-item>
      <v-list-item
        prepend-icon="mdi-account-cog-outline"
        title="Meu perfil"
        to="/perfil"
      ></v-list-item>
    </v-list>

    <template v-slot:append>
      <v-divider></v-divider>
      <v-list density="compact" nav>
        <v-list-item
          prepend-icon="mdi-logout"
          title="Sair"
          class="text-error"
          @click="logout"
        ></v-list-item>
      </v-list>
    </template>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { navigateTo, useCookie } from "#imports";
import { useTheme } from "vuetify";

const theme = useTheme();
const isDark = computed(() => theme.global.name.value === "dark");

const tokenCookie = useCookie("access_token");
const userCookie = useCookie<{
  id: string;
  nome: string;
  email: string;
  role: string;
} | null>("user_data");

const userName = computed(() => userCookie.value?.nome || "Usuário");
const userEmail = computed(() => userCookie.value?.email || "");

onMounted(() => {
  const savedTheme = localStorage.getItem("isDarkTheme");
  
  if (savedTheme === null) {
    setTheme(true);
  } else {
    setTheme(savedTheme === "true");
  }
});

function setTheme(dark: boolean) {
  theme.global.name.value = dark ? "dark" : "light";
  localStorage.setItem("isDarkTheme", dark.toString());
}

function toggleTheme() {
  const newTheme = !isDark.value;
  setTheme(newTheme);
}

const logout = () => {
  tokenCookie.value = null;
  userCookie.value = null;
  navigateTo("/login");
};
</script>
