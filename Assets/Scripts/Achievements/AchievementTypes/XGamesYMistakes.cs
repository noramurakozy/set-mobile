using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games with maximum Y mistakes 
    public class XGamesYMistakes : Achievement
    {
        public int gamesCount;

        public int gamesCountCondition;
        public int mistakesCountCondition;
        
        public XGamesYMistakes(CreationType creationType, string text, int x, int y) : base(text, creationType)
        {
            gamesCountCondition = x;
            mistakesCountCondition = y;
            UpdateType = UpdateType.EndOfGame;
        }

        public override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.MistakesCount <= mistakesCountCondition)
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
            var mistakesCountCategory = DifficultyUtils.CalculateMistakesCountCategory(mistakesCountCondition);
            
            switch (gamesCountCategory)
            {
                case ParameterCountCategory.Low when mistakesCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Low when mistakesCountCategory == ParameterCountCategory.Medium:
                case ParameterCountCategory.Low when mistakesCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.Medium when mistakesCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Medium when mistakesCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.Medium when mistakesCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.High when mistakesCountCategory == ParameterCountCategory.Low:
                    Difficulty = Difficulty.Hard;
                    break;
                case ParameterCountCategory.High when mistakesCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.High when mistakesCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                default:
                    Difficulty = Difficulty.Medium;
                    break;
            }
        }
    }
}