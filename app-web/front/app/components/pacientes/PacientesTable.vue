<template>
  <v-card elevation="2" class="rounded-lg">
    <v-card-title class="d-flex align-center pa-4 bg-surface-light">
      <v-text-field
        v-model="search"
        prepend-inner-icon="mdi-magnify"
        label="Buscar paciente por nome ou e-mail"
        variant="outlined"
        density="compact"
        color="primary"
        hide-details
        clearable
        class="mr-4"
        style="max-width: 380px"
      />
      <v-spacer></v-spacer>
      <v-btn
        color="primary"
        prepend-icon="mdi-plus"
        size="small"
        @click="$emit('create')"
      >
        Novo Paciente
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
      <template v-slot:item.nome="{ item }">
        <div class="d-flex align-center py-2">
          <v-avatar color="primary" size="36" class="mr-3 text-white">
            {{ item.nome.charAt(0).toUpperCase() }}
          </v-avatar>
          <div>
            <div class="font-weight-medium">{{ item.nome }}</div>
            <div class="text-caption text-medium-emphasis">
              {{ item.email }}
            </div>
          </div>
        </div>
      </template>

      <template v-slot:item.pin="{ item }">
        <div class="d-flex align-center">
          <span class="font-weight-medium mr-2">{{ item.pin || "N/A" }}</span>
          <v-tooltip text="Renovar PIN" location="top">
            <template v-slot:activator="{ props: tooltipProps }">
              <v-btn
                icon="mdi-refresh"
                variant="text"
                size="x-small"
                color="primary"
                v-bind="tooltipProps"
                @click="$emit('refresh-pin', item)"
              ></v-btn>
            </template>
          </v-tooltip>
        </div>
      </template>

      <template v-slot:item.observacoes="{ item }">
        <small
          class="text-medium-emphasis"
          style="
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: normal;
          "
          :title="item.observacoes"
        >
          {{ item.observacoes || "-" }}
        </small>
      </template>

      <template v-slot:item.created_at="{ item }">
        <span class="text-caption text-medium-emphasis text-no-wrap">
          {{ formatDateTime(item.created_at) }}
        </span>
      </template>

      <template v-slot:item.acoes="{ item }">
        <v-tooltip text="Ver Respostas" location="top">
          <template v-slot:activator="{ props: tooltipProps }">
            <v-btn
              icon="mdi-chart-line"
              variant="text"
              size="small"
              color="secondary"
              v-bind="tooltipProps"
              @click="$emit('view-dashboard', item)"
            ></v-btn>
          </template>
        </v-tooltip>
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
          Nenhum paciente encontrado.
        </div>
      </template>
    </v-data-table-server>
  </v-card>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";

const props = defineProps<{
  items: any[];
  totalItems: number;
  loading: boolean;
}>();

const emit = defineEmits<{
  (e: "update:options", options: { page: number; itemsPerPage: number }): void;
  (e: "update:search", search: string): void;
  (e: "create"): void;
  (e: "edit", item: any): void;
  (e: "delete", item: any): void;
  (e: "view-dashboard", item: any): void;
  (e: "refresh-pin", item: any): void;
}>();

const search = ref("");
let debounceTimer: ReturnType<typeof setTimeout> | null = null;

watch(search, (value) => {
  if (debounceTimer) clearTimeout(debounceTimer);
  debounceTimer = setTimeout(() => {
    emit("update:search", value ?? "");
  }, 400);
});

const headers = [
  { title: "Paciente", key: "nome", align: "start" as const, sortable: false },
  { title: "Idade", key: "idade", align: "center" as const, sortable: false },
  { title: "PIN", key: "pin", align: "start" as const, sortable: false },
  {
    title: "Observações",
    key: "observacoes",
    align: "start" as const,
    sortable: false,
    width: "30%",
  },
  {
    title: "Criado em",
    key: "created_at",
    align: "start" as const,
    sortable: false,
  },
  { title: "Ações", key: "acoes", align: "end" as const, sortable: false },
];

const formatDateTime = (dateString: string) => {
  if (!dateString) return "-";
  const utcString = dateString.endsWith("Z") ? dateString : dateString + "Z";
  const date = new Date(utcString);
  return date.toLocaleString("pt-BR", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
    timeZone: "America/Sao_Paulo",
  });
};

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
</script>
