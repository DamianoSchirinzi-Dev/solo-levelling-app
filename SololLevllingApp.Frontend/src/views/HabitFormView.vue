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
              <label for="title" class="block text-sm font-medium mb-2">
                Quest Title <span class="text-red-500">*</span>
              </label>
              <input
                id="title"
                v-model="form.title"
                type="text"
                required
                maxlength="100"
                class="w-full px-4 py-3 bg-gray-700 border border-gray-600 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-colors"
                :class="{ 'border-red-500': errors.title }"
                placeholder="e.g., Morning Meditation, Daily Coding Practice"
              />
              <div v-if="errors.title" class="text-red-400 text-sm mt-1">
                {{ errors.title }}
              </div>
              <div class="text-gray-400 text-sm mt-1">
                {{ form.title.length }}/100 characters
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
                maxlength="500"
                class="w-full px-4 py-3 bg-gray-700 border border-gray-600 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-colors resize-none"
                placeholder="Describe what this habit involves and why it's important to you..."
              ></textarea>
              <div class="text-gray-400 text-sm mt-1">
                {{ form.description.length }}/500 characters
              </div>
            </div>

            <!-- Frequency and Category Row -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <!-- Frequency Field -->
              <div>
                <label class="block text-sm font-medium mb-2">
                  Quest Frequency <span class="text-red-500">*</span>
                </label>
                <div class="space-y-2">
                  <label class="flex items-center">
                    <input
                      v-model="form.frequency"
                      type="radio"
                      value="Daily"
                      class="w-4 h-4 text-purple-600 bg-gray-700 border-gray-600 focus:ring-purple-500"
                    />
                    <span class="ml-2">Daily Quest</span>
                    <span class="ml-auto text-sm text-gray-400">Every day</span>
                  </label>
                  <label class="flex items-center">
                    <input
                      v-model="form.frequency"
                      type="radio"
                      value="Weekly"
                      class="w-4 h-4 text-purple-600 bg-gray-700 border-gray-600 focus:ring-purple-500"
                    />
                    <span class="ml-2">Weekly Quest</span>
                    <span class="ml-auto text-sm text-gray-400">Once per week</span>
                  </label>
                </div>
              </div>

              <!-- Category Field -->
              <div>
                <label for="category" class="block text-sm font-medium mb-2">
                  Quest Category
                </label>
                <select
                  id="category"
                  v-model="form.category"
                  class="w-full px-4 py-3 bg-gray-700 border border-gray-600 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-colors"
                >
                  <option value="">Select a category</option>
                  <option value="Health">🏃 Health & Fitness</option>
                  <option value="Learning">📚 Learning & Skills</option>
                  <option value="Productivity">⚡ Productivity</option>
                  <option value="Mindfulness">🧘 Mindfulness</option>
                  <option value="Social">👥 Social & Relationships</option>
                  <option value="Creative">🎨 Creative & Hobbies</option>
                  <option value="Career">💼 Career & Finance</option>
                  <option value="Other">🌟 Other</option>
                </select>
              </div>
            </div>

            <!-- Difficulty and XP Row -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <!-- Difficulty Level -->
              <div>
                <label class="block text-sm font-medium mb-2">
                  Difficulty Level <span class="text-red-500">*</span>
                </label>
                <div class="space-y-2">
                  <label 
                    v-for="difficulty in difficultyLevels" 
                    :key="difficulty.value"
                    class="flex items-center p-3 rounded-lg border transition-colors cursor-pointer"
                    :class="form.difficulty === difficulty.value ? 
                      'border-purple-500 bg-purple-900/30' : 
                      'border-gray-600 hover:border-gray-500'"
                  >
                    <input
                      v-model="form.difficulty"
                      type="radio"
                      :value="difficulty.value"
                      class="w-4 h-4 text-purple-600 bg-gray-700 border-gray-600 focus:ring-purple-500"
                    />
                    <div class="ml-3 flex-1">
                      <div class="flex items-center gap-2">
                        <span>{{ difficulty.emoji }} {{ difficulty.label }}</span>
                        <span class="text-yellow-500 font-bold">+{{ difficulty.xp }} XP</span>
                      </div>
                      <div class="text-sm text-gray-400">{{ difficulty.description }}</div>
                    </div>
                  </label>
                </div>
              </div>

              <!-- Custom XP (if needed) -->
              <div>
                <label for="customXp" class="block text-sm font-medium mb-2">
                  Custom XP Value
                </label>
                <input
                  id="customXp"
                  v-model.number="form.customXp"
                  type="number"
                  min="1"
                  max="1000"
                  class="w-full px-4 py-3 bg-gray-700 border border-gray-600 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-colors"
                  placeholder="Leave empty to use difficulty XP"
                />
                <div class="text-gray-400 text-sm mt-1">
                  Override the difficulty-based XP value
                </div>
              </div>
            </div>

            <!-- Reminder Settings -->
            <div class="bg-gray-700/50 rounded-lg p-4">
              <h3 class="font-medium mb-3 flex items-center gap-2">
                <BellIcon class="w-5 h-5" />
                Reminder Settings
              </h3>
              
              <div class="space-y-3">
                <label class="flex items-center">
                  <input
                    v-model="form.reminderEnabled"
                    type="checkbox"
                    class="w-4 h-4 text-purple-600 bg-gray-700 border-gray-600 rounded focus:ring-purple-500"
                  />
                  <span class="ml-2">Enable daily reminders</span>
                </label>
                
                <div v-if="form.reminderEnabled" class="ml-6 space-y-2">
                  <div>
                    <label for="reminderTime" class="block text-sm text-gray-400 mb-1">
                      Reminder Time
                    </label>
                    <input
                      id="reminderTime"
                      v-model="form.reminderTime"
                      type="time"
                      class="px-3 py-2 bg-gray-700 border border-gray-600 rounded focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                    />
                  </div>
                </div>
              </div>
            </div>

            <!-- XP Preview -->
            <div class="bg-gradient-to-r from-purple-900/50 to-purple-800/50 rounded-lg p-4 border border-purple-700/50">
              <div class="flex items-center justify-between">
                <div>
                  <h3 class="font-medium">Quest Reward Preview</h3>
                  <p class="text-sm text-gray-300">XP you'll earn for completing this quest</p>
                </div>
                <div class="text-right">
                  <div class="text-2xl font-bold text-yellow-500">+{{ finalXpValue }} XP</div>
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
                <span>{{ isSubmitting ? 'Creating Quest...' : 'Create Quest' }}</span>
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
            <h3 class="font-medium text-green-400">Quest Created Successfully!</h3>
            <p class="text-sm text-green-300">Your new habit has been added to your daily quests.</p>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, reactive } from 'vue'

