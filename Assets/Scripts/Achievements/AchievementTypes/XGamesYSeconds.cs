using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games in Y minutes and Z seconds 
    public class XGamesYSeconds : Achievement
    {
        public int gamesCount;

        public int gamesCountCondition;
        // includes the converted minutes too
        public int secondsCountCondition;
        public XGamesYSeconds(CreationType creationType, string text, int x, int y, int z) : base(text, creationType)
        {
            gamesCountCondition = x;
            secondsCountCondition = y*60+z;
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

        public override float GetProgressValue()
        {
            return (float)gamesCount / gamesCountCondition;
        }

        public override string GetProgressText()
        {
            return $"{gamesCount}/{gamesCountCondition}";
        }
    }
}