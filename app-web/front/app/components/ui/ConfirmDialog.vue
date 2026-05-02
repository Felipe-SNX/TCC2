<template>
  <v-dialog :model-value="modelValue" max-width="450" persistent @update:model-value="$emit('update:modelValue', $event)">
    <v-card class="rounded-lg">
      <v-card-title class="d-flex align-center pa-4">
        <v-icon class="mr-2" color="warning">mdi-alert-circle-outline</v-icon>
        <span class="text-h6 font-weight-bold">{{ title }}</span>
      </v-card-title>

      <v-divider></v-divider>

      <v-card-text class="pa-6 text-body-1">
        {{ message }}
      </v-card-text>

      <v-divider></v-divider>

      <v-card-actions class="pa-4">
        <v-spacer></v-spacer>
        <v-btn
          variant="text"
          @click="$emit('cancel')"
        >
          {{ cancelText }}
        </v-btn>
        <v-btn
          :color="confirmColor"
          variant="elevated"
          :prepend-icon="confirmIcon"
          @click="$emit('confirm')"
        >
          {{ confirmText }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
defineProps<{
  modelValue: boolean
  title: string
  message: string
  confirmText: string
  cancelText: string
  confirmColor: string
  confirmIcon: string
}>()

defineEmits<{
  (e: 'update:modelValue', value: boolean): void
  (e: 'confirm'): void
  (e: 'cancel'): void
}>()
</script>
