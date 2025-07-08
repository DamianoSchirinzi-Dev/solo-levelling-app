<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center p-4">
    <div class="max-w-md w-full bg-white rounded-xl shadow-lg p-8">
      <!-- Header -->
      <div class="text-center mb-8">
        <h1 class="text-3xl font-bold text-gray-900 mb-2">Create Account</h1>
        <p class="text-gray-600">Join us and get started today</p>
      </div>

      <!-- Registration Form -->
      <form @submit.prevent="register" class="space-y-6">
        <!-- Full Name Field -->
        <div>
          <label for="fullName" class="block text-sm font-medium text-gray-700 mb-2">
            Full Name
          </label>
          <div class="relative">
            <input
              id="fullName"
              v-model="fullName"
              type="text"
              required
              placeholder="Enter your full name"
              class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition duration-200 pl-11"
              :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': fullNameError }"
            />
            <UserIcon class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
          </div>
          <p v-if="fullNameError" class="mt-1 text-sm text-red-600">{{ fullNameError }}</p>
        </div>

        <!-- Email Field -->
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700 mb-2">
            Email Address
          </label>
          <div class="relative">
            <input
              id="email"
              v-model="email"
              type="email"
              required
              placeholder="Enter your email"
              class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition duration-200 pl-11"
              :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': emailError }"
            />
            <MailIcon class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
          </div>
          <p v-if="emailError" class="mt-1 text-sm text-red-600">{{ emailError }}</p>
        </div>

        <!-- Password Field -->
        <div>
          <label for="password" class="block text-sm font-medium text-gray-700 mb-2">
            Password
          </label>
          <div class="relative">
            <input
              id="password"
              v-model="password"
              :type="showPassword ? 'text' : 'password'"
              required
              placeholder="Create a password"
              class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition duration-200 pl-11 pr-11"
              :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': passwordError }"
            />
            <LockIcon class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
            <button
              type="button"
              @click="togglePasswordVisibility"
              class="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600 focus:outline-none"
            >
              <EyeIcon v-if="!showPassword" class="h-5 w-5" />
              <EyeOffIcon v-else class="h-5 w-5" />
            </button>
          </div>
          <p v-if="passwordError" class="mt-1 text-sm text-red-600">{{ passwordError }}</p>
          
          <!-- Password Strength Indicator -->
          <div v-if="password" class="mt-2">
            <div class="flex items-center space-x-1">
              <div class="flex-1 h-2 bg-gray-200 rounded-full overflow-hidden">
                <div 
                  class="h-full transition-all duration-300"
                  :class="passwordStrengthColor"
                  :style="{ width: `${passwordStrengthPercentage}%` }"
                ></div>
              </div>
              <span class="text-xs font-medium" :class="passwordStrengthTextColor">
                {{ passwordStrengthText }}
              </span>
            </div>
          </div>
        </div>

        <!-- Confirm Password Field -->
        <div>
          <label for="confirmPassword" class="block text-sm font-medium text-gray-700 mb-2">
            Confirm Password
          </label>
          <div class="relative">
            <input
              id="confirmPassword"
              v-model="confirmPassword"
              :type="showConfirmPassword ? 'text' : 'password'"
              required
              placeholder="Confirm your password"
              class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition duration-200 pl-11 pr-11"
              :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': confirmPasswordError }"
            />
            <LockIcon class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
            <button
              type="button"
              @click="toggleConfirmPasswordVisibility"
              class="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600 focus:outline-none"
            >
              <EyeIcon v-if="!showConfirmPassword" class="h-5 w-5" />
              <EyeOffIcon v-else class="h-5 w-5" />
            </button>
          </div>
          <p v-if="confirmPasswordError" class="mt-1 text-sm text-red-600">{{ confirmPasswordError }}</p>
        </div>

        <!-- Terms and Conditions -->
        <div class="flex items-start">
          <input
            id="terms"
            v-model="acceptTerms"
            type="checkbox"
            class="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded mt-1"
          />
          <label for="terms" class="ml-2 block text-sm text-gray-700">
            I agree to the 
            <a href="#" class="text-blue-600 hover:text-blue-500 font-medium">Terms of Service</a>
            and 
            <a href="#" class="text-blue-600 hover:text-blue-500 font-medium">Privacy Policy</a>
          </label>
        </div>
        <p v-if="termsError" class="text-sm text-red-600">{{ termsError }}</p>

        <!-- Error Message -->
        <div v-if="error" class="bg-red-50 border border-red-200 rounded-lg p-4">
          <div class="flex items-center">
            <AlertCircleIcon class="h-5 w-5 text-red-400 mr-2" />
            <p class="text-sm text-red-800">{{ error }}</p>
          </div>
        </div>

        <!-- Success Message -->
        <div v-if="success" class="bg-green-50 border border-green-200 rounded-lg p-4">
          <div class="flex items-center">
            <CheckCircleIcon class="h-5 w-5 text-green-400 mr-2" />
            <p class="text-sm text-green-800">{{ success }}</p>
          </div>
        </div>

        <!-- Submit Button -->
        <button
          type="submit"
          :disabled="isLoading || !isFormValid"
          class="w-full bg-blue-600 text-white py-3 px-4 rounded-lg font-medium hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition duration-200 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
        >
          <LoaderIcon v-if="isLoading" class="animate-spin h-5 w-5 mr-2" />
          {{ isLoading ? 'Creating Account...' : 'Create Account' }}
        </button>
      </form>

      <!-- Divider -->
      <div class="mt-8 mb-6">
        <div class="relative">
          <div class="absolute inset-0 flex items-center">
            <div class="w-full border-t border-gray-300"></div>
          </div>
          <div class="relative flex justify-center text-sm">
            <span class="px-2 bg-white text-gray-500">Or sign up with</span>
          </div>
        </div>
      </div>

      <!-- Social Registration Buttons -->
      <div class="grid grid-cols-2 gap-3">
        <button
          type="button"
          class="w-full inline-flex justify-center py-2.5 px-4 border border-gray-300 rounded-lg shadow-sm bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition duration-200"
        >
          <svg class="h-5 w-5" viewBox="0 0 24 24">
            <path fill="currentColor" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"/>
            <path fill="currentColor" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"/>
            <path fill="currentColor" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"/>
            <path fill="currentColor" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"/>
          </svg>
          <span class="ml-2">Google</span>
        </button>
        <button
          type="button"
          class="w-full inline-flex justify-center py-2.5 px-4 border border-gray-300 rounded-lg shadow-sm bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition duration-200"
        >
          <svg class="h-5 w-5" fill="currentColor" viewBox="0 0 24 24">
            <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z"/>
          </svg>
          <span class="ml-2">Facebook</span>
        </button>
      </div>

      <!-- Sign In Link -->
      <div class="mt-8 text-center">
        <p class="text-sm text-gray-600">
          Already have an account?
          <router-link to="/login" class="font-medium text-blue-600 hover:text-blue-500">
            Sign in here
          </router-link>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useRouter } from 'vue-router'
