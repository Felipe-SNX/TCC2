<template>
  <v-dialog :model-value="modelValue" max-width="600" persistent @update:model-value="$emit('update:modelValue', $event)">
    <v-card class="rounded-lg">
      <v-card-title class="d-flex align-center pa-4 bg-surface-light">
        <v-icon class="mr-2" color="primary">
          {{ isEditing ? 'mdi-account-edit' : 'mdi-account-plus' }}
        </v-icon>
        <span class="text-h6 font-weight-bold">
          {{ isEditing ? 'Editar Usuário' : 'Novo Usuário' }}
        </span>
      </v-card-title>

      <v-divider></v-divider>

      <v-card-text class="pa-6">
        <v-form ref="formRef" @submit.prevent="handleSubmit">
          <v-text-field
            v-model="form.nome"
            label="Nome completo"
            prepend-inner-icon="mdi-account"
            :rules="[rules.required]"
            variant="outlined"
            class="mb-2"
          ></v-text-field>

          <v-text-field
            v-model="form.email"
            label="E-mail"
            type="email"
            prepend-inner-icon="mdi-email"
            :rules="[rules.required, rules.email]"
            variant="outlined"
            class="mb-2"
          ></v-text-field>

          <v-select
            v-model="form.role"
            label="Perfil de acesso"
            prepend-inner-icon="mdi-shield-account"
            :items="roleOptions"
            item-title="label"
            item-value="value"
            :rules="[rules.required]"
            variant="outlined"
            class="mb-2"
          ></v-select>

          <v-text-field
            v-model="form.senha"
            :label="isEditing ? 'Nova senha (deixe em branco para manter)' : 'Senha'"
            type="password"
            prepend-inner-icon="mdi-lock"
            :rules="isEditing ? [] : [rules.required, rules.minLength]"
            variant="outlined"
            class="mb-2"
          ></v-text-field>
        </v-form>
      </v-card-text>

      <v-divider></v-divider>

      <v-card-actions class="pa-4">
        <v-spacer></v-spacer>
        <v-btn
          variant="text"
          @click="$emit('cancel')"
          :disabled="saving"
        >
          Cancelar
        </v-btn>
        <v-btn
          color="primary"
          variant="elevated"
          :loading="saving"
          :prepend-icon="isEditing ? 'mdi-content-save' : 'mdi-plus'"
          @click="handleSubmit"
        >
          {{ isEditing ? 'Salvar' : 'Criar' }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'

interface UsuarioFormData {
  nome: string
  email: string
  role: 'PSICOLOGO' | 'ADMIN'
  senha: string
}

const props = defineProps<{
  modelValue: boolean
  usuario: any | null
  saving: boolean
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: boolean): void
  (e: 'cancel'): void
  (e: 'save', data: UsuarioFormData): void
}>()

const formRef = ref()

const isEditing = computed(() => !!props.usuario)

const roleOptions = [
  { label: 'Psicólogo', value: 'PSICOLOGO' },
  { label: 'Administrador', value: 'ADMIN' }
]

const defaultForm = (): UsuarioFormData => ({
  nome: '',
  email: '',
  role: 'PSICOLOGO',
  senha: ''
})

const form = ref<UsuarioFormData>(defaultForm())

const rules = {
  required: (v: string) => !!v || 'Campo obrigatório',
  email: (v: string) => /.+@.+\..+/.test(v) || 'E-mail inválido',
  minLength: (v: string) => v.length >= 6 || 'Mínimo de 6 caracteres'
}

// Observa mudanças no dialog e no usuario para preencher/limpar o form
watch(() => props.modelValue, (open) => {
  if (open && props.usuario) {
    form.value = {
      nome: props.usuario.nome,
      email: props.usuario.email,
      role: props.usuario.role,
      senha: ''
    }
  } else if (open) {
    form.value = defaultForm()
  }
})

const handleSubmit = async () => {
  const { valid } = await formRef.value?.validate()
  if (!valid) return
  emit('save', { ...form.value })
}
</script>
