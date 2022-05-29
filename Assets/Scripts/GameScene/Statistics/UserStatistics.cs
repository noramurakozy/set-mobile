using System;
using System.Collections.Generic;
using System.IO;
using Achievements;
using Achievements.AchievementTypes;
using Newtonsoft.Json;
using Statistics;
using UnityEngine;
using UnityEngine.Events;

namespace GameScene.Statistics
{
    public class UserStatistics
    {
        // General statistics
        public int TotalGameCount { get; set; }
        public TimeSpan BestTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan AvgTimePerSet { get; set; }
        public TimeSpan AvgTimePerGame { get; set; }
        [JsonProperty] private int TotalMistakeCount { get; set; }
        public int AvgMistakesPerGame { get; set; }
        [JsonProperty] private int TotalHintCount { get; set; }
        public int AvgHintsPerGame { get; set; }
        [JsonProperty] private int TotalShuffleCount { get; set; }
        public int AvgShufflesPerGame { get; set; }
        
        // SET related statistics
        public int NumSets1DiffProp { get; set; }
        public int NumSets2DiffProp { get; set; }
        public int NumSets3DiffProp { get; set; }
        public int NumSets4DiffProp { get; set; }
        public int TotalSetCount { get; set; }
        
        // Achievement related statistics
        public int NumHardAchievements { get; set; }
        public int NumMediumAchievements { get; set; }
        public int NumEasyAchievements { get; set; }
        public int AchievementsUnlockedInTotal { get; set; }
        public int CustomAchievementsCount { get; set; }
        
        public bool IsNewBestTime { get; set; }

        public void UpdateStatistics(GameStatistics gameStatistics)
        {
            TotalGameCount++;
            TotalMistakeCount += gameStatistics.MistakesCount;
            TotalHintCount += gameStatistics.HintsUsed;
            TotalShuffleCount += gameStatistics.ShufflesUsed;
            TotalSetCount += gameStatistics.SetsFound;
            TotalTime += TimeSpan.FromSeconds(gameStatistics.DurationInSeconds);
            
            UpdateBestTime(gameStatistics);
            AvgTimePerSet = TotalTime / TotalSetCount;
            AvgTimePerGame = TotalTime / TotalGameCount;
            AvgMistakesPerGame = TotalMistakeCount / TotalGameCount;
            AvgHintsPerGame = TotalHintCount / TotalGameCount;
            AvgShufflesPerGame = TotalShuffleCount / TotalGameCount;

            NumSets1DiffProp += gameStatistics.NumSets1DiffProp;
            NumSets2DiffProp += gameStatistics.NumSets2DiffProp;
            NumSets3DiffProp += gameStatistics.NumSets3DiffProp;
            NumSets4DiffProp += gameStatistics.NumSets4DiffProp;
        }

        private void UpdateBestTime(GameStatistics gameStatistics)
        {
            if (BestTime == TimeSpan.Zero)
            {
                BestTime = TimeSpan.FromSeconds(gameStatistics.DurationInSeconds);
            }
            else
            {
                if (gameStatistics.DurationInSeconds < BestTime.TotalSeconds)
                {
                    BestTime = TimeSpan.FromSeconds(gameStatistics.DurationInSeconds);
                    IsNewBestTime = true;
                }
                else
                {
                    IsNewBestTime = false;
                }
            }
        }

        public void UpdateAchievementStatistics()
        {
            var stats = AchievementManager.Instance.GetStatistics();
            AchievementsUnlockedInTotal = stats.UnlockedInTotal;
            CustomAchievementsCount = stats.CustomAchievementCount;
            NumEasyAchievements = stats.NumEasyAchievements;
            NumMediumAchievements = stats.NumMediumAchievements;
            NumHardAchievements = stats.NumHardAchievements;
        }
    }
}