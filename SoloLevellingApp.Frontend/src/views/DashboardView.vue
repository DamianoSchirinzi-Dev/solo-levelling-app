<template>
  <div class="min-h-screen bg-gray-900 text-white">
    <div v-if="userProfile">
      <!-- Header with user stats -->
      <header class="bg-gray-800 p-6 shadow-lg">
        <div class="container mx-auto">
          <div
            class="flex flex-col md:flex-row justify-between items-center gap-4"
          >
            <div class="flex items-center gap-4">
              <div class="relative">
                <div
                  class="w-16 h-16 rounded-full bg-purple-700 flex items-center justify-center text-2xl font-bold border-2 border-purple-400"
                >
                  {{ userProfile.level }}
                </div>
                <div
                  class="absolute -bottom-1 -right-1 bg-yellow-500 text-black text-xs font-bold rounded-full w-6 h-6 flex items-center justify-center"
                >
                  {{ userProfile.level < 10 ? userProfile.level + 1 : "MAX" }}
                </div>
              </div>
              <div class="flex flex-col">
                <h1 class="text-xl font-bold">{{ userProfile.username }}</h1>
                <div class="text-sm text-gray-300">{{ userProfile.title }}</div>
              </div>
            </div>

            <div class="flex items-center gap-4">
              <div class="flex items-center gap-2">
                <span class="text-yellow-500 text-2xl">🔥</span>
                <span class="font-bold"
                  >{{ userProfile.streak }} day streak</span
                >
              </div>
            </div>
          </div>

          <!-- XP Progress Bar -->
          <div class="mt-4">
            <div class="flex justify-between text-sm mb-1">
              <span>Level {{ userProfile.level }}</span>
              <span
                >{{ userProfile.currentXP }} /
                {{ userProfile.XPToNextLevel }} XP</span
              >
            </div>
            <div class="w-full bg-gray-700 rounded-full h-4">
              <div
                class="bg-gradient-to-r from-purple-600 to-purple-400 h-4 rounded-full transition-all duration-500 ease-out"
                :style="{
                  width: `${
                    (userProfile.currentXP / userProfile.XPToNextLevel) * 100
                  }%`,
                }"
              ></div>
            </div>
          </div>
        </div>
      </header>

      <main class="container mx-auto p-6">
        <!-- Summary Panel -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
          <div class="bg-gray-800 rounded-lg p-6 shadow-lg">
            <h3 class="text-gray-400 mb-2 text-sm uppercase">
              Habits Completed
            </h3>
            <div class="text-3xl font-bold">
              {{ completedHabitsCount }} / {{ todayHabits.length }}
            </div>
          </div>
          <div class="bg-gray-800 rounded-lg p-6 shadow-lg">
            <h3 class="text-gray-400 mb-2 text-sm uppercase">
              XP Earned Today
            </h3>
            <div class="text-3xl font-bold text-purple-400">
              +{{ totalXpEarned }} XP
            </div>
          </div>
          <div class="bg-gray-800 rounded-lg p-6 shadow-lg">
            <h3 class="text-gray-400 mb-2 text-sm uppercase">Current Streak</h3>
            <div class="text-3xl font-bold flex items-center">
              <span class="text-yellow-500 mr-2">🔥</span>
              {{ userProfile.streak }} days
            </div>
          </div>
        </div>

        <!-- Habits List -->
        <div class="bg-gray-800 rounded-lg p-6 shadow-lg mb-8">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-xl font-bold mb-4">Today's Quests</h2>
            <router-link
              to="/create-habit"
              class="inline-block bg-gradient-to-r from-purple-600 to-purple-500 hover:from-purple-500 hover:to-purple-400 text-white font-medium px-6 py-2 rounded-lg shadow-md transition duration-200 mb-4"
            >
              + Create Quest
            </router-link>
          </div>

          <div v-if="loading" class="flex justify-center py-8">
            <div
              class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-purple-500"
            ></div>
          </div>

          <div
            v-else-if="todayHabits.length === 0"
            class="text-center py-8 text-gray-400"
          >
            <div class="text-5xl mb-4">🏆</div>
            <p>No habits for today. Time to create some new quests!</p>
          </div>

          <div v-else class="space-y-4">
            <div
              v-for="habit in todayHabits"
              :key="habit.id"
              class="bg-gray-700 rounded-lg p-4 flex items-center transition-all duration-200"
              :class="{
                'border-l-4 border-green-500 bg-opacity-60': habit.completed,
              }"
            >
              <div class="flex-1">
                <div class="flex items-center">
                  <h3
                    class="font-bold"
                    :class="{ 'line-through text-gray-400': habit.completed }"
                  >
                    {{ habit.name }}
                  </h3>
                  <span
                    class="ml-2 px-2 py-0.5 text-xs rounded-full"
                    :class="
                      habit.frequency === 'Daily'
                        ? 'bg-blue-600'
                        : 'bg-purple-600'
                    "
                  >
                    {{ habit.frequency }}
                  </span>
                </div>
                <p class="text-sm text-gray-400">{{ habit.description }}</p>
              </div>

              <div class="flex items-center gap-4">
                <div class="text-yellow-500 font-bold">
                  +{{ habit.xpValue }} XP
                </div>
                <button
                  @click="toggleHabitCompletion(habit)"
                  class="px-4 py-2 rounded-lg font-medium transition-colors duration-200"
                  :class="
                    habit.completed
                      ? 'bg-gray-600 text-gray-300'
                      : 'bg-gradient-to-r from-purple-600 to-purple-500 hover:from-purple-500 hover:to-purple-400 text-white'
                  "
                  :disabled="isToggling"
                >
                  {{ habit.completed ? "Completed" : "Complete" }}
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Habits Chart -->
        <div class="bg-gray-800 rounded-lg p-6 shadow-lg">
          <h2 class="text-xl font-bold mb-4">Weekly Progress</h2>
          <canvas ref="chartRef" height="100"></canvas>
        </div>
      </main>
    </div>
    <div v-else class="text-center py-20 text-gray-400">
      <div class="text-5xl mb-4 animate-pulse">🧙‍♂️</div>
      <p>Loading your adventure...</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { type UserProfile, type Habit } from "../types/interfaces";
