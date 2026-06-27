<template>
  <v-container class="py-6">
    <PacientesTable
      :items="pacientes"
      :total-items="total"
      :loading="isLoading"
      @update:options="fetchPacientes"
      @update:search="handleSearchUpdate"
      @create="openCreateDialog"
      @edit="openEditDialog"
      @delete="handleDelete"
      @view-dashboard="handleViewDashboard"
      @refresh-pin="handleRefreshPin"
    />

    <PacientesFormDialog
      v-model="dialogOpen"
      :paciente="selectedPaciente"
      :saving="isSaving"
      @save="handleSave"
      @cancel="closeDialog"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useConfirmDialog } from "~/composables/useConfirmDialog";
import {
  pacientesService,
  type PacienteForm,
} from "~/services/pacientes.service";

definePageMeta({
  layout: "dashboard",
  title: "Painel de Pacientes",
  description: "Gerencie seus pacientes e acompanhe o histórico de respostas.",
});

const { showSnackbar } = useSnackbar();
const { confirm } = useConfirmDialog();

const pacientes = ref([]);
const total = ref(0);
const isLoading = ref(false);
const currentOptions = ref({ page: 1, itemsPerPage: 25 });
const searchQuery = ref("");

const dialogOpen = ref(false);
const selectedPaciente = ref<any | null>(null);
const isSaving = ref(false);

const fetchPacientes = async (options: {
  page: number;
  itemsPerPage: number;
}) => {
  currentOptions.value = options;
  isLoading.value = true;
  try {
    const data = await pacientesService.listar(
      options.page,
      options.itemsPerPage,
      searchQuery.value || undefined,
    );
    pacientes.value = data.items;
    total.value = data.total;
  } catch (error: any) {
    console.error("Erro ao buscar pacientes:", error);
    if (error.response?.status !== 401 && error.response?.status !== 403) {
      showSnackbar({
        message: "Falha ao carregar lista de pacientes.",
        color: "error",
      });
    }
  } finally {
    isLoading.value = false;
  }
};

const handleSearchUpdate = (search: string) => {
  searchQuery.value = search;
  // Reseta para a primeira página ao realizar uma nova busca
  fetchPacientes({ page: 1, itemsPerPage: currentOptions.value.itemsPerPage });
};

const openCreateDialog = () => {
  selectedPaciente.value = null;
  dialogOpen.value = true;
};

const openEditDialog = (paciente: any) => {
  selectedPaciente.value = paciente;
  dialogOpen.value = true;
};

const closeDialog = () => {
  dialogOpen.value = false;
  selectedPaciente.value = null;
};

const handleSave = async (formData: PacienteForm) => {
  isSaving.value = true;
  try {
    if (selectedPaciente.value) {
      await pacientesService.atualizar(selectedPaciente.value.id, formData);
      showSnackbar({
        message: "Paciente atualizado com sucesso.",
        color: "success",
      });
    } else {
      await pacientesService.criar(formData);
      showSnackbar({
        message: "Paciente criado com sucesso.",
        color: "success",
      });
    }
    closeDialog();
    await fetchPacientes(currentOptions.value);
  } catch (error: any) {
    console.error("Erro ao salvar paciente:", error);
    const detail = error.response?.data?.detail;
    showSnackbar({
      message: detail || "Falha ao salvar paciente.",
      color: "error",
    });
  } finally {
    isSaving.value = false;
  }
};

const handleDelete = async (paciente: any) => {
  const decision = await confirm({
    title: "Excluir paciente",
    message: `Tem certeza que deseja excluir o paciente "${paciente.nome}"? Esta ação não pode ser desfeita.`,
    confirmText: "Excluir",
    cancelText: "Cancelar",
    confirmColor: "error",
    confirmIcon: "mdi-delete",
  });

  if (!decision) return;

  try {
    await pacientesService.excluir(paciente.id);
    showSnackbar({
      message: "Paciente excluído com sucesso.",
      color: "success",
    });
    await fetchPacientes(currentOptions.value);
  } catch (error: any) {
    console.error("Erro ao excluir paciente:", error);
    const detail = error.response?.data?.detail;
    showSnackbar({
      message: detail || "Falha ao excluir paciente.",
      color: "error",
    });
  }
};

const handleViewDashboard = (paciente: any) => {
  navigateTo(`/dashboard/${paciente.id}`);
};

const handleRefreshPin = async (paciente: any) => {
  const decision = await confirm({
    title: "Renovar PIN",
    message: `Tem certeza que deseja renovar o PIN do paciente "${paciente.nome}"? O PIN antigo deixará de funcionar imediatamente.`,
    confirmText: "Renovar",
    cancelText: "Cancelar",
    confirmColor: "primary",
    confirmIcon: "mdi-refresh",
  });

  if (!decision) return;

  try {
    await pacientesService.renovarPin(paciente.id);
    showSnackbar({
      message: "PIN renovado com sucesso.",
      color: "success",
    });
    await fetchPacientes(currentOptions.value);
  } catch (error: any) {
    console.error("Erro ao renovar PIN:", error);
    const detail = error.response?.data?.detail;
    showSnackbar({
      message: detail || "Falha ao renovar PIN.",
      color: "error",
    });
  }
};
</script>
