using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games without shuffling the cards 
    public class XGamesNoShuffle : Achievement
    {
        private int _gamesCountCondition;
        private int _gamesCount;

        public XGamesNoShuffle(string text, int x) : base(text)
        {
            _gamesCountCondition = x;
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.ShufflesUsed == 0)
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
            var gamesCountCategory = DifficultyUtils.CalculateGameCountCategory(_gamesCountCondition);

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
    }
}