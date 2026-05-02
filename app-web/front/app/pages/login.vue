<template>
  <v-container class="fill-height d-flex align-center justify-center">
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6" lg="4">
        <!-- O Nuxt utiliza prefixo baseado em pastas para auto-import. -->
        <AuthLoginForm @submit="handleLogin" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
const handleLogin = (credentials: { email: string; password: string }) => {
  console.log('Login submetido com credenciais:', credentials)
}
</script>
