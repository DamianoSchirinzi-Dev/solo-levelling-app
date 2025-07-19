import { HabitFrequency, Difficulty } from "../types/enums";

export const useEnumLabels = () => {
  const frequencyLabels = {
    [HabitFrequency.Daily]: "Daily",
    [HabitFrequency.Weekly]: "Weekly",
    [HabitFrequency.Monthly]: "Monthly",
  } as const;

  const difficultyLabels = {
    [Difficulty.Easy]: "Easy",
    [Difficulty.Medium]: "Medium", 
    [Difficulty.Hard]: "Hard",
    [Difficulty.Expert]: "Expert",
  } as const;

  const getFrequencyLabel = (frequency: number): string => {
    return frequencyLabels[frequency as HabitFrequency] || "Unknown";
  };

  const getDifficultyLabel = (difficulty: number): string => {
    return difficultyLabels[difficulty as Difficulty] || "Unknown";
  };

  return {
    frequencyLabels,
    difficultyLabels,
    getFrequencyLabel,
    getDifficultyLabel,
  };
};