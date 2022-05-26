using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Achievements.AchievementTypes;
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

            // args.AddRange(_inputFields.Select(field => (object)int.Parse(field.text)));
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
            foreach (var achievement in AllAchievements.Where(a => a.UpdateType == updateType))
            {
                achievement.UpdateProgress(statistics);
            }
            SaveAchievements(AllAchievements);
        }
        
        private void SaveAchievements(List<Achievement> allAchievements)
        {
            File.WriteAllText(Application.persistentDataPath + "/achievements.json",
                JsonConvert.SerializeObject(allAchievements, JsonUtils.SerializerSettings));
        }
    }
}