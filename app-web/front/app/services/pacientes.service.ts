import { useNuxtApp } from '#app'

export interface PacienteForm {
  nome: string
  idade: number
  email: string
  observacoes?: string
}

export const pacientesService = {
  async listar(page: number, itemsPerPage: number) {
    const { $api } = useNuxtApp() as any
    const response = await $api.get('/pacientes/', {
      params: {
        page,
        items_per_page: itemsPerPage
      }
    })
    return response.data
  },

  async criar(paciente: PacienteForm) {
    const { $api } = useNuxtApp() as any
    const response = await $api.post('/pacientes/', paciente)
    return response.data
  },

  async atualizar(id: string, paciente: Partial<PacienteForm>) {
    const { $api } = useNuxtApp() as any
    const response = await $api.put(`/pacientes/${id}`, paciente)
    return response.data
  },

  async excluir(id: string) {
    const { $api } = useNuxtApp() as any
    await $api.delete(`/pacientes/${id}`)
  }
}
