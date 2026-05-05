<template>
  <v-container class="py-6">
    <div class="d-flex align-center mb-6">
      <div class="text-h5 font-weight-bold d-flex align-center">
        <v-btn icon="mdi-arrow-left" variant="text" size="small" class="mr-2" @click="navigateTo('/pacientes')"></v-btn>
        <span class="text-medium-emphasis text-body-1 mr-2" style="cursor: pointer;" @click="navigateTo('/pacientes')">Pacientes</span>
        <v-icon size="small" class="mr-2 text-medium-emphasis">mdi-chevron-right</v-icon>
        <span v-if="paciente" class="text-h5">{{ paciente.nome }}</span>
        <v-skeleton-loader v-else type="text" width="150" class="mt-2"></v-skeleton-loader>
      </div>
    </div>

    <v-row class="mb-4" align="center">
      <v-col cols="12" sm="6" md="4" lg="3">
        <v-text-field
          v-model="dataFiltro"
          label="Filtrar por data"
          type="date"
          variant="outlined"
          density="compact"
          hide-details
          prepend-inner-icon="mdi-calendar"
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

    <!-- O Gráfico será inserido aqui futuramente -->
    <v-card class="mb-6 pa-4 d-flex align-center justify-center rounded-lg bg-surface-light border" style="min-height: 200px; border-style: dashed !important;">
      <div class="text-center text-medium-emphasis">
        <v-icon size="large" class="mb-2">mdi-chart-bar</v-icon>
        <div>[Área reservada para o Gráfico de Respostas]</div>
      </div>
    </v-card>

    <!-- Tabela de Histórico -->
    <HistoricoRespostasTable
      :items="respostasFiltradas"
      :loading="isLoading"
    />
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
const dataFiltro = ref<string>("");

// Computed para filtrar as respostas pela data selecionada
const respostasFiltradas = computed(() => {
  if (!dataFiltro.value) return respostas.value;
  
  const filtroDate = new Date(dataFiltro.value);
  // Resetando timezone para comparação local (considerando yyyy-mm-dd)
  const filterDateString = filtroDate.toISOString().split('T')[0];

  return respostas.value.filter((r) => {
    if (!r.created_at) return false;
    const respDateString = new Date(r.created_at).toISOString().split('T')[0];
    return respDateString === filterDateString;
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
    fetchRespostas()
  ]);
  isLoading.value = false;
};

onMounted(() => {
  fetchData();
});
</script>
