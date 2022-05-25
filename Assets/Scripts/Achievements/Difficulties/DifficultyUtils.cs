namespace Achievements.Difficulties
{
    public static class DifficultyUtils
    {
        public static ParameterCountCategory CalculateHintCountCategory(int hintCount)
        {
            return hintCount switch
            {
                <= 2 => ParameterCountCategory.Low,
                >= 20 => ParameterCountCategory.High,
                _ => ParameterCountCategory.Medium
            };
        }

        public static ParameterCountCategory CalculateGameCountCategory(int gameCount)
        {
            return gameCount switch
            {
                >= 20 => ParameterCountCategory.High,
                <= 10 => ParameterCountCategory.Low,
                _ => ParameterCountCategory.Medium
            };
        }
    }
}