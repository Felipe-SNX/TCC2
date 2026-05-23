<template>
  <div class="dashboard-layout">
    <LayoutAppHeader />
    <LayoutAppSidebar />
    <v-main style="min-height: 100vh;">
      <v-container fluid class="pa-6">
        <slot />
      </v-container>
    </v-main>
  </div>
</template>

<script setup lang="ts">
import LayoutAppHeader from '~/components/layout/AppHeader.vue'
import LayoutAppSidebar from '~/components/layout/AppSidebar.vue'
</script>
