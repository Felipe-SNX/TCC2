<template>
  <v-row align="stretch">
    <!-- Total de Sessões -->
    <v-col cols="12" sm="6" lg class="d-flex">
      <v-card rounded="lg" variant="elevated" class="pa-4 w-100" elevation="2">
        <div class="d-flex align-center ga-2 mb-4">
          <v-icon color="primary" size="22">mdi-gamepad-variant-outline</v-icon>
          <span class="text-overline text-medium-emphasis text-no-wrap"
            >Sessões</span
          >
        </div>
        <span class="text-h4 font-weight-bold mb-1">{{ totalSessoes }}</span>
        <span class="text-caption text-medium-emphasis">&nbsp;no período</span>
      </v-card>
    </v-col>

    <!-- Resposta Emocional Média -->
    <v-col cols="12" sm="6" lg class="d-flex">
      <v-card rounded="lg" variant="elevated" class="pa-4 w-100" elevation="2">
        <div class="d-flex align-center ga-2 mb-4">
          <v-icon color="primary" size="22">mdi-emoticon-outline</v-icon>
          <span class="text-overline text-medium-emphasis text-no-wrap"
            >Humor Médio</span
          >
        </div>
        <div class="d-flex align-center ga-2 mb-1">
          <span class="text-h4 font-weight-bold">{{ mediaResposta }}</span>
          <span style="font-size: 1.4rem; line-height: 1">{{
            emojiEmocional
          }}</span>
          <span class="text-caption text-medium-emphasis">
            &nbsp;{{ labelEmocional }}
          </span>
        </div>
      </v-card>
    </v-col>

    <!-- Tempo Médio por Sessão -->
    <v-col cols="12" sm="6" lg class="d-flex">
      <v-card rounded="lg" variant="elevated" class="pa-4 w-100" elevation="2">
        <div class="d-flex align-center ga-2 mb-4">
          <v-icon color="primary" size="22">mdi-timer-outline</v-icon>
          <span class="text-overline text-medium-emphasis text-no-wrap"
            >Tempo Médio</span
          >
        </div>
        <span class="text-h4 font-weight-bold mb-1">{{ tempoMedio }}</span>
        <span class="text-caption text-medium-emphasis">/ sessão</span>
      </v-card>
    </v-col>

    <!-- Colecionáveis Totais -->
    <v-col cols="12" sm="6" lg class="d-flex">
      <v-card rounded="lg" variant="elevated" class="pa-4 w-100" elevation="2">
        <div class="d-flex align-center ga-2 mb-4">
          <v-icon color="primary" size="22">mdi-star-outline</v-icon>
          <span class="text-overline text-medium-emphasis text-no-wrap"
            >Colecionáveis</span
          >
        </div>
        <span class="text-h4 font-weight-bold mb-1">{{
          totalColectables
        }}</span>
        <span class="text-caption text-medium-emphasis">
          &nbsp;coletados no período
        </span>
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

const avgResponse = computed(() => {
  if (!props.items.length) return 0;
  return (
    props.items.reduce((acc, r) => acc + r.response, 0) / props.items.length
  );
});

const mediaResposta = computed(() =>
  props.items.length ? avgResponse.value.toFixed(1) : "–",
);

const emojiEmocional = computed(() => {
  const avg = avgResponse.value;
  if (avg >= 4.5) return "😄";
  if (avg >= 3.5) return "🙂";
  if (avg >= 2.5) return "😐";
  if (avg >= 1.5) return "😕";
  return "😞";
});

const labelEmocional = computed(() => {
  const avg = avgResponse.value;
  if (avg >= 4.5) return "Muito Feliz";
  if (avg >= 3.5) return "Feliz";
  if (avg >= 2.5) return "Neutro";
  if (avg >= 1.5) return "Triste";
  return "Muito Triste";
});

const tempoMedio = computed(() => {
  if (!props.items.length) return "–";
  const avg =
    props.items.reduce((acc, r) => acc + r.time, 0) / props.items.length;
  const min = Math.floor(avg / 60);
  const sec = Math.floor(avg % 60);
  return min > 0 ? `${min}m ${String(sec).padStart(2, "0")}s` : `${sec}s`;
});

const totalColectables = computed(() =>
  props.items.reduce((acc, r) => acc + r.colectables, 0),
);
</script>
