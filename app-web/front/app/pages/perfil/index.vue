<template>
  <v-container class="py-6" style="max-width: 720px">
    <v-row>
      <v-col cols="12">
        <v-card elevation="2" rounded="lg">
          <!-- Skeleton de carregamento inicial -->
          <v-card-text v-if="isLoadingData" class="pa-6">
            <v-skeleton-loader type="text" class="mb-4" />
            <v-skeleton-loader type="text" class="mb-4" />
            <v-skeleton-loader type="text" class="mb-4" />
          </v-card-text>

          <!-- Formulário de edição -->
          <v-form
            v-else
            ref="formRef"
            v-model="isFormValid"
            @submit.prevent="handleSubmit"
          >
            <v-card-text class="pa-6">
              <v-text-field
                v-model="form.nome"
                label="Nome completo"
                prepend-inner-icon="mdi-account"
                variant="outlined"
                :rules="[rules.required]"
                class="mb-2"
              />

              <v-text-field
                v-model="form.email"
                label="E-mail"
                prepend-inner-icon="mdi-email"
                variant="outlined"
                type="email"
                :rules="[rules.required, rules.email]"
                class="mb-2"
              />

              <v-expansion-panels variant="accordion">
                <v-expansion-panel>
                  <v-expansion-panel-title>
                    <v-icon icon="mdi-lock-reset" class="mr-2" />
                    Alterar senha
                  </v-expansion-panel-title>
                  <v-expansion-panel-text>
                    <v-text-field
                      v-model="form.senha"
                      label="Nova senha"
                      prepend-inner-icon="mdi-lock"
                      variant="outlined"
                      :type="showPassword ? 'text' : 'password'"
                      :append-inner-icon="
                        showPassword ? 'mdi-eye-off' : 'mdi-eye'
                      "
                      @click:append-inner="showPassword = !showPassword"
                      :rules="form.senha ? [rules.minLength(6)] : []"
                      hint="Deixe em branco para manter a senha atual"
                      persistent-hint
                      class="mt-2"
                    />

                    <v-text-field
                      v-model="confirmSenha"
                      label="Confirmar nova senha"
                      prepend-inner-icon="mdi-lock-check"
                      variant="outlined"
                      :type="showPassword ? 'text' : 'password'"
                      :rules="
                        confirmSenha || form.senha
                          ? [rules.required, passwordMatchRule]
                          : []
                      "
                      class="mt-2"
                    />
                  </v-expansion-panel-text>
                </v-expansion-panel>
              </v-expansion-panels>
            </v-card-text>

            <v-divider />

            <v-card-actions class="pa-6">
              <v-spacer />
              <v-btn variant="outlined" @click="resetForm" :disabled="isSaving">
                Desfazer alterações
              </v-btn>
              <v-btn
                color="primary"
                variant="elevated"
                type="submit"
                prepend-icon="mdi-content-save"
                :loading="isSaving"
                :disabled="!isFormValid"
              >
                Salvar
              </v-btn>
            </v-card-actions>
          </v-form>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { perfilService, type PerfilForm } from "~/services/perfil.service";

definePageMeta({
  layout: "dashboard",
  title: "Meu Perfil",
  description: "Edite seus dados pessoais e credenciais de acesso.",
});

const { showSnackbar } = useSnackbar();

// Estado
const isLoadingData = ref(true);
const isSaving = ref(false);
const isFormValid = ref(false);
const showPassword = ref(false);
const formRef = ref();
const currentRole = ref("");

const form = ref<PerfilForm>({
  nome: "",
  email: "",
  senha: "",
});

const confirmSenha = ref("");

// Dados originais para restauração
const originalData = ref<PerfilForm>({
  nome: "",
  email: "",
  senha: "",
});

// Label legível para o role
const roleLabel = computed(() => {
  const map: Record<string, string> = {
    ADMIN: "Administrador",
    PSICOLOGO: "Psicólogo",
  };
  return map[currentRole.value] || currentRole.value;
});

// Regras de validação
const rules = {
  required: (v: string) => !!v || "Campo obrigatório",
  email: (v: string) => /.+@.+\..+/.test(v) || "E-mail inválido",
  minLength: (min: number) => (v: string) =>
    !v || v.length >= min || `Mínimo de ${min} caracteres`,
};

const passwordMatchRule = (v: string) =>
  v === form.value.senha || "As senhas não coincidem";

// Carrega os dados ao montar
const loadPerfil = async () => {
  isLoadingData.value = true;
  try {
    const data = await perfilService.obterMeusDados();
    form.value.nome = data.nome;
    form.value.email = data.email;
    form.value.senha = "";
    currentRole.value = data.role;

    // Salva cópia para restauração
    originalData.value = {
      nome: data.nome,
      email: data.email,
      senha: "",
    };
  } catch (error: any) {
    console.error("Erro ao carregar perfil:", error);
    if (error.response?.status !== 401 && error.response?.status !== 403) {
      showSnackbar({
        message: "Falha ao carregar dados do perfil.",
        color: "error",
      });
    }
  } finally {
    isLoadingData.value = false;
  }
};

// Submissão
const handleSubmit = async () => {
  if (!isFormValid.value) return;

  // Valida confirmação de senha se preenchida
  if (form.value.senha && form.value.senha !== confirmSenha.value) {
    showSnackbar({ message: "As senhas não coincidem.", color: "warning" });
    return;
  }

  isSaving.value = true;
  try {
    const payload: Partial<PerfilForm> = {};

    // Envia apenas campos alterados
    if (form.value.nome !== originalData.value.nome) {
      payload.nome = form.value.nome;
    }
    if (form.value.email !== originalData.value.email) {
      payload.email = form.value.email;
    }
    if (form.value.senha) {
      payload.senha = form.value.senha;
    }

    // Se nenhum campo mudou
    if (Object.keys(payload).length === 0) {
      showSnackbar({ message: "Nenhuma alteração detectada.", color: "info" });
      isSaving.value = false;
      return;
    }

    const updated = await perfilService.atualizarMeusDados(payload);

    // Atualiza o cookie user_data com os novos dados
    const userCookie = useCookie("user_data", { maxAge: 60 * 60 * 24 * 7 });
    userCookie.value = {
      id: updated.id,
      nome: updated.nome,
      email: updated.email,
      role: updated.role,
    } as any;

    // Atualiza dados originais
    originalData.value = {
      nome: updated.nome,
      email: updated.email,
      senha: "",
    };

    form.value.senha = "";
    confirmSenha.value = "";

    showSnackbar({
      message: "Perfil atualizado com sucesso!",
      color: "success",
    });
  } catch (error: any) {
    console.error("Erro ao atualizar perfil:", error);
    const detail = error.response?.data?.detail;
    showSnackbar({
      message: detail || "Falha ao atualizar perfil.",
      color: "error",
    });
  } finally {
    isSaving.value = false;
  }
};

// Restaurar dados originais
const resetForm = () => {
  form.value = { ...originalData.value };
  confirmSenha.value = "";
  showPassword.value = false;
};

onMounted(() => {
  loadPerfil();
});
</script>
