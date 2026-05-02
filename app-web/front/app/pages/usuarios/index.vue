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
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { usuariosService } from '~/services/usuarios.service'

const { showSnackbar } = useSnackbar()

const usuarios = ref([])
const total = ref(0)
const isLoading = ref(false)

const fetchUsuarios = async (options: { page: number, itemsPerPage: number }) => {
  isLoading.value = true
  try {
    const data = await usuariosService.listar(options.page, options.itemsPerPage)

    usuarios.value = data.items
    total.value = data.total
  } catch (error: any) {
    console.error('Erro ao buscar usuários:', error)
    // Erros 401 e 403 já são interceptados globalmente pelo axios plugin
    if (error.response?.status !== 401 && error.response?.status !== 403) {
      showSnackbar({ message: 'Falha ao carregar lista de usuários.', color: 'error' })
    }
  } finally {
    isLoading.value = false
  }
}
</script>
