using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;
using UnityEngine;

namespace Achievements.AchievementTypes
{
    // Find X SETs within the first Y seconds in Z games 
    public class XSetsYSecondsZGames : Achievement
    {
        public int gamesCount;

        public int setsCountCondition;
        public int secondsCountCondition;
        public int gamesCountCondition;
        public XSetsYSecondsZGames(CreationType creationType, string text, int x, int y, int z) : base(text, creationType)
        {
            setsCountCondition = x;
            secondsCountCondition = y;
            gamesCountCondition = z;
            UpdateType = UpdateType.DuringGame;
        }

        public override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.SetsFound >= setsCountCondition)
            {
                if (statistics.CurrentElapsedSeconds <= secondsCountCondition)
                {
                    gamesCount++;
                }
            }

            if (gamesCount >= gamesCountCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var secondsCountCategory = DifficultyUtils.CalculateSecondsFor1SetCountCategory(secondsCountCondition, setsCountCondition);

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