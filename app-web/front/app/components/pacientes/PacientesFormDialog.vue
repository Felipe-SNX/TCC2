<template>
  <v-dialog :model-value="modelValue" max-width="600" persistent @update:model-value="$emit('update:modelValue', $event)">
    <v-card class="rounded-lg">
      <v-card-title class="d-flex align-center pa-4 bg-surface-light">
        <v-icon class="mr-2" color="primary">
          {{ isEditing ? 'mdi-account-edit' : 'mdi-account-plus' }}
        </v-icon>
        <span class="text-h6 font-weight-bold">
          {{ isEditing ? 'Editar Paciente' : 'Novo Paciente' }}
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
            color="primary"
            class="mb-2"
          ></v-text-field>

          <v-text-field
            v-model="form.email"
            label="E-mail"
            type="email"
            prepend-inner-icon="mdi-email"
            :rules="[rules.required, rules.email]"
            variant="outlined"
            color="primary"
            class="mb-2"
          ></v-text-field>

          <v-text-field
            v-model.number="form.idade"
            label="Idade"
            type="number"
            prepend-inner-icon="mdi-calendar"
            :rules="[rules.required, rules.idade]"
            variant="outlined"
            color="primary"
            class="mb-2"
          ></v-text-field>

          <v-textarea
            v-model="form.observacoes"
            label="Observações (opcional)"
            prepend-inner-icon="mdi-note-text"
            variant="outlined"
            color="primary"
            rows="3"
            auto-grow
            class="mb-2"
          ></v-textarea>
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

interface PacienteFormData {
  nome: string
  email: string
  idade: number | null
  observacoes: string
}

import type { PacienteForm } from '~/services/pacientes.service'

const props = defineProps<{
  modelValue: boolean
  paciente: any | null
  saving: boolean
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: boolean): void
  (e: 'cancel'): void
  (e: 'save', data: PacienteForm): void
}>()

const formRef = ref()

const isEditing = computed(() => !!props.paciente)

const defaultForm = (): PacienteFormData => ({
  nome: '',
  email: '',
  idade: null,
  observacoes: ''
})

const form = ref<PacienteFormData>(defaultForm())

const rules = {
  required: (v: any) => !!v || v === 0 || 'Campo obrigatório',
  email: (v: string) => /.+@.+\..+/.test(v) || 'E-mail inválido',
  idade: (v: number) => (v !== null && v > 0 && v <= 150) || 'Idade inválida'
}

// Observa mudanças no dialog e no paciente para preencher/limpar o form
watch(() => props.modelValue, (open) => {
  if (open && props.paciente) {
    form.value = {
      nome: props.paciente.nome,
      email: props.paciente.email,
      idade: props.paciente.idade,
      observacoes: props.paciente.observacoes || ''
    }
  } else if (open) {
    form.value = defaultForm()
  }
})

const handleSubmit = async () => {
  const { valid } = await formRef.value?.validate()
  if (!valid) return
  
  const payload: PacienteForm = {
    nome: form.value.nome,
    email: form.value.email,
    idade: form.value.idade as number,
    observacoes: form.value.observacoes
  }
  
  emit('save', payload)
}
</script>
