export interface SnackbarOptions {
  message: string;
  color?: 'success' | 'error' | 'warning' | 'info' | string;
  timeout?: number;
}

export const useSnackbar = () => {
  const isVisible = useState<boolean>('snackbar-visible', () => false)
  const message = useState<string>('snackbar-message', () => '')
  const color = useState<string>('snackbar-color', () => 'success')
  const timeout = useState<number>('snackbar-timeout', () => 3000)

  const showSnackbar = (options: SnackbarOptions) => {
    message.value = options.message
    color.value = options.color || 'success'
    timeout.value = options.timeout || 3000
    isVisible.value = true
  }

  return {
    isVisible,
    message,
    color,
    timeout,
    showSnackbar
  }
}
