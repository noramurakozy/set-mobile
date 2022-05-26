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

        public static ParameterCountCategory CalculateMistakesCountCategory(int mistakesCount)
        {
            return mistakesCount switch
            {
                >= 10 => ParameterCountCategory.High,
                <= 2 => ParameterCountCategory.Low,
                _ => ParameterCountCategory.Medium
            };
        }

        public static ParameterCountCategory CalculateSecondsCountCategory(int secondsCountCondition)
        {
            return secondsCountCondition switch
            {
                >= 15 * 60 => ParameterCountCategory.High,
                <= 5 * 60 => ParameterCountCategory.Low,
                _ => ParameterCountCategory.Medium,
            };
        }

        public static ParameterCountCategory CalculateSetsInARowCountCategory(int setsCountCondition)
        {
            return setsCountCondition switch
            {
                >= 15 => ParameterCountCategory.High,
                <= 3 => ParameterCountCategory.Low,
                _ => ParameterCountCategory.Medium
            };
        }

        public static ParameterCountCategory CalculateSetsInTotalCountCategory(int setsCountCondition)
        {
            return setsCountCondition switch
            {
                >= 30 => ParameterCountCategory.High,
                <= 5 => ParameterCountCategory.Low,
                _ => ParameterCountCategory.Medium
            };
        }
        public static ParameterCountCategory CalculateSetsPropsCountCategory(int paramCount)
        {
            return paramCount switch
            {
                1 => ParameterCountCategory.Low,
                2 => ParameterCountCategory.Medium,
                3 => ParameterCountCategory.Medium,
                4 => ParameterCountCategory.High,
                _ => ParameterCountCategory.Medium
            };
        }

        public static ParameterCountCategory CalculateSecondsFor1SetCountCategory(int secondsCount,
            int setsCountCondition)
        {
            float secondsPerSet = (float)secondsCount / setsCountCondition;

            return secondsPerSet switch
            {
                >= 30 => ParameterCountCategory.High,
                <= 5 => ParameterCountCategory.Low,
                _ => ParameterCountCategory.Medium
            };
        }
    }
}