<template>
  <div class="min-h-screen bg-gray-900 text-white">
    <!-- Header -->
    <header class="bg-gray-800 p-6 shadow-lg">
      <div class="container mx-auto">
        <div class="flex items-center gap-4">
          <button
            @click="goBack"
            class="p-2 rounded-lg bg-gray-700 hover:bg-gray-600 transition-colors"
          >
            <ArrowLeftIcon class="w-5 h-5" />
          </button>
          <div>
            <h1 class="text-2xl font-bold">Create New Quest</h1>
            <p class="text-gray-400">Design your next challenge</p>
          </div>
        </div>
      </div>
    </header>

    <main class="container mx-auto p-6">
      <div class="max-w-2xl mx-auto">
        <!-- Form Card -->
        <div class="bg-gray-800 rounded-lg p-8 shadow-lg">
          <form @submit.prevent="submitHabit" class="space-y-6">
            <!-- Title Field -->
            <div>
              <label for="name" class="block text-sm font-medium mb-2">
                Quest Title <span class="text-red-500">*</span>
              </label>
              <input
                id="name"
                v-model="form.name"
                type="text"
                required
                maxlength="20"
                class="w-full px-4 py-3 bg-gray-700 border border-gray-600 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-colors"
                :class="{ 'border-red-500': errors.name }"
                placeholder="e.g., Morning Meditation, Daily Coding Practice"
              />
              <div v-if="errors.name" class="text-red-400 text-sm mt-1">
                {{ errors.name }}
              </div>
              <div class="text-gray-400 text-sm mt-1">
                {{ form.name.length }}/20 characters
              </div>
            </div>

            <!-- Description Field -->
            <div>
              <label for="description" class="block text-sm font-medium mb-2">
                Quest Description
              </label>
              <textarea
                id="description"
                v-model="form.description"
                rows="3"
                maxlength="250"
                class="w-full px-4 py-3 bg-gray-700 border border-gray-600 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-colors resize-none"
                :class="{ 'border-red-500': errors.description }"
                placeholder="Describe what this habit involves and why it's important to you..."
              ></textarea>
              <div v-if="errors.description" class="text-red-400 text-sm mt-1">
                {{ errors.description }}
              </div>
              <div
                class="text-gray-400 text-sm mt-1"
                :class="{ 'text-red-400': form.description.length > 250 }"
              >
                {{ form.description.length }}/250 characters
              </div>
            </div>

            <!-- Enhanced Frequency Field with validation -->
            <div>
              <label class="block text-sm font-medium mb-2">
                Quest Frequency <span class="text-red-500">*</span>
              </label>
              <div
                class="space-y-2"
                :class="{
                  'border border-red-500 rounded-lg p-2': errors.frequency,
                }"
              >
                <label class="flex items-center">
                  <input
                    v-model.number="form.frequency"
                    type="radio"
                    :value="HabitFrequency.Daily"
                    class="w-4 h-4 text-purple-600 bg-gray-700 border-gray-600 focus:ring-purple-500"
                  />
                  <span class="ml-2">Daily Quest</span>
                  <span class="ml-auto text-sm text-gray-400">Every day</span>
                </label>
                <label class="flex items-center">
                  <input
                    v-model.number="form.frequency"
                    type="radio"
                    :value="HabitFrequency.Weekly"
                    class="w-4 h-4 text-purple-600 bg-gray-700 border-gray-600 focus:ring-purple-500"
                  />
                  <span class="ml-2">Weekly Quest</span>
                  <span class="ml-auto text-sm text-gray-400"
                    >Once per week</span
                  >
                </label>
              </div>
              <div v-if="errors.frequency" class="text-red-400 text-sm mt-1">
                {{ errors.frequency }}
              </div>
            </div>

            <!-- Enhanced Difficulty Level with validation -->
            <div>
              <label class="block text-sm font-medium mb-2">
                Difficulty Level <span class="text-red-500">*</span>
              </label>
              <div
                class="space-y-2"
                :class="{
                  'border border-red-500 rounded-lg p-2': errors.difficulty,
                }"
              >
                <label
                  v-for="difficulty in difficultyLevels"
                  :key="difficulty.value"
                  class="flex items-center p-3 rounded-lg border transition-colors cursor-pointer"
                  :class="
                    form.difficulty === difficulty.enumValue
                      ? 'border-purple-500 bg-purple-900/30'
                      : 'border-gray-600 hover:border-gray-500'
                  "
                >
                  <input
                    v-model.number="form.difficulty"
                    type="radio"
                    :value="difficulty.enumValue"
                    class="w-4 h-4 text-purple-600 bg-gray-700 border-gray-600 focus:ring-purple-500"
                  />
                  <div class="ml-3 flex-1">
                    <div class="flex items-center gap-2">
                      <span>{{ difficulty.emoji }} {{ difficulty.label }}</span>
                      <span class="text-yellow-500 font-bold"
                        >+{{ difficulty.xp }} XP</span
                      >
                    </div>
                    <div class="text-sm text-gray-400">
                      {{ difficulty.description }}
                    </div>
                  </div>
                </label>
              </div>
              <div v-if="errors.difficulty" class="text-red-400 text-sm mt-1">
                {{ errors.difficulty }}
              </div>
            </div>

            <!-- Enhanced Custom XP with validation -->
            <div>
              <label for="xpValue" class="block text-sm font-medium mb-2">
                Custom XP Value
              </label>
              <input
                id="xpValue"
                v-model.number="form.xpValue"
                type="number"
                min="1"
                max="200"
                class="w-full px-4 py-3 bg-gray-700 border border-gray-600 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-colors"
                :class="{ 'border-red-500': errors.xpValue }"
                placeholder="Leave empty to use difficulty XP"
              />
              <div v-if="errors.xpValue" class="text-red-400 text-sm mt-1">
                {{ errors.xpValue }}
              </div>
              <div class="text-gray-400 text-sm mt-1">
                Override the difficulty-based XP value (1-200)
              </div>
            </div>

            <!-- Enhanced Reminder Time with validation -->
            <div v-if="form.reminderEnabled" class="ml-6 space-y-2">
              <div>
                <label
                  for="reminderTime"
                  class="block text-sm text-gray-400 mb-1"
                >
                  Reminder Time <span class="text-red-500">*</span>
                </label>
                <input
                  id="reminderTime"
                  v-model="form.reminderTime"
                  type="time"
                  class="px-3 py-2 bg-gray-700 border border-gray-600 rounded focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                  :class="{ 'border-red-500': errors.reminderTime }"
                />
                <div
                  v-if="errors.reminderTime"
                  class="text-red-400 text-sm mt-1"
                >
                  {{ errors.reminderTime }}
                </div>
              </div>
            </div>

            <!-- Form validation summary (optional) -->
            <div
              v-if="hasErrors && Object.values(errors).some((e) => e !== '')"
              class="bg-red-900/30 border border-red-700 rounded-lg p-4"
            >
              <h4 class="text-red-400 font-medium mb-2">
                Please fix the following errors:
              </h4>
              <ul class="text-red-300 text-sm space-y-1">
                <li v-for="(error, field) in errors" :key="field" v-if="errors">
                  {{ error }}
                </li>
              </ul>
            </div>

            <!-- XP Preview -->
            <div
              class="bg-gradient-to-r from-purple-900/50 to-purple-800/50 rounded-lg p-4 border border-purple-700/50"
            >
              <div class="flex items-center justify-between">
                <div>
                  <h3 class="font-medium">Quest Reward Preview</h3>
                  <p class="text-sm text-gray-300">
                    XP you'll earn for completing this quest
                  </p>
                </div>
                <div class="text-right">
                  <div class="text-2xl font-bold text-yellow-500">
                    +{{ finalXpValue }} XP
                  </div>
                  <div class="text-sm text-gray-400">per completion</div>
                </div>
              </div>
            </div>

            <!-- Form Actions -->
            <div class="flex gap-4 pt-4">
              <button
                type="button"
                @click="goBack"
                class="flex-1 px-6 py-3 bg-gray-700 hover:bg-gray-600 rounded-lg font-medium transition-colors"
              >
                Cancel
              </button>
              <button
                type="submit"
                :disabled="isSubmitting || !isFormValid"
                class="flex-1 px-6 py-3 bg-gradient-to-r from-purple-600 to-purple-500 hover:from-purple-500 hover:to-purple-400 disabled:from-gray-600 disabled:to-gray-600 disabled:cursor-not-allowed rounded-lg font-medium transition-colors flex items-center justify-center gap-2"
              >
                <LoadingSpinner v-if="isSubmitting" class="w-5 h-5" />
                <span>{{
                  isSubmitting ? "Creating Quest..." : "Create Quest"
                }}</span>
              </button>
            </div>
          </form>
        </div>

        <!-- Success Message -->
        <div
          v-if="showSuccess"
          class="mt-6 bg-green-900/50 border border-green-700 rounded-lg p-4 flex items-center gap-3"
        >
          <CheckCircleIcon class="w-6 h-6 text-green-400" />
          <div>
            <h3 class="font-medium text-green-400">
              Quest Created Successfully!
            </h3>
            <p class="text-sm text-green-300">
              Your new habit has been added to your daily quests.
            </p>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, reactive } from "vue";
