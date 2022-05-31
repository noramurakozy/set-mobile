using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games without shuffling the cards 
    public class XGamesNoShuffle : Achievement
    {
        public int gamesCountCondition;
        public int gamesCount;

        public XGamesNoShuffle(CreationType creationType, string text, int x) : base(text, creationType)
        {
            gamesCountCondition = x;
            UpdateType = UpdateType.EndOfGame;
        }

        public override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.ShufflesUsed == 0)
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

            switch (gamesCountCategory)
            {
                case ParameterCountCategory.High:
                    Difficulty = Difficulty.Hard;
                    break;
                case ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.Low:
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