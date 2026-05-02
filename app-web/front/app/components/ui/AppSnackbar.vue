<template>
  <v-snackbar
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    :color="color"
    :timeout="timeout"
    location="bottom right"
  >
    <div class="d-flex align-center">
      <v-icon class="mr-2">{{ icon }}</v-icon>
      {{ message }}
    </div>

    <template v-slot:actions>
      <v-btn
        variant="text"
        icon="mdi-close"
        @click="$emit('update:modelValue', false)"
      ></v-btn>
    </template>
  </v-snackbar>
</template>

<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{
  modelValue: boolean
  message: string
  color: string
  timeout: number
}>()

defineEmits<{
  (e: 'update:modelValue', value: boolean): void
}>()

const icon = computed(() => {
  switch (props.color) {
    case 'success': return 'mdi-check-circle'
    case 'error': return 'mdi-alert-circle'
    case 'warning': return 'mdi-alert'
    case 'info': return 'mdi-information'
    default: return 'mdi-information'
  }
})
</script>
