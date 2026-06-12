<template>
  <v-row dense>
    <!-- Total de Sessões -->
    <v-col cols="12" sm="6" md="4" lg>
      <v-card
        rounded="lg"
        variant="tonal"
        color="primary"
        class="pa-4"
        elevation="0"
      >
        <div class="d-flex align-center justify-space-between mb-3">
          <span class="text-caption text-medium-emphasis font-weight-medium text-uppercase">
            Total de Sessões
          </span>
          <v-icon color="primary" size="20">mdi-gamepad-variant-outline</v-icon>
        </div>
        <div class="text-h4 font-weight-bold">{{ totalSessoes }}</div>
        <div class="text-caption text-medium-emphasis mt-1">registros no período</div>
      </v-card>
    </v-col>

    <!-- Resposta Emocional Média -->
    <v-col cols="12" sm="6" md="4" lg>
      <v-card
        rounded="lg"
        variant="tonal"
        :color="corEmocional"
        class="pa-4"
        elevation="0"
      >
        <div class="d-flex align-center justify-space-between mb-3">
          <span class="text-caption text-medium-emphasis font-weight-medium text-uppercase">
            Resposta Emocional Média
          </span>
          <v-icon :color="corEmocional" size="20">mdi-emoticon-outline</v-icon>
        </div>
        <div class="d-flex align-center ga-2">
          <span class="text-h4 font-weight-bold">{{ mediaResposta }}</span>
          <span class="text-h5">{{ emojiEmocional }}</span>
        </div>
        <div class="text-caption text-medium-emphasis mt-1">{{ labelEmocional }}</div>
      </v-card>
    </v-col>

    <!-- Tempo Médio por Sessão -->
    <v-col cols="12" sm="6" md="4" lg>
      <v-card
        rounded="lg"
        variant="tonal"
        color="secondary"
        class="pa-4"
        elevation="0"
      >
        <div class="d-flex align-center justify-space-between mb-3">
          <span class="text-caption text-medium-emphasis font-weight-medium text-uppercase">
            Tempo Médio
          </span>
          <v-icon color="secondary" size="20">mdi-timer-outline</v-icon>
        </div>
        <div class="text-h4 font-weight-bold">{{ tempoMedio }}</div>
        <div class="text-caption text-medium-emphasis mt-1">por sessão</div>
      </v-card>
    </v-col>

    <!-- Colecionáveis Totais -->
    <v-col cols="12" sm="6" md="4" lg>
      <v-card
        rounded="lg"
        variant="tonal"
        color="warning"
        class="pa-4"
        elevation="0"
      >
        <div class="d-flex align-center justify-space-between mb-3">
          <span class="text-caption text-medium-emphasis font-weight-medium text-uppercase">
            Colecionáveis
          </span>
          <v-icon color="warning" size="20">mdi-star-outline</v-icon>
        </div>
        <div class="text-h4 font-weight-bold">{{ totalColectables }}</div>
        <div class="text-caption text-medium-emphasis mt-1">coletados no período</div>
      </v-card>
    </v-col>

    <!-- Nível Mais Frequente -->
    <v-col cols="12" sm="6" md="4" lg>
      <v-card
        rounded="lg"
        variant="tonal"
        color="info"
        class="pa-4"
        elevation="0"
      >
        <div class="d-flex align-center justify-space-between mb-3">
          <span class="text-caption text-medium-emphasis font-weight-medium text-uppercase">
            Nível Mais Frequente
          </span>
          <v-icon color="info" size="20">mdi-layers-outline</v-icon>
        </div>
        <div class="text-h4 font-weight-bold">{{ nivelFrequente }}</div>
        <div class="text-caption text-medium-emphasis mt-1">nível com mais sessões</div>
      </v-card>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
interface Resposta {
  id: string;
  currentLevel: string;
  time: number;
  tries: number;
  response: number;
  colectables: number;
  created_at: string;
}

const props = defineProps<{
  items: Resposta[];
}>();

const totalSessoes = computed(() => props.items.length);

const mediaResposta = computed(() => {
  if (!props.items.length) return "–";
  const avg = props.items.reduce((acc, r) => acc + r.response, 0) / props.items.length;
  return avg.toFixed(1);
});

const corEmocional = computed(() => {
  const avg = props.items.reduce((acc, r) => acc + r.response, 0) / (props.items.length || 1);
  if (avg >= 4.5) return "success";
  if (avg >= 3.5) return "success";
  if (avg >= 2.5) return "warning";
  return "error";
});

const emojiEmocional = computed(() => {
  const avg = props.items.reduce((acc, r) => acc + r.response, 0) / (props.items.length || 1);
  if (avg >= 4.5) return "😄";
  if (avg >= 3.5) return "🙂";
  if (avg >= 2.5) return "😐";
  if (avg >= 1.5) return "😕";
  return "😞";
});

const labelEmocional = computed(() => {
  const avg = props.items.reduce((acc, r) => acc + r.response, 0) / (props.items.length || 1);
  if (avg >= 4.5) return "Muito Feliz";
  if (avg >= 3.5) return "Feliz";
  if (avg >= 2.5) return "Neutro";
  if (avg >= 1.5) return "Triste";
  return "Muito Triste";
});

const tempoMedio = computed(() => {
  if (!props.items.length) return "–";
  const avg = props.items.reduce((acc, r) => acc + r.time, 0) / props.items.length;
  const min = Math.floor(avg / 60);
  const sec = Math.floor(avg % 60);
  return min > 0
    ? `${min}m ${String(sec).padStart(2, "0")}s`
    : `${sec}s`;
});

const totalColectables = computed(() =>
  props.items.reduce((acc, r) => acc + r.colectables, 0)
);

const nivelFrequente = computed(() => {
  if (!props.items.length) return "–";
  const freq: Record<string, number> = {};
  for (const r of props.items) {
    freq[r.currentLevel] = (freq[r.currentLevel] || 0) + 1;
  }
  const nivel = Object.entries(freq).sort((a, b) => b[1] - a[1])[0]?.[0] ?? "–";
  // Formata "Level_3" → "Level 3"
  return nivel.replace("_", " ");
});
</script>
