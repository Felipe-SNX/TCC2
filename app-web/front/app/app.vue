<template>
  <v-app>
    <NuxtLayout>
      <NuxtPage />
    </NuxtLayout>

    <UiAppSnackbar
      v-model="isVisible"
      :message="message"
      :color="color"
      :timeout="timeout"
    />
  </v-app>
</template>

<script setup lang="ts">
const { isVisible, message, color, timeout } = useSnackbar()
</script>
