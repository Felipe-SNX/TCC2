<template>
  <v-card elevation="2" class="rounded-lg">
    <v-card-title class="d-flex align-center pa-4 bg-surface-light">
      <v-icon class="mr-2" color="primary">mdi-help-circle-outline</v-icon>
      <span class="text-h6 font-weight-bold">Perguntas do Sistema</span>
      <v-spacer></v-spacer>
      <v-btn
        color="primary"
        prepend-icon="mdi-plus"
        size="small"
        @click="$emit('create')"
      >
        Nova Pergunta
      </v-btn>
    </v-card-title>

    <v-divider></v-divider>

    <v-data-table-server
      :headers="headers"
      :items="items"
      :items-length="totalItems"
      :loading="loading"
      :items-per-page-options="[10, 25, 50, 100]"
      v-model:page="options.page"
      v-model:items-per-page="options.itemsPerPage"
      @update:options="handleOptionsUpdate"
      class="elevation-0"
      hover
    >
      <template v-slot:item.pergunta="{ item }">
        <div class="d-flex align-center py-2">
          <v-icon color="primary" class="mr-3" size="small">mdi-chat-question</v-icon>
          <div class="font-weight-medium">{{ item.pergunta }}</div>
        </div>
      </template>

      <template v-slot:item.alternativas="{ item }">
        <div class="d-flex ga-1 flex-wrap py-1">
          <v-chip
            v-for="(alt, idx) in item.alternativas"
            :key="idx"
            size="x-small"
            variant="tonal"
            :color="chipColor(alt.valor)"
          >
            {{ alt.valor }} - {{ alt.texto }}
          </v-chip>
        </div>
      </template>

      <template v-slot:item.created_at="{ item }">
        <span class="text-body-2 text-medium-emphasis">
          {{ formatDate(item.created_at) }}
        </span>
      </template>

      <template v-slot:item.acoes="{ item }">
        <v-tooltip text="Editar" location="top">
          <template v-slot:activator="{ props: tooltipProps }">
            <v-btn
              icon="mdi-pencil"
              variant="text"
              size="small"
              color="warning"
              v-bind="tooltipProps"
              @click="$emit('edit', item)"
            ></v-btn>
          </template>
        </v-tooltip>
        <v-tooltip text="Excluir" location="top">
          <template v-slot:activator="{ props: tooltipProps }">
            <v-btn
              icon="mdi-delete"
              variant="text"
              size="small"
              color="error"
              v-bind="tooltipProps"
              @click="$emit('delete', item)"
            ></v-btn>
          </template>
        </v-tooltip>
      </template>

      <template v-slot:no-data>
        <div class="pa-4 text-center text-medium-emphasis">
          Nenhuma pergunta encontrada.
        </div>
      </template>
    </v-data-table-server>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from "vue";

const props = defineProps<{
  items: any[];
  totalItems: number;
  loading: boolean;
}>();

const emit = defineEmits<{
  (e: "update:options", options: { page: number; itemsPerPage: number }): void;
  (e: "create"): void;
  (e: "edit", item: any): void;
  (e: "delete", item: any): void;
}>();

const headers = [
  { title: "Pergunta", key: "pergunta", align: "start" as const, sortable: false },
  { title: "Alternativas", key: "alternativas", align: "start" as const, sortable: false },
  { title: "Criado em", key: "created_at", align: "center" as const, sortable: false },
  { title: "Ações", key: "acoes", align: "end" as const, sortable: false },
];

const options = ref({
  page: 1,
  itemsPerPage: 25,
});

const handleOptionsUpdate = (newOptions: any) => {
  emit("update:options", {
    page: newOptions.page,
    itemsPerPage: newOptions.itemsPerPage,
  });
};

const chipColor = (valor: number): string => {
  const colors: Record<number, string> = {
    1: 'error',
    2: 'warning',
    3: 'grey',
    4: 'success',
    5: 'info'
  }
  return colors[valor] || 'grey'
}

const formatDate = (dateStr: string): string => {
  if (!dateStr) return '-'
  return new Date(dateStr).toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  })
}
</script>
