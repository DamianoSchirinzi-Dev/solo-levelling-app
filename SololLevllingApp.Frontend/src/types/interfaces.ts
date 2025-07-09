export interface UserProfile {
  id: number;
  username: string;
  email: string;
  title: string;
  level: number;
  currentXP: number;
  XPToNextLevel: number;
  streak: number;
  lastLogin: string;
}

export type HabitFrequency = 'Daily' | 'Weekly' | 'Monthly' | 'Custom';

export interface CreateHabitDto {
  name: string;
  description: string;
  frequency: HabitFrequency;
}

export interface HabitCompletionDto {
  id: number;
  completedAt: string; // ISO string representation of DateTime
  habitId: number;
  habit?: Habit; // Optional navigation property
  userId: number;
}

export interface Habit {
  id: number;
  name: string;
  description: string;
  frequency: HabitFrequency;
  createdAt?: string;
  updatedAt?: string;
  completed?: boolean; // Track if habit is completed today
  xpValue?: number; 
}

export interface UpdateHabitDto {
  name?: string;
  description?: string;
  frequency?: HabitFrequency;
}