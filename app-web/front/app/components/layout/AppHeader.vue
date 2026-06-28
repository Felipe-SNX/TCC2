<template>
  <v-app-bar
    app
    color="primary"
    elevation="4"
    height="60"
    style="z-index: 1000"
  >
    <div class="px-6 w-100 d-flex flex-row align-center">
      <img src="/logo-v3.png" alt="Logo" class="mr-4" style="height: 60px" />
      <div class="d-flex flex-column justify-center flex-grow-1">
        <div class="text-h5 font-weight-bold">{{ title }}</div>
        <small class="text-body-2" style="opacity: 0.8">{{
          description
        }}</small>
      </div>
      <img
        src="/espectro-label.png"
        alt="Logo"
        class="mr-4"
        style="height: 35px"
      />
    </div>
  </v-app-bar>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useRoute } from "#imports";

const route = useRoute();

const title = computed(() => route.meta.title || "Dashboard");
const description = computed(
  () => route.meta.description || "Bem-vindo ao painel de controle do sistema",
);
</script>
