import { createRouter, createWebHistory } from "vue-router";
import { useUserStore } from '@/store/userStore'
import DashboardView from "@/views/DashboardView.vue";
import HabitFormView from "@/views/HabitFormView.vue";
import LoginView from "@/views/LoginView.vue";
import RegisterView from "@/views/RegisterView.vue";

const routes = [
  { path: "/", redirect: "/dashboard" },
  { 
    path: '/dashboard', 
    component: DashboardView,
    meta: { requiresAuth: true },
  },
  { path: "/create-habit", component: HabitFormView},
  { path: "/login", component: LoginView },
  { path: "/register", component: RegisterView },

  // Add more routes like Dashboard, etc.
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const userStore = useUserStore()
  userStore.loadTokenFromStorage()

  if (to.meta.requiresAuth && !userStore.token) {
    next('/login')
  } else {
    next()
  }
})

export default router;
