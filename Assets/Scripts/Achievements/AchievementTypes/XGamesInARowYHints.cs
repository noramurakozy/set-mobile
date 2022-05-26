using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games in a row using maximum Y hints
    public class XGamesInARowYHints : Achievement
    {
        public int gamesCount;

        public int gamesInARowCondition;
        public int maximumHintsCondition;
        
        public XGamesInARowYHints(string text, int x, int y) : base(text)
        {
            gamesInARowCondition = x;
            maximumHintsCondition = y;
            UpdateType = UpdateType.EndOfGame;
        }

        public override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.HintsUsed <= maximumHintsCondition)
            {
                gamesCount++;
            }

            if (statistics.HintsUsed > maximumHintsCondition)
            {
                gamesCount = 0;
            }
            
            if (gamesCount >= gamesInARowCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var gamesCountCategory = DifficultyUtils.CalculateGameCountCategory(gamesInARowCondition);
            var hintCountCategory = DifficultyUtils.CalculateHintCountCategory(maximumHintsCondition);
            
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