import { useNuxtApp } from '#app'

export interface PerfilForm {
  nome: string
  email: string
  senha?: string
}

export interface PerfilResponse {
  id: string
  nome: string
  email: string
  role: string
}

export const perfilService = {
  async obterMeusDados(): Promise<PerfilResponse> {
    const { $api } = useNuxtApp() as any
    const response = await $api.get('/usuarios/me')
    return response.data
  },

  async atualizarMeusDados(dados: Partial<PerfilForm>): Promise<PerfilResponse> {
    const { $api } = useNuxtApp() as any
    const response = await $api.put('/usuarios/me', dados)
    return response.data
  }
}