import { useUserStore } from "../store/userStore";
import { useHabitStore } from "../store/habitStore";
import Chart from "chart.js/auto";

// Types
interface ChartData {
  date: string;
  completed: number;
  total: number;
}

// User store
const userStore = useUserStore();
const habitStore = useHabitStore();

// State
const todayHabits = computed(() => habitStore.todayHabits);
const loading = computed(() => habitStore.isLoading);
const userProfile = computed(() => userStore.userProfile as UserProfile);
const chartData = ref<ChartData[]>([]);
const isToggling = ref(false);
const chartRef = ref<HTMLCanvasElement | null>(null);
let chart: Chart | null = null;

// Computed properties
const completedHabitsCount = computed(
  () => todayHabits.value.filter((h: Habit) => h.completed).length
);

const totalXpEarned = computed(() =>
  todayHabits.value
    .filter((h: Habit) => h.completed)
    .reduce((sum: number, habit: Habit) => sum + (habit.xpValue || 0), 0)
);

// Methods

const fetchChartData = async () => {
  try {
    // In a real app, this would be an API call
    // const response = await fetch('/api/habits/history/7days')
    // chartData.value = await response.json()

    // Mock data for demonstration
    setTimeout(() => {
      const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
      const today = new Date().getDay();

      chartData.value = Array(7)
        .fill(null)
        .map((_, i) => {
          const dayIndex = (today - 6 + i + 7) % 7;
          return {
            date: days[dayIndex],
            completed: Math.floor(Math.random() * 5) + 1,
            total: 5,
          };
        });

      initChart();
    }, 1200);
  } catch (error) {
    console.error("Error fetching chart data:", error);
  }
};

const toggleHabitCompletion = async (habit: Habit) => {
  isToggling.value = true;
  try {
    console.log(`Toggling habit ${habit.name} (ID: ${habit.id})`);

    await habitStore.completeHabit(habit.id);

    habit.completed = !habit.completed;

    // Add XP
    userProfile.value.currentXP += habit.xpValue || 0;

    // Level up if enough XP
    if (userProfile.value.currentXP >= userProfile.value.XPToNextLevel) {
      userProfile.value.level += 1;
      userProfile.value.currentXP -= userProfile.value.XPToNextLevel;
      userProfile.value.XPToNextLevel = Math.round(
        userProfile.value.XPToNextLevel * 1.5
      );

      // Show level up notification (in a real app)
      alert(`🎉 Level Up! You are now level ${userProfile.value.level}!`);
    }

    console.log(
      `Habit ${habit.name} marked as ${
        !habit.completed ? "completed" : "not completed"
      }`
    );
  } catch (error) {
    console.error("Error toggling habit completion:", error);
  } finally {
    isToggling.value = false;
  }
};

const initChart = () => {
  if (!chartRef.value) return;

  const ctx = chartRef.value.getContext("2d");
  if (!ctx) return;

  if (chart) {
    chart.destroy();
  }

  chart = new Chart(ctx, {
    type: "bar",
    data: {
      labels: chartData.value.map((d) => d.date),
      datasets: [
        {
          label: "Completed",
          data: chartData.value.map((d) => d.completed),
          backgroundColor: "rgba(147, 51, 234, 0.8)",
          borderColor: "rgba(147, 51, 234, 1)",
          borderWidth: 1,
        },
        {
          label: "Total",
          data: chartData.value.map((d) => d.total),
          backgroundColor: "rgba(75, 85, 99, 0.4)",
          borderColor: "rgba(75, 85, 99, 0.6)",
          borderWidth: 1,
        },
      ],
    },
    options: {
      responsive: true,
      scales: {
        y: {
          beginAtZero: true,
          ticks: {
            color: "rgba(255, 255, 255, 0.7)",
          },
          grid: {
            color: "rgba(255, 255, 255, 0.1)",
          },
        },
        x: {
          ticks: {
            color: "rgba(255, 255, 255, 0.7)",
          },
          grid: {
            color: "rgba(255, 255, 255, 0.1)",
          },
        },
      },
      plugins: {
        legend: {
          labels: {
            color: "rgba(255, 255, 255, 0.7)",
          },
        },
      },
    },
  });
};

// Lifecycle hooks
onMounted(async () => {
  await userStore.fetchUserProfile();
  await habitStore.fetchTodayHabits();
  await habitStore.getUserCompletions();

  // Ensure habits are marked as completed after both habits and completions are fetched
  habitStore.markHabitsAsCompleted();

  fetchChartData();
});
</script>
