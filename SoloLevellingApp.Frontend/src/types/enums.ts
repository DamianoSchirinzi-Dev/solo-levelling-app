export const HabitFrequency = {
  Daily: 0,
  Weekly: 1,
  Monthly: 2
} as const;

export const Difficulty = {
  Easy: 0,
  Medium: 1,
  Hard: 2,
  Expert: 3
} as const;

export type HabitFrequency = typeof HabitFrequency[keyof typeof HabitFrequency];
export type Difficulty = typeof Difficulty[keyof typeof Difficulty];