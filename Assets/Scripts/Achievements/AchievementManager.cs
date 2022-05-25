using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Achievements.AchievementTypes;
using Newtonsoft.Json;
using Statistics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { get; private set; }
        
        private List<AchievementUI> _achievementUIs;
        [SerializeField] private AchievementUI achievementUIPrefab;
        [SerializeField] private AchievementUI completedAchievementUIPrefab;
        [SerializeField] private VerticalLayoutGroup inProgressScrollViewContent;
        [SerializeField] private VerticalLayoutGroup completedScrollViewContent;
        [SerializeField] private Button btnAddNew;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            DestroyAllAchievementUIs();
        }

        private void DestroyAllAchievementUIs()
        {
            foreach (Transform child in inProgressScrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in completedScrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void Start()
        {
            InitAchievementUIs();
            btnAddNew.onClick.AddListener(() => SceneManager.LoadScene("CreateAchievementScene"));
        }

        private void InitAchievementUIs()
        {
            var allAchievements = ReadAllAchievements();

            _achievementUIs = allAchievements?.Select(a =>
            {
                AchievementUI achievementUI;
                switch (a.Status)
                {
                    case Status.InProgress:
                        achievementUI = Instantiate(achievementUIPrefab, inProgressScrollViewContent.transform, false);
                        achievementUI.Achievement = a;
                        break;
                    case Status.Complete:
                        achievementUI = Instantiate(completedAchievementUIPrefab, completedScrollViewContent.transform, false);
                        achievementUI.Achievement = a;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return achievementUI;
            }).ToList();
        }

        private List<Achievement> ReadAllAchievements()
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
    }
}