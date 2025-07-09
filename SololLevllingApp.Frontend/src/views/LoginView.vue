<template>
  <div
    class="min-h-screen bg-gray-900 text-white flex items-center justify-center p-6"
  >
    <div class="w-full max-w-md bg-gray-800 rounded-xl shadow-lg p-8">
      <!-- Header -->
      <div class="text-center mb-8">
        <h1 class="text-3xl font-bold text-white mb-2">Welcome Back</h1>
        <p class="text-gray-400">Sign in to your account</p>
      </div>

      <!-- Login Form -->
      <form @submit.prevent="login" class="space-y-6">
      <!-- Identifier Field (Username or Email) -->
        <div>
          <label
            for="identifier"
            class="block text-sm font-medium text-gray-300 mb-2"
          >
            Username or Email Address
          </label>
          <div class="relative">
            <input
              id="identifier"
              v-model="identifier"
              type="text"
              required
              placeholder="Enter your username or email"
              class="w-full px-4 py-3 border border-gray-600 bg-gray-700 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-purple-500 transition duration-200 pl-11 text-white placeholder-gray-400"
              :class="{
                'border-red-500 focus:ring-red-500 focus:border-red-500':
                  identifierError,
              }"
            />
            <MailIcon
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
            />
          </div>
          <p v-if="identifierError" class="mt-1 text-sm text-red-400">
            {{ identifierError }}
          </p>
        </div>

        <!-- Password Field -->
        <div>
          <label
            for="password"
            class="block text-sm font-medium text-gray-300 mb-2"
          >
            Password
          </label>
          <div class="relative">
            <input
              id="password"
              v-model="password"
              :type="showPassword ? 'text' : 'password'"
              required
              placeholder="Enter your password"
              class="w-full px-4 py-3 border border-gray-600 bg-gray-700 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-purple-500 transition duration-200 pl-11 pr-11 text-white placeholder-gray-400"
              :class="{
                'border-red-500 focus:ring-red-500 focus:border-red-500':
                  passwordError,
              }"
            />
            <LockIcon
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
            />
            <button
              type="button"
              @click="togglePasswordVisibility"
              class="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-300 focus:outline-none"
            >
              <EyeIcon v-if="!showPassword" class="h-5 w-5" />
              <EyeOffIcon v-else class="h-5 w-5" />
            </button>
          </div>
          <p v-if="passwordError" class="mt-1 text-sm text-red-400">
            {{ passwordError }}
          </p>
        </div>

        <!-- Remember Me -->
        <div class="flex items-center justify-between">
          <div class="flex items-center">
            <input
              id="remember-me"
              v-model="rememberMe"
              type="checkbox"
              class="h-4 w-4 text-purple-600 focus:ring-purple-500 border-gray-600 bg-gray-800 rounded"
            />
            <label for="remember-me" class="ml-2 block text-sm text-gray-300">
              Remember me
            </label>
          </div>
          <a
            href="#"
            class="text-sm text-purple-400 hover:text-purple-300 font-medium"
          >
            Forgot password?
          </a>
        </div>

        <!-- Error Message -->
        <div
          v-if="error"
          class="bg-red-600 bg-opacity-20 border border-red-500 rounded-lg p-4"
        >
          <div class="flex items-center">
            <AlertCircleIcon class="h-5 w-5 text-red-400 mr-2" />
            <p class="text-sm text-red-300">{{ error }}</p>
          </div>
        </div>

        <!-- Submit Button -->
        <button
          type="submit"
          :disabled="isLoading || !isFormValid"
          class="w-full bg-gradient-to-r from-purple-600 to-purple-500 text-white py-3 px-4 rounded-lg font-medium hover:from-purple-500 hover:to-purple-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:ring-offset-2 transition duration-200 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
        >
          <LoaderIcon v-if="isLoading" class="animate-spin h-5 w-5 mr-2" />
          {{ isLoading ? "Signing in..." : "Sign In" }}
        </button>
      </form>

      <!-- Divider -->
      <div class="mt-8 mb-6">
        <div class="relative">
          <div class="absolute inset-0 flex items-center">
            <div class="w-full border-t border-gray-700"></div>
          </div>
          <div class="relative flex justify-center text-sm">
            <span class="px-2 bg-gray-800 text-gray-400">Or continue with</span>
          </div>
        </div>
      </div>

      <!-- Social Login Buttons -->
      <div class="grid grid-cols-2 gap-3">
        <button
          type="button"
          class="w-full inline-flex justify-center py-2.5 px-4 border border-gray-600 rounded-lg shadow-sm bg-gray-700 text-sm font-medium text-gray-300 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:ring-offset-2 transition duration-200"
        >
          <svg class="h-5 w-5" viewBox="0 0 24 24">
            <path
              fill="currentColor"
              d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"
            />
            <path
              fill="currentColor"
              d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"
            />
            <path
              fill="currentColor"
              d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"
            />
            <path
              fill="currentColor"
              d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"
            />
          </svg>
          <span class="ml-2">Google</span>
        </button>
        <button
          type="button"
          class="w-full inline-flex justify-center py-2.5 px-4 border border-gray-600 rounded-lg shadow-sm bg-gray-700 text-sm font-medium text-gray-300 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:ring-offset-2 transition duration-200"
        >
          <svg class="h-5 w-5" fill="currentColor" viewBox="0 0 24 24">
            <path
              d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z"
            />
          </svg>
          <span class="ml-2">Facebook</span>
        </button>
      </div>

      <!-- Sign Up Link -->
      <div class="mt-8 text-center">
        <p class="text-sm text-gray-400">
          Don't have an account?
          <router-link
            to="/register"
            class="font-medium text-purple-400 hover:text-purple-300"
          >
            Sign up for free
          </router-link>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted } from "vue";
