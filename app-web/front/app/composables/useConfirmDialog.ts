export interface ConfirmDialogOptions {
  title?: string
  message: string
  confirmText?: string
  cancelText?: string
  confirmColor?: string
  confirmIcon?: string
}

// Armazena a função resolve da Promise no escopo do módulo.
// Como só existe um ConfirmDialog ativo por vez, isso é seguro.
let resolvePromise: ((value: boolean) => void) | null = null

export const useConfirmDialog = () => {
  const isOpen = useState<boolean>('confirm-dialog-open', () => false)
  const title = useState<string>('confirm-dialog-title', () => 'Confirmação')
  const message = useState<string>('confirm-dialog-message', () => '')
  const confirmText = useState<string>('confirm-dialog-confirm-text', () => 'Confirmar')
  const cancelText = useState<string>('confirm-dialog-cancel-text', () => 'Cancelar')
  const confirmColor = useState<string>('confirm-dialog-confirm-color', () => 'error')
  const confirmIcon = useState<string>('confirm-dialog-confirm-icon', () => 'mdi-check')

  /**
   * Abre o dialog de confirmação e retorna uma Promise que resolve
   * com `true` se o usuário confirmar, ou `false` se cancelar.
   *
   * Uso:
   * ```ts
   * const decision = await confirm({ message: 'Tem certeza?' })
   * if (!decision) return
   * ```
   */
  const confirm = (options: ConfirmDialogOptions): Promise<boolean> => {
    title.value = options.title || 'Confirmação'
    message.value = options.message
    confirmText.value = options.confirmText || 'Confirmar'
    cancelText.value = options.cancelText || 'Cancelar'
    confirmColor.value = options.confirmColor || 'error'
    confirmIcon.value = options.confirmIcon || 'mdi-check'
    isOpen.value = true

    return new Promise<boolean>((resolve) => {
      resolvePromise = resolve
    })
  }

  const onConfirm = () => {
    isOpen.value = false
    resolvePromise?.(true)
    resolvePromise = null
  }

  const onCancel = () => {
    isOpen.value = false
    resolvePromise?.(false)
    resolvePromise = null
  }

  return {
    isOpen,
    title,
    message,
    confirmText,
    cancelText,
    confirmColor,
    confirmIcon,
    confirm,
    onConfirm,
    onCancel
  }
}
