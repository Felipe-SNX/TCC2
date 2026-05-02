import { useNuxtApp } from '#app'

export interface Alternativa {
  texto: string
  valor: number
}

export interface PerguntaForm {
  pergunta: string
  alternativas: Alternativa[]
}

export const perguntasService = {
  async listar(page: number, itemsPerPage: number) {
    const { $api } = useNuxtApp() as any
    const response = await $api.get('/perguntas/', {
      params: {
        page,
        items_per_page: itemsPerPage
      }
    })
    return response.data
  },

  async criar(pergunta: PerguntaForm) {
    const { $api } = useNuxtApp() as any
    const response = await $api.post('/perguntas/', pergunta)
    return response.data
  },

  async atualizar(id: string, pergunta: Partial<PerguntaForm>) {
    const { $api } = useNuxtApp() as any
    const response = await $api.put(`/perguntas/${id}`, pergunta)
    return response.data
  },

  async excluir(id: string) {
    const { $api } = useNuxtApp() as any
    await $api.delete(`/perguntas/${id}`)
  }
}
