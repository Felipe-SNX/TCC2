<template>
  <v-container
    class="fill-height d-flex align-center justify-center bg-background"
  >
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-form @submit.prevent="handleSubmit" ref="formRef">
          <div
            class="d-flex flex-column ga-4 pa-6 elevation-2 rounded bg-surface"
          >
            <div class="text-h5 text-center mb-2">Recuperar Senha</div>
            <div class="text-body-2 text-center text-medium-emphasis mb-2">
              Informe seu e-mail cadastrado para receber um link de redefinição
              de senha.
            </div>

            <v-text-field
              v-model="email"
              label="E-mail"
              type="email"
              prepend-inner-icon="mdi-email"
              :rules="emailRules"
              variant="outlined"
              required
              :disabled="enviado"
            ></v-text-field>

            <v-alert
              v-if="enviado"
              type="info"
              variant="tonal"
              density="compact"
              icon="mdi-email-check"
            >
              Verifique sua caixa de entrada. Se o seu e-mail existe no nosso
              sistema, você receberá um link para recriar sua senha.
            </v-alert>

            <v-btn
              v-if="!enviado"
              type="submit"
              color="primary"
              class="mt-2"
              block
              size="large"
              :loading="isLoading"
            >
              Enviar
            </v-btn>

            <div class="d-flex justify-center mt-2">
              <v-btn
                variant="text"
                size="small"
                color="secondary"
                @click="navigateTo('/login')"
              >
                Voltar ao login
              </v-btn>
            </div>
          </div>
        </v-form>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";

const config = useRuntimeConfig();
const isLoading = ref(false);
const enviado = ref(false);
const formRef = ref<any>(null);
const email = ref("");

// TODO: olha rules solta auqui
const emailRules = [
  (v: string) => !!v || "O e-mail é obrigatório.",
  (v: string) => /.+@.+\..+/.test(v) || "Insira um e-mail válido.",
];

const handleSubmit = async () => {
  if (!formRef.value) return;
  const { valid } = await formRef.value.validate();
  if (!valid) return;

  isLoading.value = true;

  try {
    await $fetch(`${config.public.apiBaseUrl}/auth/esqueci-senha`, {
      method: "POST",
      body: { email: email.value },
    });
  } catch (error: any) {
    // Mesmo em caso de erro de rede, exibimos a mensagem para não revelar informações
    console.error("Erro ao solicitar redefinição:", error);
  } finally {
    isLoading.value = false;
    enviado.value = true;
  }
};
</script>
