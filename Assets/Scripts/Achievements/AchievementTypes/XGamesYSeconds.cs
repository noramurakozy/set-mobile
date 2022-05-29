using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games in Y seconds 
    public class XGamesYSeconds : Achievement
    {
        public int gamesCount;

        public int gamesCountCondition;
        public int secondsCountCondition;
        public XGamesYSeconds(CreationType creationType, string text, int x, int y) : base(text, creationType)
        {
            gamesCountCondition = x;
            secondsCountCondition = y;
            UpdateType = UpdateType.EndOfGame;
        }

        public override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.DurationInSeconds <= secondsCountCondition)
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
            var secondsCountCategory = DifficultyUtils.CalculateSecondsCountCategory(secondsCountCondition);

            switch (secondsCountCategory)
            {
                case ParameterCountCategory.Low:
                    Difficulty = Difficulty.Hard;
                    break;
                case ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                default:
                    Difficulty = Difficulty.Medium;
                    break;
            }
        }
    }
}