import { useHabitStore } from "@/store/habitStore";

const habitStore = useHabitStore();

// Icons
const ArrowLeftIcon = {
  template:
    '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m12 19-7-7 7-7"/><path d="M19 12H5"/></svg>',
};
const BellIcon = {
  template:
    '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M6 8a6 6 0 0 1 12 0c0 7 3 9 3 9H3s3-2 3-9"/><path d="M10.3 21a1.94 1.94 0 0 0 3.4 0"/></svg>',
};
const CheckCircleIcon = {
  template:
    '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/><path d="m9 11 3 3L22 4"/></svg>',
};
const LoadingSpinner = {
  template:
    '<svg class="animate-spin" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"/><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"/></svg>',
};

// Types
interface HabitForm {
  name: string;
  description: string;
  frequency: HabitFrequency;
  category: string;
  difficulty: Difficulty;
  xpValue: number | null;
  reminderEnabled: boolean;
  reminderTime: string;
}

interface DifficultyLevel {
  value: string;
  enumValue: Difficulty;
  label: string;
  emoji: string;
  xp: number;
  description: string;
}

// State
const form = reactive<HabitForm>({
  name: "",
  description: "",
  frequency: HabitFrequency.Daily,
  category: "",
  difficulty: Difficulty.Easy,
  xpValue: null,
  reminderEnabled: false,
  reminderTime: "09:00",
});

