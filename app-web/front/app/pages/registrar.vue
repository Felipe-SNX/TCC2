<template>
  <v-container
    class="fill-height d-flex align-center justify-center bg-background"
  >
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-form
          @submit.prevent="handleRegister"
          ref="formRef"
          v-model="isFormValid"
        >
          <div
            class="d-flex flex-column ga-4 pa-6 elevation-2 rounded bg-surface"
          >
            <div class="text-h5 text-center mb-2">Criar Conta</div>
            <div class="text-body-2 text-center text-medium-emphasis mb-2">
              Preencha os dados abaixo para criar sua conta no sistema.
            </div>

            <v-text-field
              v-model="form.nome"
              label="Nome completo"
              prepend-inner-icon="mdi-account"
              :rules="nomeRules"
              variant="outlined"
              required
            ></v-text-field>

            <v-text-field
              v-model="form.email"
              label="E-mail"
              type="email"
              prepend-inner-icon="mdi-email"
              :rules="emailRules"
              variant="outlined"
              required
            ></v-text-field>

            <v-text-field
              v-model="form.senha"
              label="Senha"
              :type="showPassword ? 'text' : 'password'"
              prepend-inner-icon="mdi-lock"
              :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
              @click:append-inner="showPassword = !showPassword"
              :rules="senhaRules"
              variant="outlined"
              required
            ></v-text-field>

            <v-text-field
              v-model="confirmSenha"
              label="Confirmar senha"
              :type="showPassword ? 'text' : 'password'"
              prepend-inner-icon="mdi-lock-check"
              :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
              @click:append-inner="showPassword = !showPassword"
              :rules="confirmSenhaRules"
              variant="outlined"
              required
            ></v-text-field>

            <v-btn
              type="submit"
              color="primary"
              class="mt-2"
              block
              size="large"
              :loading="isLoading"
              :disabled="!isFormValid"
            >
              Registrar
            </v-btn>

            <div class="d-flex justify-center mt-2">
              <v-btn
                variant="text"
                size="small"
                color="secondary"
                @click="navigateTo('/login')"
              >
                Já tenho uma conta
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
const isFormValid = ref(false);
const showPassword = ref(false);
const formRef = ref<any>(null);
const { showSnackbar } = useSnackbar();

const form = ref({
  nome: "",
  email: "",
  senha: "",
});
const confirmSenha = ref("");

const nomeRules = [
  (v: string) => !!v || "O nome é obrigatório.",
  (v: string) => v.length >= 3 || "O nome deve ter no mínimo 3 caracteres.",
];

const emailRules = [
  (v: string) => !!v || "O e-mail é obrigatório.",
  (v: string) => /.+@.+\..+/.test(v) || "Insira um e-mail válido.",
];

const senhaRules = [
  (v: string) => !!v || "A senha é obrigatória.",
  (v: string) => v.length >= 6 || "A senha deve ter no mínimo 6 caracteres.",
];

const confirmSenhaRules = [
  (v: string) => !!v || "Confirme sua senha.",
  (v: string) => v === form.value.senha || "As senhas não coincidem.",
];

const handleRegister = async () => {
  if (!formRef.value) return;
  const { valid } = await formRef.value.validate();
  if (!valid) return;

  isLoading.value = true;

  try {
    await $fetch(`${config.public.apiBaseUrl}/auth/registrar`, {
      method: "POST",
      body: {
        nome: form.value.nome,
        email: form.value.email,
        senha: form.value.senha,
      },
    });

    showSnackbar({
      message: "Conta criada com sucesso! Você já pode fazer login.",
      color: "success",
      timeout: 5000,
    });

    navigateTo("/login");
  } catch (error: any) {
    const detail = error.response?._data?.detail || error.data?.detail;
    if (detail) {
      showSnackbar({ message: detail, color: "warning" });
    } else {
      showSnackbar({
        message: "Ocorreu um erro ao criar a conta. Tente novamente.",
        color: "error",
      });
    }
    console.error("Erro no registro:", error);
  } finally {
    isLoading.value = false;
  }
};
</script>
