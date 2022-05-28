using System.IO;
using Achievements.AchievementTypes;
using Newtonsoft.Json;
using Statistics;
using UnityEngine;

namespace GameScene.Statistics
{
    public class UserStatisticsManager
    {
        private static UserStatisticsManager _instance;
        public static UserStatisticsManager Instance => _instance ??= new UserStatisticsManager();

        private UserStatisticsManager()
        {
            LoadUserStatistics();
        }
        
        public UserStatistics UserStatistics { get; set; }
        
        private void LoadUserStatistics()
        {
            if (File.Exists(Application.persistentDataPath + "/userStatistics.json"))
            {
                UserStatistics =
                    JsonConvert.DeserializeObject<UserStatistics>(
                        File.ReadAllText(Application.persistentDataPath + "/userStatistics.json"), JsonUtils.SerializerSettings);
            }
            else
            {
                UserStatistics = new UserStatistics();
            }
        }

        private void SaveUserStatistics()
        {
            File.WriteAllText(Application.persistentDataPath + "/userStatistics.json",
                JsonConvert.SerializeObject(UserStatistics, JsonUtils.SerializerSettings));
        }

        public void UpdateUserStatistics(GameStatistics gameStatistics)
        {
            LoadUserStatistics();
            UserStatistics.UpdateStatistics(gameStatistics);
            SaveUserStatistics();
        }

        public void UpdateAchievementStatistics(Achievement achievement)
        {
            LoadUserStatistics();
            UserStatistics.UpdateAchievementStatistics(achievement);
            SaveUserStatistics();
        }

        public void ClearStats()
        {
            File.Delete(Application.persistentDataPath + "/userStatistics.json");
            LoadUserStatistics();
        }

        public void UpdateCustomAchievementCount(int newCount)
        {
            UserStatistics.CustomAchievementsCount = newCount;
            SaveUserStatistics();
        }
    }
}