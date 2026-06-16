import { createVuetify, type ThemeDefinition } from 'vuetify'
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

const lightTheme: ThemeDefinition = {
  dark: false,
  colors: {
    primary: '#673AB7',
    secondary: '#00BFA5',
    accent: '#FFD600',
    background: '#F4F7F6',
    surface: '#FFFFFF',
    error: '#FF5252',
    info: '#2196F3',
    success: '#4CAF50',
    warning: '#FB8C00',
    'on-primary': '#FFFFFF',
    'on-secondary': '#FFFFFF',
    'on-background': '#2C3E50',
    'on-surface': '#2C3E50',
  },
}

const darkTheme: ThemeDefinition = {
  dark: true,
  colors: {
    primary: '#9575CD',
    secondary: '#1DE9B6',
    accent: '#FFFF00',
    background: '#121212',
    surface: '#1E1E1E',
    error: '#FF8A80',
    info: '#80D8FF',
    success: '#B9F6CA',
    warning: '#FFE57F',
    'on-primary': '#000000',
    'on-secondary': '#000000',
    'on-background': '#ECEFF1',
    'on-surface': '#ECEFF1',
  },
}

export default defineNuxtPlugin((nuxtApp) => {
  const vuetify = createVuetify({
    ssr: true,
    theme: {
      defaultTheme: 'light',
      themes: {
        light: lightTheme,
        dark: darkTheme,
      },
      variations: {
        colors: ['primary', 'secondary'],
        lengths: 3,
      },
    }
  })
  nuxtApp.vueApp.use(vuetify)
})
