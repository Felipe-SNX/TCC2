<template>
  <v-container class="py-6">
    <div class="mb-6">
      <h1 class="text-h4 font-weight-bold">Painel de Pacientes</h1>
      <p class="text-medium-emphasis">Gerencie seus pacientes e acompanhe o histórico de respostas.</p>
    </div>

    <!-- 
      Componente da tabela, sendo puramente "dummy". 
      A lógica é toda controlada pelo Container Pai 
    -->
    <PacientesTable
      :items="pacientes"
      :total-items="total"
      :loading="isLoading"
      @update:options="fetchPacientes"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { pacientesService } from '~/services/pacientes.service'

const { showSnackbar } = useSnackbar()

const pacientes = ref([])
const total = ref(0)
const isLoading = ref(false)

const fetchPacientes = async (options: { page: number, itemsPerPage: number }) => {
  isLoading.value = true
  try {
    const data = await pacientesService.listar(options.page, options.itemsPerPage)

    pacientes.value = data.items
    total.value = data.total
  } catch (error: any) {
    console.error('Erro ao buscar pacientes:', error)
    if (error.response?.status === 401 || error.response?.status === 403) {
      showSnackbar({ message: 'Sessão expirada ou sem permissão. Faça login novamente.', color: 'error' })
      navigateTo('/login')
    } else {
      showSnackbar({ message: 'Falha ao carregar lista de pacientes.', color: 'error' })
    }
  } finally {
    isLoading.value = false
  }
}
</script>