const isSubmitting = ref(false);
const showSuccess = ref(false);

// Difficulty levels configuration
const difficultyLevels: DifficultyLevel[] = [
  {
    value: "Easy",
    enumValue: Difficulty.Easy,
    label: "Novice",
    emoji: "🟢",
    xp: 10,
    description: "Simple habits that take 5-15 minutes",
  },
  {
    value: "Medium",
    enumValue: Difficulty.Medium,
    label: "Adventurer",
    emoji: "🟡",
    xp: 25,
    description: "Moderate habits that take 15-30 minutes",
  },
  {
    value: "Hard",
    enumValue: Difficulty.Hard,
    label: "Warrior",
    emoji: "🟠",
    xp: 50,
    description: "Challenging habits that take 30-60 minutes",
  },
  {
    value: "Expert",
    enumValue: Difficulty.Expert,
    label: "Legend",
    emoji: "🔴",
    xp: 100,
    description: "Intensive habits that take 1+ hours",
  },
];

// Computed properties
const finalXpValue = computed(() => {
  if (form.xpValue && form.xpValue > 0) {
    return form.xpValue;
  }
  const difficulty = difficultyLevels.find(
    (d) => d.enumValue === form.difficulty
  );
  console.log("Final XP Value Computed:", difficulty?.xp);
  return difficulty?.xp || 10;
});

const errors = reactive({
  name: "",
  description: "",
  frequency: "",
  difficulty: "",
  xpValue: "",
  reminderTime: "",
});

