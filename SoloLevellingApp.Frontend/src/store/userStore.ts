import { useRouter } from "vue-router";
import { defineStore } from "pinia";
import axios from "axios";
import type { UserProfile } from "../types/interfaces";

export const useUserStore = defineStore("user", {
  state: () => ({
    user: null as null | { email: string },
    token: null as null | string,
    userProfile: null as null | UserProfile,
  }),

  actions: {
    async register(username: string, email: string, password: string) {
      try {
        console.log("Registering user:", { username, email, password });

        const response = await axios.post(
          `${import.meta.env.VITE_API_BASE_URL}/api/auth/register`,
          {
            username,
            email,
            password,
          }
        );

        console.log("Registration response:", response.data);

        if (response.data?.token) {
          this.token = response.data.token;
          this.user = { email };

          if (this.token) {
            localStorage.setItem("token", this.token);
            axios.defaults.headers.common[
              "Authorization"
            ] = `Bearer ${this.token}`;
          }
        }
      } catch (error: any) {
        throw new Error(
          error.response?.data?.message ||
            "Registration failed. Please try again."
        );
      }
    },

    async login(identifier: string, password: string) {
      try {
        const response = await axios.post(
          `${import.meta.env.VITE_API_BASE_URL}/api/auth/login`,
          {
            identifier,
            password,
          }
        );

        this.token = response.data.token;
        this.user = { email: identifier };

        // Save token for persistence
        if (this.token) {
          localStorage.setItem("token", this.token);
          axios.defaults.headers.common[
            "Authorization"
          ] = `Bearer ${this.token}`;
        }
      } catch (error: any) {
        throw new Error(
          error.response?.data?.message ||
            "Invalid credentials or server error."
        );
      }
    },

    async fetchUserProfile() {
      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/api/user/profile`
        );

        this.userProfile = {
          id: response.data.id,
          username: response.data.username,
          email: response.data.email,
          title: response.data.title,
          level: response.data.level,
          currentXP: response.data.xp, // map correctly
          XPToNextLevel: response.data.xPtoNextLevel, // map casing mismatch
          streak: response.data.streak,
          lastLogin: response.data.lastLogin,
        };
      } catch (error: any) {
        console.error("Failed to fetch profile:", error);
        throw new Error("Unable to fetch user profile.");
      }
    },

    async updateUserProfile(profileData: UserProfile) {
      try {
        const response = await axios.put(
          `${import.meta.env.VITE_API_BASE_URL}/api/user/profile`,
          profileData
        );

        this.userProfile = response.data;
      } catch (error: any) {
        console.error("Failed to update profile:", error);
        throw new Error("Unable to update user profile.");
      }
    },

    logout() {
      this.user = null;
      this.token = null;
      localStorage.removeItem("token");
      delete axios.defaults.headers.common["Authorization"];
    },

    loadTokenFromStorage() {
      const token = localStorage.getItem("token");
      if (token) {
        this.token = token;
        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
      }
    },
  },
});
