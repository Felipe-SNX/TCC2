<template>
  <v-card elevation="2" class="rounded-lg">
    <v-data-table
      :headers="headers"
      :items="items"
      :loading="loading"
      :items-per-page="10"
      class="elevation-0"
      hover
    >
      <template v-slot:item.created_at="{ item }">
        {{ formatDateTime(item.created_at) }}
      </template>

      <template v-slot:item.cor="{ item }">
        <div class="d-flex align-center">
          <div
            class="color-dot mr-2"
            :style="{ backgroundColor: item.cor }"
          ></div>
          <span class="text-capitalize">{{ item.cor }}</span>
        </div>
      </template>

      <template v-slot:item.resposta="{ item }">
        <v-chip
          :color="getEmotionColor(item.resposta)"
          size="small"
          variant="tonal"
        >
          <v-icon start size="small">
            {{ getEmotionIcon(item.resposta) }}
          </v-icon>
          {{ getEmotionText(item.resposta) }}
        </v-chip>
      </template>

      <template v-slot:no-data>
        <div class="pa-4 text-center text-medium-emphasis">
          Nenhuma resposta encontrada para este período.
        </div>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup lang="ts">
const props = defineProps<{
  items: any[];
  loading: boolean;
}>();

const headers = [
  {
    title: "Data e Hora",
    key: "created_at",
    align: "start" as const,
    sortable: true,
  },
  {
    title: "Cor da Fase",
    key: "cor",
    align: "center" as const,
    sortable: true,
  },
  {
    title: "Resposta (1-5)",
    key: "resposta",
    align: "center" as const,
    sortable: true,
  },
];

const formatDateTime = (dateString: string) => {
  if (!dateString) return "-";
  const date = new Date(dateString);
  return date.toLocaleString("pt-BR", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};

const getEmotionText = (value: number) => {
  const map: Record<number, string> = {
    1: "Muito Triste",
    2: "Triste",
    3: "Neutro",
    4: "Feliz",
    5: "Muito Feliz",
  };
  return map[value] || `Desconhecido (${value})`;
};

const getEmotionColor = (value: number) => {
  const map: Record<number, string> = {
    1: "error",
    2: "warning",
    3: "grey-darken-1",
    4: "info",
    5: "success",
  };
  return map[value] || "grey";
};

const getEmotionIcon = (value: number) => {
  const map: Record<number, string> = {
    1: "mdi-emoticon-cry-outline",
    2: "mdi-emoticon-sad-outline",
    3: "mdi-emoticon-neutral-outline",
    4: "mdi-emoticon-happy-outline",
    5: "mdi-emoticon-excited-outline",
  };
  return map[value] || "mdi-help-circle-outline";
};
</script>

<style scoped>
.color-dot {
  width: 16px;
  height: 16px;
  border-radius: 50%;
  border: 1px solid rgba(0, 0, 0, 0.12);
}
</style>
