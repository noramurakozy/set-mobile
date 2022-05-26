﻿using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games in Y seconds 
    public class XGamesYSeconds : Achievement
    {
        private int _gamesCount;
        
        private int _gamesCountCondition;
        private int _secondsCountCondition;
        public XGamesYSeconds(string text, int x, int y) : base(text)
        {
            _gamesCountCondition = x;
            _secondsCountCondition = y;
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.DurationInSeconds <= _secondsCountCondition)
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
            var secondsCountCategory = DifficultyUtils.CalculateSecondsCountCategory(_secondsCountCondition);

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