import { defineStore } from "pinia";
import axios from "axios";
import type {
  Habit,
  CreateHabitDto,
  UpdateHabitDto,
  HabitCompletionDto,
} from "../types/interfaces";

export const useHabitStore = defineStore("habit", {
  state: () => ({
    habits: [] as Habit[],
    todayHabits: [] as Habit[],
    completions: [] as HabitCompletionDto[],
    isLoading: false,
    error: "" as string | null,
  }),

  actions: {
    async fetchHabits() {
      this.isLoading = true;
      this.error = null;

      console.log("Fetching habits...");

      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/api/habits/getHabits`
        );

        if (Array.isArray(response.data)) {
          this.habits = response.data;
        } else {
          this.habits = [];
        }

        return this.habits;
      } catch (error: any) {
        this.error = error.response?.data?.message || "Failed to fetch habits.";
        console.error(this.error);
        throw new Error(this.error || "Failed to fetch habits.");
      } finally {
        this.isLoading = false;
      }
    },

    async fetchHabitById(habitId: number) {
      this.isLoading = true;
      this.error = null;
      try {
        return this.habits.find((habit) => habit.id === habitId) || null;
      } catch (error: any) {
        this.error = error.message || "Failed to fetch habit by ID.";
        console.error(this.error);
        throw new Error(this.error || "Failed to fetch habit by ID.");
      } finally {
        this.isLoading = false;
      }
    },

    async fetchTodayHabits() {
      this.isLoading = true;
      this.error = null;
      try {
        console.log("Fetching today's habits!");
        const response = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/api/habits/today`
        );

        console.log("Today's habits response:", response.data);
        if (Array.isArray(response.data)) {
          this.todayHabits = response.data;
        } else {
          this.todayHabits = [];
        }

        // Mark habits as completed based on completions array
        this.markHabitsAsCompleted();

        return this.todayHabits;
      } catch (error: any) {
        this.error =
          error.response?.data?.message || "Failed to fetch today's habits.";
        console.error(this.error);
        throw new Error(this.error || "Failed to fetch today's habits.");
      } finally {
        this.isLoading = false;
      }
    },

    markHabitsAsCompleted() {
      if (
        !Array.isArray(this.completions) ||
        !Array.isArray(this.todayHabits)
      ) {
        return;
      }

      // Create a Set of completed habit IDs for efficient lookup
      const completedHabitIds = new Set(
        this.completions.map((completion) => completion.habitId)
      );

      // Update todayHabits with completion status
      this.todayHabits = this.todayHabits.map((habit) => ({
        ...habit,
        completed: completedHabitIds.has(habit.id),
      }));
    },

    async createHabit(habitData: CreateHabitDto) {
      try {
        const response = await axios.post(
          `${import.meta.env.VITE_API_BASE_URL}/api/habits/create`,
          habitData
        );
        this.habits.push(response.data);
        return response.data;
      } catch (error: any) {
        const message =
          error.response?.data?.message || "Failed to create habit.";
        console.error(message);
        throw new Error(message);
      }
    },

    async updateHabit(habitId: number, habitData: UpdateHabitDto) {
      try {
        const response = await axios.put(
          `${
            import.meta.env.VITE_API_BASE_URL
          }/api/habits/${habitId}/updateHabit`,
          habitData
        );

        const index = this.habits.findIndex((h) => h.id === habitId);
        if (index !== -1) {
          this.habits[index] = response.data;
        }

        return response.data;
      } catch (error: any) {
        const message =
          error.response?.data?.message || "Failed to update habit.";
        console.error(message);
        throw new Error(message);
      }
    },

    async deleteHabit(habitId: number) {
      try {
        await axios.delete(
          `${import.meta.env.VITE_API_BASE_URL}/api/habits/${habitId}`
        );
        this.habits = this.habits.filter((h) => h.id !== habitId);
      } catch (error: any) {
        const message =
          error.response?.data?.message || "Failed to delete habit.";
        console.error(message);
        throw new Error(message);
      }
    },

    async completeHabit(habitId: number): Promise<HabitCompletionDto> {
      try {
        const response = await axios.post(
          `${
            import.meta.env.VITE_API_BASE_URL
          }/api/completions/${habitId}/complete`
        );

        const completion: HabitCompletionDto = response.data;

        // Update the habit's completed status in the local state
        const index = this.habits.findIndex((h) => h.id === habitId);
        if (index !== -1) {
          this.habits[index].completed = true;
        }

        return completion;
      } catch (error: any) {
        const message =
          error.response?.data?.message || "Failed to complete habit.";
        console.error(message);
        throw new Error(message);
      }
    },
    async uncompleteHabit(habitId: number): Promise<void> {
      console.log("Uncompleting habit with ID:", habitId);
      try {
        await axios.delete(
          `${
            import.meta.env.VITE_API_BASE_URL
          }/api/completions/${habitId}/uncomplete`
        );

        const index = this.habits.findIndex((h) => h.id === habitId);
        if (index !== -1) {
          this.habits[index].completed = false;
        }
      } catch (error: any) {
        const message =
          error.response?.data?.message || "Failed to undo habit completion.";
        console.error(message);
        throw new Error(message);
      }
    },

    async getHabitCompletions(habitId: number): Promise<HabitCompletionDto[]> {
      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/api/completions/${habitId}`
        );
        return response.data;
      } catch (error: any) {
        const message =
          error.response?.data?.message || "Failed to fetch habit completions.";
        console.error(message);
        throw new Error(message);
      }
    },
    
    async getUserCompletions(): Promise<HabitCompletionDto[]> {
      console.log("Fetching user completions...");

      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/api/completions/today`
        );

        this.completions = response.data;
        if (!Array.isArray(this.completions)) {
          console.warn(
            "Expected an array of completions, got:",
            this.completions
          );
          this.completions = [];
        }

        console.log("User completions response:", response.data);

        // Mark habits as completed based on the fetched completions
        this.markHabitsAsCompleted();

        return this.completions;
      } catch (error: any) {
        const message =
          error.response?.data?.message || "Failed to fetch user completions.";
        console.error(message);
        throw new Error(message);
      }
    },

    async deleteHabitCompletion(completionId: number): Promise<void> {
      try {
        await axios.delete(
          `${import.meta.env.VITE_API_BASE_URL}/api/completions/${completionId}`
        );
      } catch (error: any) {
        const message =
          error.response?.data?.message || "Failed to delete habit completion.";
        console.error(message);
        throw new Error(message);
      }
    },
  },
});
