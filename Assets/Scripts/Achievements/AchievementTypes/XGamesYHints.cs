using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games using maximum Y hints 
    public class XGamesYHints : Achievement
    {
        public int gamesCount;

        public int gamesCountCondition;
        public int hintsCountCondition;
        public XGamesYHints(CreationType creationType, string text, int x, int y) : base(text, creationType)
        {
            gamesCountCondition = x;
            hintsCountCondition = y;
            UpdateType = UpdateType.EndOfGame;
        }

        public override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.HintsUsed <= hintsCountCondition)
            {
                gamesCount++;
            }

            if (gamesCount >= gamesCountCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var gamesCountCategory = DifficultyUtils.CalculateGameCountCategory(gamesCountCondition);
            var hintCountCategory = DifficultyUtils.CalculateHintCountCategory(hintsCountCondition);

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
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                default:
                    Difficulty = Difficulty.Medium;
                    break;
            }
        }
    }
}