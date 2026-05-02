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

    <UiConfirmDialog
      v-model="confirmIsOpen"
      :title="confirmTitle"
      :message="confirmMessage"
      :confirm-text="confirmConfirmText"
      :cancel-text="confirmCancelText"
      :confirm-color="confirmConfirmColor"
      :confirm-icon="confirmConfirmIcon"
      @confirm="onConfirm"
      @cancel="onCancel"
    />
  </v-app>
</template>

<script setup lang="ts">
import { useSnackbar } from '~/composables/useSnackbar'
import { useConfirmDialog } from '~/composables/useConfirmDialog'
import UiConfirmDialog from '~/components/ui/ConfirmDialog.vue'

const { isVisible, message, color, timeout } = useSnackbar()

const {
  isOpen: confirmIsOpen,
  title: confirmTitle,
  message: confirmMessage,
  confirmText: confirmConfirmText,
  cancelText: confirmCancelText,
  confirmColor: confirmConfirmColor,
  confirmIcon: confirmConfirmIcon,
  onConfirm,
  onCancel
} = useConfirmDialog()
</script>
