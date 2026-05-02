export const emailRules = [
  (v: string) => !!v || 'O e-mail é obrigatório.',
  (v: string) => /.+@.+\..+/.test(v) || 'Insira um e-mail válido.',
]

export const passwordRules = [
  (v: string) => !!v || 'A senha é obrigatória.',
  (v: string) => v.length >= 3 || 'A senha deve ter no mínimo 3 caracteres.',
]
