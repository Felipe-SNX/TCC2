<template>
  <v-container class="py-6">
    <div class="d-flex align-center mb-6">
      <div class="text-h5 font-weight-bold d-flex align-center">
        <span v-if="paciente" class="text-h5">{{ paciente.nome }}</span>
        <v-skeleton-loader
          v-else
          type="text"
          width="150"
          class="mt-2"
        ></v-skeleton-loader>
      </div>
    </div>

    <v-row class="mb-4" align="center">
      <v-col cols="12" sm="6" md="4" lg="2">
        <v-text-field
          v-model="dataInicio"
          label="Data Início"
          type="date"
          variant="outlined"
          density="compact"
          hide-details
          prepend-inner-icon="mdi-calendar-start"
          color="primary"
        ></v-text-field>
      </v-col>
      <v-col cols="12" sm="6" md="4" lg="2">
        <v-text-field
          v-model="dataFim"
          label="Data Fim"
          type="date"
          variant="outlined"
          density="compact"
          hide-details
          prepend-inner-icon="mdi-calendar-end"
          color="primary"
        ></v-text-field>
      </v-col>
      <v-col cols="auto" class="d-flex align-center">
        <v-tooltip text="Atualizar dados" location="top">
          <template v-slot:activator="{ props }">
            <v-btn
              color="primary"
              icon="mdi-refresh"
              variant="tonal"
              v-bind="props"
              @click="fetchData"
              :loading="isLoading"
            ></v-btn>
          </template>
        </v-tooltip>
      </v-col>
    </v-row>

    <!-- Gráfico de Respostas -->
    <v-card class="mb-6 pa-4 rounded-lg bg-surface-light border elevation-0">
      <DashboardRespostasChart :items="respostasFiltradas" />
    </v-card>

    <!-- Tabela de Histórico -->
    <DashboardHistoricoRespostasTable :items="respostasFiltradas" :loading="isLoading" />
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { useRoute } from "vue-router";
import { pacientesService } from "~/services/pacientes.service";

definePageMeta({
  layout: "dashboard",
  title: "Dashboard do Paciente",
});

const route = useRoute();
const { showSnackbar } = useSnackbar();
const pacienteId = route.params.id as string;

const paciente = ref<any>(null);
const respostas = ref<any[]>([]);
const isLoading = ref(false);

const dataInicio = ref<string>("");
const dataFim = ref<string>("");

const respostasFiltradas = computed(() => {
  if (!dataInicio.value && !dataFim.value) return respostas.value;

  return respostas.value.filter((r) => {
    if (!r.created_at) return false;

    const respDate = new Date(r.created_at)
      .toISOString()
      .split("T")[0] as string;

    const inicio = dataInicio.value || "0000-00-00";
    const fim = dataFim.value || "9999-99-99";

    return respDate >= inicio && respDate <= fim;
  });
});

const fetchPacienteInfo = async () => {
  try {
    paciente.value = await pacientesService.obter(pacienteId);
  } catch (error: any) {
    console.error("Erro ao buscar paciente:", error);
    showSnackbar({
      message: "Falha ao carregar informações do paciente.",
      color: "error",
    });
  }
};

const fetchRespostas = async () => {
  try {
    respostas.value = await pacientesService.listarRespostas(pacienteId);
  } catch (error: any) {
    console.error("Erro ao buscar respostas:", error);
    showSnackbar({
      message: "Falha ao carregar o histórico de respostas.",
      color: "error",
    });
  }
};

const fetchData = async () => {
  isLoading.value = true;
  await Promise.all([
    !paciente.value ? fetchPacienteInfo() : Promise.resolve(),
    fetchRespostas(),
  ]);
  isLoading.value = false;
};

onMounted(() => {
  const hoje = new Date();
  const seteDiasAtras = new Date();
  seteDiasAtras.setDate(hoje.getDate() - 7);

  dataInicio.value = seteDiasAtras.toISOString().split("T")[0] as string;
  dataFim.value = hoje.toISOString().split("T")[0] as string;

  fetchData();
});
</script>
