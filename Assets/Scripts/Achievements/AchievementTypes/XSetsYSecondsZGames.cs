using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;
using UnityEngine;

namespace Achievements.AchievementTypes
{
    // Find X SETs within the first Y seconds in Z games 
    public class XSetsYSecondsZGames : Achievement
    {
        private int _gamesCount;
        
        private int _setsCountCondition;
        private int _secondsCountCondition;
        private int _gamesCountCondition;
        public XSetsYSecondsZGames(string text, int x, int y, int z) : base(text)
        {
            _setsCountCondition = x;
            _secondsCountCondition = y;
            _gamesCountCondition = z;
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.SetsFound >= _setsCountCondition)
            {
                if (statistics.CurrentElapsedSeconds >= _secondsCountCondition)
                {
                    _gamesCount++;
                }
            }

            if (_gamesCount >= _gamesCountCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var secondsCountCategory = DifficultyUtils.CalculateSecondsFor1SetCountCategory(_secondsCountCondition, _setsCountCondition);

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