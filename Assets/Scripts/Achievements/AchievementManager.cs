using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Achievements.AchievementTypes;
using Dialogs;
using Firebase.Analytics;
using FirebaseHandlers;
using GameScene.Statistics;
using Newtonsoft.Json;
using Sound;
using Statistics;
using UnityEngine;
using UnityEngine.Networking;

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
            FirebaseAnalytics.LogEvent("start_read_all_achievements");
            List<Achievement> allAchievements = new List<Achievement>();
            if (File.Exists(Application.persistentDataPath + "/achievements.json"))
            {
                FirebaseAnalytics.LogEvent("read_achievements", 
                    new Parameter("from", Application.persistentDataPath + "/achievements.json"));
                allAchievements =
                    JsonConvert.DeserializeObject<List<Achievement>>(
                        File.ReadAllText(Application.persistentDataPath + "/achievements.json"),
                        JsonUtils.SerializerSettings);
            }
            else
            {
                string filePathCustom = Path.Combine(Application.streamingAssetsPath, "defaultAchievementsCustom.json");
                string filePathStatic = Path.Combine(Application.streamingAssetsPath, "defaultAchievementsStatic.json");
#if (UNITY_EDITOR || UNITY_IOS || UNITY_STANDALONE_WIN)
                FirebaseAnalytics.LogEvent("read_achievements", 
                    new Parameter("from", RemoteConfigValueManager.Instance.CustomAchievements ? filePathCustom : filePathStatic),
                    new Parameter("device", "not_android"));
                allAchievements = RemoteConfigValueManager.Instance.CustomAchievements
                    ? JsonConvert.DeserializeObject<List<Achievement>>(
                        File.ReadAllText(filePathCustom), JsonUtils.SerializerSettings)
                    : JsonConvert.DeserializeObject<List<Achievement>>(
                        File.ReadAllText(filePathStatic), JsonUtils.SerializerSettings);
#endif

#if UNITY_ANDROID
                var loadingRequest = RemoteConfigValueManager.Instance.CustomAchievements
                    ? UnityWebRequest.Get(filePathCustom)
                    : UnityWebRequest.Get(filePathStatic);
                loadingRequest.SendWebRequest();
                while (!loadingRequest.isDone)
                {
                }

                if (loadingRequest.result != UnityWebRequest.Result.Success)
                {
                    FirebaseAnalytics.LogEvent("read_achievements_error", 
                        new Parameter("from", RemoteConfigValueManager.Instance.CustomAchievements ? filePathCustom : filePathStatic),
                        new Parameter("device", "android"));
                    Debug.LogError(
                        "An error occured while requesting data from the streaming assets folder on Android");
                }
                else
                {
                    FirebaseAnalytics.LogEvent("read_achievements_success", 
                        new Parameter("from", RemoteConfigValueManager.Instance.CustomAchievements ? filePathCustom : filePathStatic),
                        new Parameter("device", "android"));
                    Debug.Log(loadingRequest.downloadHandler.text);
                    using (var stream = new MemoryStream(loadingRequest.downloadHandler.data))
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                        allAchievements = JsonSerializer.Create(JsonUtils.SerializerSettings).Deserialize(reader, typeof(List<Achievement>)) as List<Achievement>;
                }
#endif
                SaveAchievements(allAchievements);
            }

            return allAchievements;
        }

        public Achievement InitiateAchievement(CreationType creationType, AchievementTemplate template,
            string achievementText, List<object> conditions)
        {
            var args = new List<object> { creationType, achievementText };

            args.AddRange(conditions);
            var instance = (Achievement)Activator.CreateInstance(template.Type, args.ToArray());
            instance.CalculateDifficulty();
            FirebaseAnalytics.LogEvent("init_achievement",
                new Parameter("creation_type", creationType.ToString()),
                    new Parameter("template", template.Type.FullName),
                new Parameter("achievement_text", achievementText),
                new Parameter("conditions", string.Join(", ", conditions)),
                    new Parameter("difficulty", instance.Difficulty.ToString()));
            return instance;
        }

        public void AddToExistingAchievements(Achievement newAchievement)
        {
            List<Achievement> allAchievements = new List<Achievement>();
            if (File.Exists(Application.persistentDataPath + "/achievements.json"))
            {
                allAchievements =
                    JsonConvert.DeserializeObject<List<Achievement>>(
                        File.ReadAllText(Application.persistentDataPath + "/achievements.json"),
                        JsonUtils.SerializerSettings);
            }

            allAchievements?.Insert(0, newAchievement);
            FirebaseAnalytics.LogEvent("achievement_created",
                new Parameter("achievement_text", newAchievement.Text),
                new Parameter("achievement_id", newAchievement.ID.ToString()),
                new Parameter("achievement_difficulty", newAchievement.Difficulty.ToString())
            );
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
                    FirebaseAnalytics.LogEvent("update_achievement_status", 
                        new Parameter("achievement_text", achievement.Text),
                        new Parameter("achievement_id", achievement.ID.ToString()));
                    // ToastManager.Instance.ShowToast(achievement.Text, 2f);
                    updatedAchievements.Add(achievement);
                }
            }

            if (updatedAchievements.Count > 0)
            {
                SoundManager.Instance.PlaySound(Sound.Sound.AchievementUnlocked);
                FirebaseAnalytics.LogEvent("play_achievement_sound");
                ToastManager.Instance.ShowToastList(updatedAchievements.Select(a => a.Text).ToList(), 2f);
                FirebaseAnalytics.LogEvent("show_achievement_toast_list", 
                    new Parameter("count", updatedAchievements.Count));
            }

            SaveAchievements(AllAchievements);
            UserStatisticsManager.Instance.UpdateAchievementStatistics();
        }

        private void SaveAchievements(List<Achievement> allAchievements)
        {
            File.WriteAllText(Application.persistentDataPath + "/achievements.json",
                JsonConvert.SerializeObject(allAchievements, JsonUtils.SerializerSettings));
            FirebaseAnalytics.LogEvent("save_achievements_to_file", 
                new Parameter("destination", Application.persistentDataPath + "/achievements.json"));
        }

        public void DeleteAchievement(Guid achievementID)
        {
            var achievementToDelete = AllAchievements.Find(a => a.ID == achievementID);
            AllAchievements.Remove(achievementToDelete);
            FirebaseAnalytics.LogEvent("achievement_deleted",
                new Parameter("achievement_text", achievementToDelete.Text),
                new Parameter("achievement_id", achievementToDelete.ID.ToString()));
            SaveAchievements(AllAchievements);
        }

        public AchievementStatistics GetStatistics()
        {
            var aList = ReadAllAchievements();
            var statistics = new AchievementStatistics
            {
                CustomAchievementCount = aList.Where(a => a.CreationType == CreationType.Custom).ToList().Count,
                UnlockedInTotal = aList.Where(a => a.Status == Status.Complete).ToList().Count,
                NumEasyAchievements = aList.Where(a => a.Status == Status.Complete && a.Difficulty == Difficulty.Easy)
                    .ToList().Count,
                NumMediumAchievements =
                    aList.Where(a => a.Status == Status.Complete && a.Difficulty == Difficulty.Medium).ToList().Count,
                NumHardAchievements = aList.Where(a => a.Status == Status.Complete && a.Difficulty == Difficulty.Hard)
                    .ToList().Count
            };
            FirebaseAnalytics.LogEvent("get_achievement_statistics",
                new Parameter("custom_achievement_count", statistics.CustomAchievementCount),
                new Parameter("unlocked_in_total", statistics.UnlockedInTotal),
                new Parameter("num_easy_achievements", statistics.NumEasyAchievements),
                new Parameter("num_medium_achievements", statistics.NumMediumAchievements),
                new Parameter("num_hard_achievements", statistics.NumHardAchievements)
                );

            return statistics;
        }
    }
}