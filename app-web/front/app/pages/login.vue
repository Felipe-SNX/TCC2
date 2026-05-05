<template>
  <v-container
    class="fill-height d-flex align-center justify-center bg-background"
  >
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6" lg="4">
        <AuthLoginForm
          @submit="handleLogin"
          @register="navigateTo('/registrar')"
          @forgot-password="navigateTo('/esqueci-senha')"
          :loading="isLoading"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";

const config = useRuntimeConfig();
const isLoading = ref(false);
const { showSnackbar } = useSnackbar();
const tokenCookie = useCookie("access_token", { maxAge: 60 * 60 * 24 * 7 }); // 7 dias
const userCookie = useCookie("user_data", { maxAge: 60 * 60 * 24 * 7 }); // 7 dias

const handleLogin = async (credentials: {
  email: string;
  password: string;
}) => {
  isLoading.value = true;

  try {
    const response = await $fetch<{ access_token: string; user: any }>(
      `${config.public.apiBaseUrl}/auth/login`,
      {
        method: "POST",
        body: credentials,
      },
    );

    // Salva o token e os dados do usuário (o useCookie gerencia automaticamente JSON)
    tokenCookie.value = response.access_token;
    userCookie.value = response.user;

    // Redireciona com base no papel (role) do usuário
    showSnackbar({ message: "Login realizado com sucesso!", color: "success" });

    if (response.user.role === "ADMIN") {
      navigateTo("/usuarios");
    } else if (response.user.role === "PSICOLOGO") {
      navigateTo("/pacientes");
    } else {
      navigateTo("/");
    }
  } catch (error: any) {
    if (error.response?.status === 401 || error.response?.status === 403) {
      showSnackbar({
        message: error.response._data?.detail || "E-mail ou senha incorretos.",
        color: "warning",
      });
    } else {
      showSnackbar({
        message: "Ocorreu um erro ao tentar realizar o login.",
        color: "error",
      });
    }
    console.error("Erro no login:", error);
  } finally {
    isLoading.value = false;
  }
};
</script>
