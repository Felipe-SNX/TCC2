<template>
  <v-card elevation="2" class="rounded-lg">
    <v-card-title class="d-flex align-center pa-4 bg-surface-light">
      <v-icon class="mr-2" color="primary">mdi-account-cog</v-icon>
      <span class="text-h6 font-weight-bold">Usuários do Sistema</span>
      <v-spacer></v-spacer>
      <v-btn
        color="primary"
        prepend-icon="mdi-plus"
        size="small"
        @click="$emit('create')"
      >
        Novo Usuário
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

      <template v-slot:item.role="{ item }">
        <v-chip
          :color="item.role === 'ADMIN' ? 'error' : 'info'"
          size="small"
          variant="tonal"
        >
          <v-icon start size="small">
            {{ item.role === "ADMIN" ? "mdi-shield-crown" : "mdi-account-tie" }}
          </v-icon>
          {{ item.role === "ADMIN" ? "Administrador" : "Psicólogo" }}
        </v-chip>
      </template>

      <template v-slot:item.ativo="{ item }">
        <v-tooltip text="Ativar / Desativar usuário" location="bottom">
          <template v-slot:activator="{ props: tooltipProps }">
            <v-switch
              :model-value="item.ativo"
              color="success"
              density="compact"
              hide-details
              v-bind="tooltipProps"
              @update:model-value="$emit('toggle-ativo', item)"
            ></v-switch>
          </template>
        </v-tooltip>
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
          Nenhum usuário encontrado.
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
  (e: "toggle-ativo", item: any): void;
}>();

const headers = [
  { title: "Usuário", key: "nome", align: "start" as const, sortable: false },
  { title: "Perfil", key: "role", align: "center" as const, sortable: false },
  { title: "Ativo", key: "ativo", align: "center" as const, sortable: false },
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
</script>
