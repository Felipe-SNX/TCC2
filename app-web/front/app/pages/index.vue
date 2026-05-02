<template>
  <v-container class="fill-height d-flex align-center justify-center">
    <v-progress-circular indeterminate color="primary"></v-progress-circular>
  </v-container>
</template>

<script setup lang="ts">
// Redireciona provisoriamente a rota inicial (/) para a página de login (/login)
navigateTo('/login')
</script>
