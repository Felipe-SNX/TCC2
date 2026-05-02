import { useNuxtApp } from '#app'

export const usuariosService = {
  async listar(page: number, itemsPerPage: number) {
    const { $api } = useNuxtApp() as any
    const response = await $api.get('/usuarios/', {
      params: {
        page,
        items_per_page: itemsPerPage
      }
    })
    return response.data
  }
}
