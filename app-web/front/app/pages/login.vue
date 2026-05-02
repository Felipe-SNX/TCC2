<template>
  <v-container class="fill-height d-flex align-center justify-center bg-background">
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6" lg="4">
        
        <v-alert
          v-if="errorMessage"
          type="error"
          class="mb-4"
          closable
          @click:close="errorMessage = ''"
        >
          {{ errorMessage }}
        </v-alert>

        <AuthLoginForm @submit="handleLogin" :loading="isLoading" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const config = useRuntimeConfig()
const isLoading = ref(false)
const errorMessage = ref('')
const tokenCookie = useCookie('access_token', { maxAge: 60 * 60 * 24 * 7 }) // 7 dias
const userCookie = useCookie('user_data', { maxAge: 60 * 60 * 24 * 7 }) // 7 dias

const handleLogin = async (credentials: { email: string; password: string }) => {
  isLoading.value = true
  errorMessage.value = ''
  
  try {
    const response = await $fetch<{ access_token: string, user: any }>(`${config.public.apiBaseUrl}/auth/login`, {
      method: 'POST',
      body: credentials
    })
    
    // Salva o token e os dados do usuário (o useCookie gerencia automaticamente JSON)
    tokenCookie.value = response.access_token
    userCookie.value = response.user
    
    // Redireciona para o dashboard
    navigateTo('/')
  } catch (error: any) {
    if (error.response?.status === 401 || error.response?.status === 403) {
      errorMessage.value = error.response._data?.detail || 'E-mail ou senha incorretos.'
    } else {
      errorMessage.value = 'Ocorreu um erro ao tentar realizar o login. Verifique se a API está rodando.'
    }
    console.error('Erro no login:', error)
  } finally {
    isLoading.value = false
  }
}
</script>
