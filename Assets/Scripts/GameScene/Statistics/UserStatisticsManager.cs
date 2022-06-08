using System.IO;
using Achievements.AchievementTypes;
using Firebase.Analytics;
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
                FirebaseAnalytics.LogEvent("load_user_statistics", new Parameter("from", Application.persistentDataPath + "/userStatistics.json"));
                UserStatistics =
                    JsonConvert.DeserializeObject<UserStatistics>(
                        File.ReadAllText(Application.persistentDataPath + "/userStatistics.json"), JsonUtils.SerializerSettings);
            }
            else
            {
                FirebaseAnalytics.LogEvent("load_user_statistics", new Parameter("from", "new_statistics"));
                UserStatistics = new UserStatistics();
            }
        }

        private void SaveUserStatistics()
        {
            FirebaseAnalytics.LogEvent("save_user_statistics");
            File.WriteAllText(Application.persistentDataPath + "/userStatistics.json",
                JsonConvert.SerializeObject(UserStatistics, JsonUtils.SerializerSettings));
        }

        public void UpdateUserStatistics(GameStatistics gameStatistics)
        {
            LoadUserStatistics();
            UserStatistics.UpdateStatistics(gameStatistics);
            FirebaseAnalytics.LogEvent("update_user_statistics");
            SaveUserStatistics();
        }

        public void UpdateAchievementStatistics()
        {
            LoadUserStatistics();
            FirebaseAnalytics.LogEvent("update_achievement_statistics");
            UserStatistics.UpdateAchievementStatistics();
            SaveUserStatistics();
        }

        public void ClearStats()
        {
            File.Delete(Application.persistentDataPath + "/userStatistics.json");
            UpdateAchievementStatistics();
        }
    }
}