import { useUserStore } from '@/store/userStore'
import { useRouter } from "vue-router";
import {
  MailIcon,
  LockIcon,
  EyeIcon,
  EyeOffIcon,
  AlertCircleIcon,
  LoaderIcon,
} from "lucide-vue-next";

// Reactive data
const identifier = ref("");
const password = ref("");
const error = ref("");
const isLoading = ref(false);
const showPassword = ref(false);
const rememberMe = ref(false);

// User store
const userStore = useUserStore();

// Router
const router = useRouter();

// Validation
const identifierError = ref("");
const passwordError = ref("");

// Computed properties
const isFormValid = computed(() => {
  return (
    identifier.value.length > 0 &&
    password.value.length > 0 &&
    !identifierError.value &&
    !passwordError.value
  );
});

// Lifecycle hooks
onMounted(() => {
  userStore.loadTokenFromStorage()
  if (userStore.token) {
    router.push('/dashboard')
  }
})

// Methods
const validateIdentifier = () => {
  if (!identifier.value) {
    identifierError.value = "Username or email is required";
  } else {
    identifierError.value = "";
  }
};

const validatePassword = () => {
  if (!password.value) {
    passwordError.value = "Password is required";
  } else if (password.value.length < 6) {
    passwordError.value = "Password must be at least 6 characters";
  } else {
    passwordError.value = "";
  }
};

const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value;
};

const login = async () => {
  console.log("Login function called");

  // Validate form
  validateIdentifier();
  validatePassword();

  if (!isFormValid.value) {
    return;
  }

  isLoading.value = true;
  error.value = "";

  try {
    // API call
    await userStore.login(identifier.value, password.value);

    console.log("Login successful", {
      indentifier: identifier.value,
      rememberMe: rememberMe.value,
    });

    // Redirect to dashboard
    router.push("/dashboard");
  } catch (err: any) {
    error.value =
      err.message ||
      "Login failed. Please check your credentials and try again.";
  } finally {
    isLoading.value = false;
  }
};

// Watch for input changes to clear errors
watch(identifier, () => {
  if (identifierError.value) validateIdentifier();
});

watch(password, () => {
  if (passwordError.value) validatePassword();
});
</script>