import { 
  UserIcon, 
  MailIcon, 
  LockIcon, 
  EyeIcon, 
  EyeOffIcon, 
  AlertCircleIcon, 
  CheckCircleIcon,
  LoaderIcon 
} from 'lucide-vue-next'

// Reactive data
const fullName = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const acceptTerms = ref(false)
const error = ref('')
const success = ref('')
const isLoading = ref(false)
const showPassword = ref(false)
const showConfirmPassword = ref(false)

// Router
const router = useRouter()

// Validation errors
const fullNameError = ref('')
const emailError = ref('')
const passwordError = ref('')
const confirmPasswordError = ref('')
const termsError = ref('')

// Password strength computation
const passwordStrength = computed(() => {
  if (!password.value) return 0
  
  let strength = 0
  const checks = [
    password.value.length >= 8,
    /[a-z]/.test(password.value),
    /[A-Z]/.test(password.value),
    /\d/.test(password.value),
    /[!@#$%^&*(),.?":{}|<>]/.test(password.value)
  ]
  
  strength = checks.filter(Boolean).length
  return strength
})

const passwordStrengthPercentage = computed(() => {
  return (passwordStrength.value / 5) * 100
})

const passwordStrengthText = computed(() => {
  const strength = passwordStrength.value
  if (strength <= 1) return 'Weak'
  if (strength <= 2) return 'Fair'
  if (strength <= 3) return 'Good'
  if (strength <= 4) return 'Strong'
  return 'Very Strong'
})

const passwordStrengthColor = computed(() => {
  const strength = passwordStrength.value
  if (strength <= 1) return 'bg-red-500'
  if (strength <= 2) return 'bg-orange-500'
  if (strength <= 3) return 'bg-yellow-500'
  if (strength <= 4) return 'bg-blue-500'
  return 'bg-green-500'
})

const passwordStrengthTextColor = computed(() => {
  const strength = passwordStrength.value
  if (strength <= 1) return 'text-red-600'
  if (strength <= 2) return 'text-orange-600'
  if (strength <= 3) return 'text-yellow-600'
  if (strength <= 4) return 'text-blue-600'
  return 'text-green-600'
})

// Form validation
const isFormValid = computed(() => {
  return (
    fullName.value.length > 0 &&
    email.value.length > 0 &&
    password.value.length > 0 &&
    confirmPassword.value.length > 0 &&
    acceptTerms.value &&
    !fullNameError.value &&
    !emailError.value &&
    !passwordError.value &&
    !confirmPasswordError.value &&
    !termsError.value
  )
})

// Validation methods
const validateFullName = () => {
  if (!fullName.value.trim()) {
    fullNameError.value = 'Full name is required'
  } else if (fullName.value.trim().length < 2) {
    fullNameError.value = 'Full name must be at least 2 characters'
  } else {
    fullNameError.value = ''
  }
}

const validateEmail = () => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!email.value) {
    emailError.value = 'Email is required'
  } else if (!emailRegex.test(email.value)) {
    emailError.value = 'Please enter a valid email address'
  } else {
    emailError.value = ''
  }
}

