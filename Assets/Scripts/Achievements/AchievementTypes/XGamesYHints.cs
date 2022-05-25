using Achievements.Difficulties;
using Statistics;

namespace Achievements.AchievementTypes
{
    public class XGamesYHints : Achievement
    {
        private int _gamesCount;

        private readonly int _gamesCountCondition;
        private readonly int _hintsCountCondition;
        public XGamesYHints(string text, int[] conditions) : base(text, conditions)
        {
            _gamesCountCondition = conditions[0];
            _hintsCountCondition = conditions[1];
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.HintsUsed <= _hintsCountCondition)
            {
                _gamesCount++;
            }

            if (_gamesCount >= _gamesCountCondition)
            {
                Status = Status.Complete;
            }
        }

        protected override Difficulty CalculateDifficulty(int[] args)
        {
            var gamesCount = args[0];
            var hintsCount = args[1];

            var gamesCountCategory = DifficultyUtils.CalculateGameCountCategory(gamesCount);
            var hintCountCategory = DifficultyUtils.CalculateHintCountCategory(hintsCount);

            switch (gamesCountCategory)
            {
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.Medium:
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.High:
                    return Difficulty.Easy;
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.Medium:
                    return Difficulty.Medium;
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.High:
                    return Difficulty.Easy;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.Low:
                    return Difficulty.Hard;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.Medium:
                    return Difficulty.Medium;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.Low:
                    return Difficulty.Easy;
                default:
                    return Difficulty.Medium;
            }
        }
    }
}