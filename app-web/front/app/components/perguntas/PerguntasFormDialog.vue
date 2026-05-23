<template>
  <v-dialog :model-value="modelValue" max-width="700" persistent @update:model-value="$emit('update:modelValue', $event)">
    <v-card class="rounded-lg">
      <v-card-title class="d-flex align-center pa-4 bg-surface-light">
        <v-icon class="mr-2" color="primary">
          {{ isEditing ? 'mdi-pencil-circle' : 'mdi-help-circle-plus' }}
        </v-icon>
        <span class="text-h6 font-weight-bold">
          {{ isEditing ? 'Editar Pergunta' : 'Nova Pergunta' }}
        </span>
      </v-card-title>

      <v-divider></v-divider>

      <v-card-text class="pa-6">
        <v-form ref="formRef" @submit.prevent="handleSubmit">
          <v-textarea
            v-model="form.pergunta"
            label="Texto da pergunta"
            prepend-inner-icon="mdi-chat-question"
            :rules="[rules.required]"
            variant="outlined"
            color="primary"
            rows="3"
            auto-grow
            class="mb-4"
          ></v-textarea>

          <div class="d-flex align-center mb-3">
            <v-icon class="mr-2" color="primary" size="small">mdi-format-list-numbered</v-icon>
            <span class="text-subtitle-1 font-weight-medium">Alternativas</span>
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
              variant="tonal"
              size="small"
              prepend-icon="mdi-plus"
              @click="addAlternativa"
              :disabled="form.alternativas.length >= 10"
            >
              Adicionar
            </v-btn>
          </div>

          <v-card
            v-for="(alt, index) in form.alternativas"
            :key="index"
            variant="outlined"
            class="mb-2 pa-3"
          >
            <div class="d-flex align-center ga-3">
              <v-chip :color="chipColor(alt.valor)" size="small" variant="tonal" class="flex-shrink-0">
                {{ alt.valor }}
              </v-chip>
              <v-text-field
                v-model="alt.texto"
                :label="`Alternativa ${index + 1}`"
                :rules="[rules.required]"
                variant="outlined"
                color="primary"
                density="compact"
                hide-details="auto"
              ></v-text-field>
              <v-text-field
                v-model.number="alt.valor"
                label="Valor"
                type="number"
                :rules="[rules.required, rules.positiveNumber]"
                variant="outlined"
                color="primary"
                density="compact"
                hide-details="auto"
                style="max-width: 100px"
              ></v-text-field>
              <v-btn
                icon="mdi-close-circle"
                variant="text"
                size="small"
                color="error"
                @click="removeAlternativa(index)"
                :disabled="form.alternativas.length <= 2"
              ></v-btn>
            </div>
          </v-card>

          <v-alert
            v-if="form.alternativas.length < 2"
            type="warning"
            variant="tonal"
            density="compact"
            class="mt-2"
          >
            É necessário ter ao menos 2 alternativas.
          </v-alert>
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
          :disabled="form.alternativas.length < 2"
        >
          {{ isEditing ? 'Salvar' : 'Criar' }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'

interface Alternativa {
  texto: string
  valor: number
}

interface PerguntaFormData {
  pergunta: string
  alternativas: Alternativa[]
}

const props = defineProps<{
  modelValue: boolean
  pergunta: any | null
  saving: boolean
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: boolean): void
  (e: 'cancel'): void
  (e: 'save', data: PerguntaFormData): void
}>()

const formRef = ref()

const isEditing = computed(() => !!props.pergunta)

const defaultAlternativas = (): Alternativa[] => [
  { texto: 'Muito negativo', valor: 1 },
  { texto: 'Negativo', valor: 2 },
  { texto: 'Neutro', valor: 3 },
  { texto: 'Positivo', valor: 4 },
  { texto: 'Muito positivo', valor: 5 }
]

const defaultForm = (): PerguntaFormData => ({
  pergunta: '',
  alternativas: defaultAlternativas()
})

const form = ref<PerguntaFormData>(defaultForm())

const rules = {
  required: (v: any) => (v !== null && v !== undefined && v !== '') || 'Campo obrigatório',
  positiveNumber: (v: number) => (v > 0) || 'Deve ser positivo'
}

const addAlternativa = () => {
  const nextValue = form.value.alternativas.length + 1
  form.value.alternativas.push({ texto: '', valor: nextValue })
}

const removeAlternativa = (index: number) => {
  form.value.alternativas.splice(index, 1)
}

const chipColor = (valor: number): string => {
  const colors: Record<number, string> = {
    1: 'error',
    2: 'warning',
    3: 'grey',
    4: 'success',
    5: 'info'
  }
  return colors[valor] || 'primary'
}

// Observa mudanças no dialog e na pergunta para preencher/limpar o form
watch(() => props.modelValue, (open) => {
  if (open && props.pergunta) {
    form.value = {
      pergunta: props.pergunta.pergunta,
      alternativas: Array.isArray(props.pergunta.alternativas)
        ? props.pergunta.alternativas.map((a: any) => ({ texto: a.texto, valor: a.valor }))
        : defaultAlternativas()
    }
  } else if (open) {
    form.value = defaultForm()
  }
})

const handleSubmit = async () => {
  const { valid } = await formRef.value?.validate()
  if (!valid) return
  if (form.value.alternativas.length < 2) return
  emit('save', { ...form.value })
}
</script>
