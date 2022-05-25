using Achievements.Difficulties;
using Statistics;

namespace Achievements.AchievementTypes
{
    public class XGamesYHints : Achievement
    {
        public int _gamesCount;

        public readonly int _gamesCountCondition;
        public readonly int _hintsCountCondition;
        public XGamesYHints(string text, int x, int y) : base(text)
        {
            _gamesCountCondition = x;
            _hintsCountCondition = y;
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

        public override void CalculateDifficulty()
        {
            var gamesCount = _gamesCountCondition;
            var hintsCount = _hintsCountCondition;

            var gamesCountCategory = DifficultyUtils.CalculateGameCountCategory(gamesCount);
            var hintCountCategory = DifficultyUtils.CalculateHintCountCategory(hintsCount);

            switch (gamesCountCategory)
            {
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.Medium:
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.Low:
                    Difficulty = Difficulty.Hard;
                    break;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.Low:
                    Difficulty = Difficulty.Easy;
                    break;
                default:
                    Difficulty = Difficulty.Medium;
                    break;
            }
        }
    }
}