const validatePassword = () => {
  if (!password.value) {
    passwordError.value = 'Password is required'
  } else if (password.value.length < 8) {
    passwordError.value = 'Password must be at least 8 characters'
  } else {
    passwordError.value = ''
  }
}

const validateConfirmPassword = () => {
  if (!confirmPassword.value) {
    confirmPasswordError.value = 'Please confirm your password'
  } else if (password.value !== confirmPassword.value) {
    confirmPasswordError.value = 'Passwords do not match'
  } else {
    confirmPasswordError.value = ''
  }
}

const validateTerms = () => {
  if (!acceptTerms.value) {
    termsError.value = 'You must accept the terms and conditions'
  } else {
    termsError.value = ''
  }
}

// Toggle password visibility
const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value
}

const toggleConfirmPasswordVisibility = () => {
  showConfirmPassword.value = !showConfirmPassword.value
}

// Registration handler
const register = async () => {
  console.log('Register function called')
  
  // Validate all fields
  validateFullName()
  validateEmail()
  validatePassword()
  validateConfirmPassword()
  validateTerms()
  
  if (!isFormValid.value) {
    return
  }
  
  isLoading.value = true
  error.value = ''
  success.value = ''
  
  try {
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 2000))
    
    // Uncomment when you have your user store ready
    // await userStore.register({
    //   fullName: fullName.value,
    //   email: email.value,
    //   password: password.value
    // })
    
    // For now, just simulate success
    console.log('Registration successful', {
      fullName: fullName.value,
      email: email.value
    })
    
    success.value = 'Account created successfully! Redirecting to login...'
    
    // Redirect to login after success
    setTimeout(() => {
      router.push('/login')
    }, 2000)
    
  } catch (err: any) {
    error.value = err.message || 'Registration failed. Please try again.'
  } finally {
    isLoading.value = false
  }
}

// Watch for input changes to clear errors
watch(fullName, validateFullName)
watch(email, validateEmail)
watch(password, validatePassword)
watch(confirmPassword, validateConfirmPassword)
watch(acceptTerms, validateTerms)
</script>