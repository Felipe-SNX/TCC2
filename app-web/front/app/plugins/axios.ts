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

  api.interceptors.request.use((requestConfig) => {
    if (tokenCookie.value && requestConfig.headers) {
      requestConfig.headers.Authorization = `Bearer ${tokenCookie.value}`;
    }
    return requestConfig;
  });

  api.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.config?.url?.includes("/auth/login")) {
        return Promise.reject(error);
      }

      if (
        error.response &&
        (error.response.status === 401 || error.response.status === 403)
      ) {
        tokenCookie.value = null;
        userCookie.value = null;

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
