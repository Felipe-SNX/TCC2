<template>
  <v-container class="py-6">
    <div class="mb-6">
      <h1 class="text-h4 font-weight-bold">Painel de Usuários</h1>
      <p class="text-medium-emphasis">Gerencie os usuários do sistema e seus níveis de acesso.</p>
    </div>

    <!-- 
      Componente da tabela, sendo puramente "dummy". 
      A lógica é toda controlada pelo Container Pai 
    -->
    <UsuariosTable
      :items="usuarios"
      :total-items="total"
      :loading="isLoading"
      @update:options="fetchUsuarios"
      @create="openCreateDialog"
      @edit="openEditDialog"
    />

    <!-- 
      Modal de criação/edição, também puramente "dummy".
      Apenas exibe o formulário e emite os dados preenchidos via 'save'.
    -->
    <UsuariosFormDialog
      v-model="dialogOpen"
      :usuario="selectedUsuario"
      :saving="isSaving"
      @save="handleSave"
      @cancel="closeDialog"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { usuariosService, type UsuarioForm } from '~/services/usuarios.service'

const { showSnackbar } = useSnackbar()

// Estado da tabela
const usuarios = ref([])
const total = ref(0)
const isLoading = ref(false)
const currentOptions = ref({ page: 1, itemsPerPage: 25 })

// Estado do modal
const dialogOpen = ref(false)
const selectedUsuario = ref<any | null>(null)
const isSaving = ref(false)

const fetchUsuarios = async (options: { page: number, itemsPerPage: number }) => {
  currentOptions.value = options
  isLoading.value = true
  try {
    const data = await usuariosService.listar(options.page, options.itemsPerPage)
    usuarios.value = data.items
    total.value = data.total
  } catch (error: any) {
    console.error('Erro ao buscar usuários:', error)
    if (error.response?.status !== 401 && error.response?.status !== 403) {
      showSnackbar({ message: 'Falha ao carregar lista de usuários.', color: 'error' })
    }
  } finally {
    isLoading.value = false
  }
}

const openCreateDialog = () => {
  selectedUsuario.value = null
  dialogOpen.value = true
}

const openEditDialog = (usuario: any) => {
  selectedUsuario.value = usuario
  dialogOpen.value = true
}

const closeDialog = () => {
  dialogOpen.value = false
  selectedUsuario.value = null
}

const handleSave = async (formData: UsuarioForm) => {
  isSaving.value = true
  try {
    if (selectedUsuario.value) {
      // Edição: remove senha vazia para não enviar ao backend
      const payload: Partial<UsuarioForm> = { ...formData }
      if (!payload.senha) delete payload.senha
      await usuariosService.atualizar(selectedUsuario.value.id, payload)
      showSnackbar({ message: 'Usuário atualizado com sucesso.', color: 'success' })
    } else {
      await usuariosService.criar(formData)
      showSnackbar({ message: 'Usuário criado com sucesso.', color: 'success' })
    }
    closeDialog()
    await fetchUsuarios(currentOptions.value)
  } catch (error: any) {
    console.error('Erro ao salvar usuário:', error)
    const detail = error.response?.data?.detail
    showSnackbar({ message: detail || 'Falha ao salvar usuário.', color: 'error' })
  } finally {
    isSaving.value = false
  }
}
</script>
