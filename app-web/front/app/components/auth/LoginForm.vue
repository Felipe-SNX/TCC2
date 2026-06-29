<template>
  <v-form @submit.prevent="onSubmit" ref="form">
    <div class="d-flex flex-column ga-1 pa-6 elevation-2 rounded bg-surface">
      <img
        src="/logo-v3.png"
        alt="Logo"
        class="mx-auto"
        width="120"
        height="auto"
      />

      <img
        src="/espectro-label.png"
        alt="Logo"
        class="mx-auto mb-6"
        width="240"
        height="auto"
      />

      <v-text-field
        v-model="email"
        label="E-mail"
        type="email"
        prepend-inner-icon="mdi-email"
        :rules="emailRules"
        @keyup.enter="onSubmit"
        variant="outlined"
        color="primary"
        required
      ></v-text-field>

      <v-text-field
        v-model="password"
        label="Senha"
        type="password"
        prepend-inner-icon="mdi-lock"
        :rules="passwordRules"
        @keyup.enter="onSubmit"
        variant="outlined"
        color="primary"
        required
      ></v-text-field>

      <v-btn
        type="submit"
        color="primary"
        class="mt-2"
        block
        size="large"
        :loading="loading"
      >
        Entrar
      </v-btn>

      <div class="d-flex justify-space-between mt-2">
        <v-spacer />
        <v-btn
          variant="text"
          size="small"
          color="secondary"
          @click="$emit('register')"
        >
          Criar conta
        </v-btn>
      </div>
    </div>
  </v-form>
</template>

<script setup lang="ts">
import { ref } from "vue";

const props = defineProps<{
  loading?: boolean;
}>();

const emit = defineEmits<{
  (e: "submit", payload: { email: string; password: string }): void;
  (e: "register"): void;
}>();

const form = ref<any>(null);
const email = ref("");
const password = ref("");

const onSubmit = async () => {
  if (!form.value) return;

  const { valid } = await form.value.validate();

  if (valid) {
    emit("submit", { email: email.value, password: password.value });
  }
};
</script>
