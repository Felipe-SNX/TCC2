import { useNuxtApp } from "#app";

export interface UsuarioForm {
  nome: string;
  email: string;
  role: "PSICOLOGO" | "ADMIN";
  senha?: string;
}

export const usuariosService = {
  async listar(page: number, itemsPerPage: number) {
    const { $api } = useNuxtApp() as any;
    const response = await $api.get("/usuarios/", {
      params: {
        page,
        items_per_page: itemsPerPage,
      },
    });
    return response.data;
  },

  async criar(usuario: UsuarioForm) {
    const { $api } = useNuxtApp() as any;
    const response = await $api.post("/usuarios/", usuario);
    return response.data;
  },

  async atualizar(id: string, usuario: Partial<UsuarioForm>) {
    const { $api } = useNuxtApp() as any;
    const response = await $api.put(`/usuarios/${id}`, usuario);
    return response.data;
  },

  async excluir(id: string) {
    const { $api } = useNuxtApp() as any;
    await $api.delete(`/usuarios/${id}`);
  },

  async toggleAtivo(id: string) {
    const { $api } = useNuxtApp() as any;
    const response = await $api.patch(`/usuarios/${id}/ativo`);
    return response.data;
  },
};