// Icons (you can replace these with your preferred icon library)
const ArrowLeftIcon = { template: '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m12 19-7-7 7-7"/><path d="M19 12H5"/></svg>' }
const BellIcon = { template: '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M6 8a6 6 0 0 1 12 0c0 7 3 9 3 9H3s3-2 3-9"/><path d="M10.3 21a1.94 1.94 0 0 0 3.4 0"/></svg>' }
const CheckCircleIcon = { template: '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/><path d="m9 11 3 3L22 4"/></svg>' }
const LoadingSpinner = { template: '<svg class="animate-spin" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"/><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"/></svg>' }

// Types
interface HabitForm {
  title: string
  description: string
  frequency: 'Daily' | 'Weekly'
  category: string
  difficulty: 'Easy' | 'Medium' | 'Hard' | 'Expert'
  customXp: number | null
  reminderEnabled: boolean
  reminderTime: string
}

interface DifficultyLevel {
  value: 'Easy' | 'Medium' | 'Hard' | 'Expert'
  label: string
  emoji: string
  xp: number
  description: string
}

// State
const form = reactive<HabitForm>({
  title: '',
  description: '',
  frequency: 'Daily',
  category: '',
  difficulty: 'Easy',
  customXp: null,
  reminderEnabled: false,
  reminderTime: '09:00'
})

const errors = reactive({
  title: ''
})

const isSubmitting = ref(false)
const showSuccess = ref(false)

// Difficulty levels configuration
const difficultyLevels: DifficultyLevel[] = [
  {
    value: 'Easy',
    label: 'Novice',
    emoji: '🟢',
    xp: 10,
    description: 'Simple habits that take 5-15 minutes'
  },
  {
    value: 'Medium',
    label: 'Adventurer',
    emoji: '🟡',
    xp: 25,
    description: 'Moderate habits that take 15-30 minutes'
  },
  {
    value: 'Hard',
    label: 'Warrior',
    emoji: '🟠',
    xp: 50,
    description: 'Challenging habits that take 30-60 minutes'
  },
  {
    value: 'Expert',
    label: 'Legend',
    emoji: '🔴',
    xp: 100,
    description: 'Intensive habits that take 1+ hours'
  }
]

// Computed properties
const finalXpValue = computed(() => {
  if (form.customXp && form.customXp > 0) {
    return form.customXp
  }
  const difficulty = difficultyLevels.find(d => d.value === form.difficulty)
  return difficulty?.xp || 10
})

const isFormValid = computed(() => {
  return form.title.trim().length > 0 && 
         form.frequency && 
         form.difficulty &&
         !errors.title
})

// Methods
const validateTitle = () => {
  errors.title = ''
  if (!form.title.trim()) {
    errors.title = 'Quest title is required'
  } else if (form.title.length < 3) {
    errors.title = 'Quest title must be at least 3 characters'
  } else if (form.title.length > 100) {
    errors.title = 'Quest title must be less than 100 characters'
  }
}

const submitHabit = async () => {
  validateTitle()
  
  if (!isFormValid.value) {
    return
  }

  isSubmitting.value = true
  
  try {
    const habitData = {
      title: form.title.trim(),
      description: form.description.trim(),
      xpValue: finalXpValue.value,
      frequency: form.frequency,
      category: form.category || 'Other',
      difficulty: form.difficulty,
      reminderEnabled: form.reminderEnabled,
      reminderTime: form.reminderEnabled ? form.reminderTime : null
    }

    // In a real app, this would be an API call
    // const response = await fetch('/api/habits', {
    //   method: 'POST',
    //   headers: {
    //     'Content-Type': 'application/json',
    //     'Authorization': `Bearer ${authToken}`
    //   },
    //   body: JSON.stringify(habitData)
    // })
    
    // if (!response.ok) {
    //   throw new Error('Failed to create habit')
    // }

    // Mock API call
    await new Promise(resolve => setTimeout(resolve, 1500))
    
    // Show success message
    showSuccess.value = true
    
    // Reset form after a delay
    setTimeout(() => {
      resetForm()
      showSuccess.value = false
    }, 3000)
    
  } catch (error) {
    console.error('Error creating habit:', error)
    alert('Failed to create quest. Please try again.')
  } finally {
    isSubmitting.value = false
  }
}

const resetForm = () => {
  Object.assign(form, {
    title: '',
    description: '',
    frequency: 'Daily',
    category: '',
    difficulty: 'Easy',
    customXp: null,
    reminderEnabled: false,
    reminderTime: '09:00'
  })
  Object.assign(errors, {
    title: ''
  })
}

const goBack = () => {
  // In a real app, this would use Vue Router
  // router.go(-1) or router.push('/dashboard')
  window.history.back()
}

// Watch for title changes to validate in real-time
import { watch } from 'vue'
watch(() => form.title, validateTitle)
</script>