const isFormValid = computed(() => {
  return (
    form.name.trim().length >= 3 &&
    form.name.trim().length <= 20 &&
    form.description.length <= 250 &&
    form.frequency !== null &&
    form.difficulty !== null &&
    (!form.xpValue || (form.xpValue >= 1 && form.xpValue <= 200)) &&
    (!form.reminderEnabled || form.reminderTime) &&
    !hasErrors.value
  );
});

const hasErrors = computed(() => {
  return Object.values(errors).some((error) => error !== "");
});

// Validation Methods
const validateTitle = () => {
  errors.name = "";
  if (!form.name.trim()) {
    errors.name = "Quest name is required";
  } else if (form.name.length < 3) {
    errors.name = "Quest name must be at least 3 characters";
  } else if (form.name.length > 100) {
    errors.name = "Quest name must be less than 100 characters";
  }
};

const validateDescription = () => {
  errors.description = "";
  if (form.description.length > 250) {
    errors.description = "Description must be 250 characters or less";
  }
};

const validateFrequency = () => {
  errors.frequency = "";
  if (form.frequency === null || form.frequency === undefined) {
    errors.frequency = "Please select a quest frequency";
  }
};

const validateDifficulty = () => {
  errors.difficulty = "";
  if (form.difficulty === null || form.difficulty === undefined) {
    errors.difficulty = "Please select a difficulty level";
  }
};

const validateXpValue = () => {
  errors.xpValue = "";
  if (form.xpValue !== null && form.xpValue !== undefined) {
    if (form.xpValue < 1) {
      errors.xpValue = "XP value must be at least 1";
    } else if (form.xpValue > 200) {
      errors.xpValue = "XP value cannot exceed 200";
    }
  }
};

const validateReminderTime = () => {
  errors.reminderTime = "";
  if (form.reminderEnabled && !form.reminderTime) {
    errors.reminderTime =
      "Reminder time is required when reminders are enabled";
  }
};

const validateAllFields = () => {
  validateTitle();
  validateDescription();
  validateFrequency();
  validateDifficulty();
  validateXpValue();
  validateReminderTime();
};

// Methods
const submitHabit = async () => {
  // Validate all fields before submission
  validateAllFields();

  if (!isFormValid.value) {
    console.log("Form validation failed:", errors);
    return;
  }

  isSubmitting.value = true;

  try {
    const habitData: HabitForm = {
      name: form.name.trim(),
      description: form.description.trim(),
      frequency: form.frequency,
      category: form.category || "Other",
      difficulty: form.difficulty,
      xpValue: finalXpValue.value,
      reminderEnabled: form.reminderEnabled,
      reminderTime: form.reminderTime || "09:00",
    };

    const response = await habitStore.createHabit(habitData);

    if (response.error) {
      throw new Error("Failed to create habit");
    }

    // Show success message
    showSuccess.value = true;

    // Reset form after a delay
    setTimeout(() => {
      resetForm();
      showSuccess.value = false;
    }, 3000);
  } catch (error) {
    console.error("Error creating habit:", error);
    alert("Failed to create quest. Please try again.");
  } finally {
    isSubmitting.value = false;
  }
};

const resetForm = () => {
  Object.assign(form, {
    name: "",
    description: "",
    frequency: HabitFrequency.Daily,
    category: "",
    difficulty: Difficulty.Easy,
    xpValue: null,
    reminderEnabled: false,
    reminderTime: "09:00",
  });
  Object.assign(errors, {
    name: "",
    description: "",
    frequency: "",
    difficulty: "",
    xpValue: "",
    reminderTime: "",
  });
};

const goBack = () => {
  // In a real app, this would use Vue Router
  // router.go(-1) or router.push('/dashboard')
  window.history.back();
};

// Watch for name changes to validate in real-time
import { watch } from "vue";
import { Difficulty, HabitFrequency } from "../types/enums";

watch(() => form.name, validateTitle);
watch(() => form.description, validateDescription);
watch(() => form.frequency, validateFrequency);
watch(() => form.difficulty, validateDifficulty);
watch(() => form.xpValue, validateXpValue);
watch(() => form.reminderTime, validateReminderTime);
watch(() => form.reminderEnabled, validateReminderTime);
</script>
