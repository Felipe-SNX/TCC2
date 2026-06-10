<template>
  <div class="chart-wrapper">
    <Line v-if="chartData.labels.length > 0" :data="chartData" :options="chartOptions" />
    <div v-else class="d-flex align-center justify-center fill-height text-medium-emphasis">
      Não há dados suficientes para exibir o gráfico neste período.
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  Filler,
  ChartData,
  ChartOptions
} from "chart.js";
import { Line } from "vue-chartjs";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  Filler
);

const props = defineProps<{
  items: any[];
}>();

const formatDateTime = (dateString: string) => {
  if (!dateString) return "-";
  const date = new Date(dateString);
  return date.toLocaleDateString("pt-BR", {
    day: "2-digit",
    month: "2-digit",
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
  return map[value] || `Desconhecido`;
};

const chartData = computed<ChartData<"line">>(() => {
  const sortedItems = [...props.items].sort((a, b) => {
    return new Date(a.created_at).getTime() - new Date(b.created_at).getTime();
  });

  const labels = sortedItems.map((item) => formatDateTime(item.created_at));
  const dataPoints = sortedItems.map((item) => item.resposta);

  return {
    labels,
    datasets: [
      {
        label: "Estado Emocional",
        backgroundColor: "rgba(33, 150, 243, 0.2)",
        borderColor: "#2196F3",
        pointBackgroundColor: "#2196F3",
        pointBorderColor: "#fff",
        pointHoverBackgroundColor: "#fff",
        pointHoverBorderColor: "#2196F3",
        borderWidth: 2,
        tension: 0.4,
        fill: true,
        data: dataPoints,
      },
    ],
  };
});

const chartOptions = computed<ChartOptions<"line">>(() => ({
  responsive: true,
  maintainAspectRatio: false,
  scales: {
    y: {
      min: 1,
      max: 5,
      ticks: {
        stepSize: 1,
        callback: function (value) {
          return getEmotionText(value as number);
        },
      },
    },
  },
  plugins: {
    legend: {
      display: false,
    },
    tooltip: {
      callbacks: {
        label: function (context) {
          const val = context.parsed.y;
          return `Estado: ${getEmotionText(val)} (${val})`;
        },
      },
    },
  },
}));
</script>

<style scoped>
.chart-wrapper {
  position: relative;
  height: 300px;
  width: 100%;
}
</style>
