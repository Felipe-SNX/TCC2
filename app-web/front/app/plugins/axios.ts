import axios from 'axios'
import { defineNuxtPlugin, useRuntimeConfig, useCookie } from '#app'

export default defineNuxtPlugin((nuxtApp) => {
  const config = useRuntimeConfig()
  
  const api = axios.create({
    baseURL: config.public.apiBaseUrl as string,
  })

  // Intercepta todas as requisições para injetar o token de forma global
  api.interceptors.request.use((requestConfig) => {
    const token = useCookie('access_token').value
    if (token && requestConfig.headers) {
      requestConfig.headers.Authorization = `Bearer ${token}`
    }
    return requestConfig
  })

  return {
    provide: {
      api
    }
  }
})
