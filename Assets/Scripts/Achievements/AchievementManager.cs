using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Achievements.AchievementTypes;
using Dialogs;
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
            List<Achievement> allAchievements = new List<Achievement>();
            if (File.Exists(Application.persistentDataPath + "/achievements.json"))
            {
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
                    Debug.LogError(
                        "An error occured while requesting data from the streaming assets folder on Android");
                }
                else
                {
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
                }
            }

            if (updatedAchievements.Count > 0)
            {
                SoundManager.Instance.PlaySound(Sound.Sound.AchievementUnlocked);
                ToastManager.Instance.ShowToastList(updatedAchievements.Select(a => a.Text).ToList(), 2f);
            }

            SaveAchievements(AllAchievements);
            UserStatisticsManager.Instance.UpdateAchievementStatistics();
        }

        private void SaveAchievements(List<Achievement> allAchievements)
        {
            File.WriteAllText(Application.persistentDataPath + "/achievements.json",
                JsonConvert.SerializeObject(allAchievements, JsonUtils.SerializerSettings));
        }

        public void DeleteAchievement(Guid achievementID)
        {
            var achievementToDelete = AllAchievements.Find(a => a.ID == achievementID);
            AllAchievements.Remove(achievementToDelete);
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

            return statistics;
        }
    }
}