using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Achievements.AchievementTypes;
using Dialogs;
using GameScene.Statistics;
using Newtonsoft.Json;
using Statistics;
using UnityEngine;

namespace Achievements
{
    public class AchievementManager
    {
        private static AchievementManager _instance;

        public static AchievementManager Instance => _instance ??= new AchievementManager();
        private static List<Achievement> AllAchievements { get; set; }

        private AchievementManager()
        {
            AllAchievements = ReadAllAchievements();
        }

        public List<Achievement> ReadAllAchievements()
        {
            List<Achievement> allAchievements = new List<Achievement>();
            if (File.Exists(Application.persistentDataPath + "/achievements.json"))
            {
                allAchievements =
                    JsonConvert.DeserializeObject<List<Achievement>>(
                        File.ReadAllText(Application.persistentDataPath + "/achievements.json"), JsonUtils.SerializerSettings);
            }

            return allAchievements;
        }

        public Achievement InitiateAchievement(AchievementTemplate template, string achievementText, List<object> conditions)
        {
            var args = new List<object> {achievementText};

            args.AddRange(conditions);
            var instance = (Achievement)Activator.CreateInstance(template.Type, args.ToArray());
            instance.CalculateDifficulty();
            return instance;
        }

        public void AddToExistingAchievements(Achievement newAchievement)
        {
            List<Achievement> allAchievements = new List<Achievement>();
            if (File.Exists(Application.persistentDataPath + "/achievements.json"))
            {
                allAchievements =
                    JsonConvert.DeserializeObject<List<Achievement>>(
                        File.ReadAllText(Application.persistentDataPath + "/achievements.json"), JsonUtils.SerializerSettings);
            }

            allAchievements?.Insert(0, newAchievement);
            AllAchievements = allAchievements;
            SaveAchievements(allAchievements);
        }

        public void UpdateProgressOfAchievements(GameStatistics statistics, UpdateType updateType)
        {
            var updatedAchievements = new List<Achievement>();
            foreach (var achievement in AllAchievements.Where(a => a.UpdateType == updateType))
            {
                var preStatus = achievement.Status;
                achievement.UpdateProgress(statistics);
                var afterStatus = achievement.Status;
                if (preStatus == Status.InProgress && afterStatus == Status.Complete)
                {
                    // ToastManager.Instance.ShowToast(achievement.Text, 2f);
                    updatedAchievements.Add(achievement);
                    UserStatisticsManager.Instance.UpdateAchievementStatistics(achievement);
                }
            }

            ToastManager.Instance.ShowToastList(updatedAchievements.Select(a => a.Text).ToList(), 2f);   
            
            SaveAchievements(AllAchievements);
        }
        
        private void SaveAchievements(List<Achievement> allAchievements)
        {
            UserStatisticsManager.Instance.UpdateCustomAchievementCount(allAchievements.Count);
            File.WriteAllText(Application.persistentDataPath + "/achievements.json",
                JsonConvert.SerializeObject(allAchievements, JsonUtils.SerializerSettings));
        }

        public void DeleteAchievement(Guid achievementID)
        {
            var achievementToDelete = AllAchievements.Find(a => a.ID == achievementID);
            AllAchievements.Remove(achievementToDelete);
            SaveAchievements(AllAchievements);
        }
    }
}