import axios from "axios";
import {
  defineNuxtPlugin,
  useRuntimeConfig,
  useCookie,
  navigateTo,
} from "#app";

export default defineNuxtPlugin((nuxtApp) => {
  const config = useRuntimeConfig();
  const { showSnackbar } = useSnackbar();
  const tokenCookie = useCookie("access_token");
  const userCookie = useCookie("user_data");

  const api = axios.create({
    baseURL: config.public.apiBaseUrl as string,
  });

  // Intercepta requisições para injetar o token
  api.interceptors.request.use((requestConfig) => {
    if (tokenCookie.value && requestConfig.headers) {
      requestConfig.headers.Authorization = `Bearer ${tokenCookie.value}`;
    }
    return requestConfig;
  });

  // Intercepta respostas globais para checar expiração de sessão
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      // Ignora o interceptor global se a requisição original for a de login
      // pois a tela de login já tem seu próprio tratamento para senhas erradas (401/403)
      if (error.config?.url?.includes("/auth/login")) {
        return Promise.reject(error);
      }

      // Se a API retornar erro de permissão ou token expirado para outras rotas
      if (
        error.response &&
        (error.response.status === 401 || error.response.status === 403)
      ) {
        // Limpa a sessão
        tokenCookie.value = null;
        userCookie.value = null;

        // Exibe mensagem
        showSnackbar({
          message: "Sessão expirada ou sem permissão. Faça login novamente.",
          color: "warning",
        });

        if (import.meta.client) navigateTo("/login");
      }
      return Promise.reject(error);
    },
  );

  return {
    provide: {
      api,
    },
  };
});
