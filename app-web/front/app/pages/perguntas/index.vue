<template>
  <v-container class="py-6">
    <!-- 
      Componente da tabela, sendo puramente "dummy". 
      A lógica é toda controlada pelo Container Pai 
    -->
    <PerguntasTable
      :items="perguntas"
      :total-items="total"
      :loading="isLoading"
      @update:options="fetchPerguntas"
      @create="openCreateDialog"
      @edit="openEditDialog"
      @delete="handleDelete"
    />

    <!-- 
      Modal de criação/edição, também puramente "dummy".
      Apenas exibe o formulário e emite os dados preenchidos via 'save'.
    -->
    <PerguntasFormDialog
      v-model="dialogOpen"
      :pergunta="selectedPergunta"
      :saving="isSaving"
      @save="handleSave"
      @cancel="closeDialog"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { perguntasService, type PerguntaForm } from '~/services/perguntas.service'

definePageMeta({
  layout: 'dashboard',
  title: 'Painel de Perguntas',
  description: 'Gerencie as perguntas e alternativas aplicadas aos pacientes.'
});

const { showSnackbar } = useSnackbar()
const { confirm } = useConfirmDialog()

// Estado da tabela
const perguntas = ref([])
const total = ref(0)
const isLoading = ref(false)
const currentOptions = ref({ page: 1, itemsPerPage: 25 })

// Estado do modal
const dialogOpen = ref(false)
const selectedPergunta = ref<any | null>(null)
const isSaving = ref(false)

const fetchPerguntas = async (options: { page: number, itemsPerPage: number }) => {
  currentOptions.value = options
  isLoading.value = true
  try {
    const data = await perguntasService.listar(options.page, options.itemsPerPage)
    perguntas.value = data.items
    total.value = data.total
  } catch (error: any) {
    console.error('Erro ao buscar perguntas:', error)
    if (error.response?.status !== 401 && error.response?.status !== 403) {
      showSnackbar({ message: 'Falha ao carregar lista de perguntas.', color: 'error' })
    }
  } finally {
    isLoading.value = false
  }
}

const openCreateDialog = () => {
  selectedPergunta.value = null
  dialogOpen.value = true
}

const openEditDialog = (pergunta: any) => {
  selectedPergunta.value = pergunta
  dialogOpen.value = true
}

const closeDialog = () => {
  dialogOpen.value = false
  selectedPergunta.value = null
}

const handleSave = async (formData: PerguntaForm) => {
  isSaving.value = true
  try {
    if (selectedPergunta.value) {
      await perguntasService.atualizar(selectedPergunta.value.id, formData)
      showSnackbar({ message: 'Pergunta atualizada com sucesso.', color: 'success' })
    } else {
      await perguntasService.criar(formData)
      showSnackbar({ message: 'Pergunta criada com sucesso.', color: 'success' })
    }
    closeDialog()
    await fetchPerguntas(currentOptions.value)
  } catch (error: any) {
    console.error('Erro ao salvar pergunta:', error)
    const detail = error.response?.data?.detail
    showSnackbar({ message: detail || 'Falha ao salvar pergunta.', color: 'error' })
  } finally {
    isSaving.value = false
  }
}

const handleDelete = async (pergunta: any) => {
  const decision = await confirm({
    title: 'Excluir pergunta',
    message: `Tem certeza que deseja excluir a pergunta "${pergunta.pergunta}"? Esta ação não pode ser desfeita.`,
    confirmText: 'Excluir',
    cancelText: 'Cancelar',
    confirmColor: 'error',
    confirmIcon: 'mdi-delete'
  })

  if (!decision) return

  try {
    await perguntasService.excluir(pergunta.id)
    showSnackbar({ message: 'Pergunta excluída com sucesso.', color: 'success' })
    await fetchPerguntas(currentOptions.value)
  } catch (error: any) {
    console.error('Erro ao excluir pergunta:', error)
    const detail = error.response?.data?.detail
    showSnackbar({ message: detail || 'Falha ao excluir pergunta.', color: 'error' })
  }
}
</script>
