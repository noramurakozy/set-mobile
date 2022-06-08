using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Achievements.AchievementTypes;
using EasyUI.Dialogs;
using Firebase.Analytics;
using FirebaseHandlers;
using Newtonsoft.Json;
using Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementUIManager : MonoBehaviour
    {
        public static AchievementUIManager Instance { get; private set; }
        
        private List<AchievementUI> _achievementUIs;
        [SerializeField] private AchievementUI achievementUIPrefab;
        [SerializeField] private AchievementUI completedAchievementUIPrefab;
        [SerializeField] private VerticalLayoutGroup inProgressScrollViewContent;
        [SerializeField] private VerticalLayoutGroup completedScrollViewContent;
        [SerializeField] private Button btnAddNew;
        [SerializeField] private Button btnHome;
        [SerializeField] private TMP_Text inProgressPlaceholderText;
        [SerializeField] private TMP_Text completedPlaceholderText;
        [SerializeField] private ConfirmDialogUI confirmDialogUI;
        [SerializeField] private Fader fader;

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

        private void Start()
        {
            FirebaseAnalytics.LogEvent("enter_achievements_scene", 
                new Parameter("custom_achievements", RemoteConfigValueManager.Instance.CustomAchievements.ToString()));
            fader.EnterSceneAnimation();
            inProgressPlaceholderText.gameObject.SetActive(true);
            completedPlaceholderText.gameObject.SetActive(true);
            InitAchievementUIs();
            if (RemoteConfigValueManager.Instance.CustomAchievements)
            {
                btnAddNew.onClick.AddListener(() =>
                {
                    FirebaseAnalytics.LogEvent("switch_scene", 
                        new Parameter("from", "AchievementsScene"), 
                        new Parameter("to", "CreateAchievementScene"));
                    fader.ExitSceneAnimation("CreateAchievementScene");
                });
            }
            else
            {
                btnAddNew.gameObject.SetActive(false);
            }
            
            btnHome.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "AchievementsScene"), 
                    new Parameter("to", "MainMenu"));
                fader.ExitSceneAnimation("MainMenu");
            });
        }

        private void Update()
        {
            inProgressPlaceholderText.gameObject.SetActive(inProgressScrollViewContent.transform.childCount == 0);
            completedPlaceholderText.gameObject.SetActive(completedScrollViewContent.transform.childCount == 0);
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

        private void InitAchievementUIs()
        {
            var allAchievements = AchievementManager.Instance.ReadAllAchievements();
            FirebaseAnalytics.LogEvent("start_init_achievements");
            _achievementUIs = allAchievements?.Select(a =>
            {
                AchievementUI achievementUI;
                switch (a.Status)
                {
                    case Status.InProgress:
                        achievementUI = Instantiate(achievementUIPrefab, inProgressScrollViewContent.transform, false);
                        achievementUI.Achievement = a;
                        FirebaseAnalytics.LogEvent("init_achievement_in_progress",
                            new Parameter("achievement_text", a.Text),
                            new Parameter("achievement_id", a.ID.ToString()));
                        break;
                    case Status.Complete:
                        achievementUI = Instantiate(completedAchievementUIPrefab, completedScrollViewContent.transform, false);
                        achievementUI.Achievement = a;
                        FirebaseAnalytics.LogEvent("init_achievement_complete",
                            new Parameter("achievement_text", a.Text),
                            new Parameter("achievement_id", a.ID.ToString()));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return achievementUI;
            }).ToList();
        }

        public void DeleteAchievement(AchievementUI achievementUI)
        {
            FirebaseAnalytics.LogEvent("achievement_deleted", 
                new Parameter("achievement_text", achievementUI.Achievement.Text),
                new Parameter("achievement_id", achievementUI.Achievement.ID.ToString()));
            AchievementManager.Instance.DeleteAchievement(achievementUI.Achievement.ID);
            Destroy(achievementUI.gameObject);
        }

        public void ShowConfirmationDialog(AchievementUI achievementUI)
        {
            FirebaseAnalytics.LogEvent("open_confirm_delete_achievement", 
                new Parameter("achievement_text", achievementUI.Achievement.Text),
                new Parameter("achievement_id", achievementUI.Achievement.ID.ToString()));
            confirmDialogUI.gameObject.SetActive(true);
            confirmDialogUI
                .SetTitle("Delete achievement")
                .SetMessage(
                    "Are you sure you want delete this achievement?")
                .SetNegativeButtonText("Yes, delete")
                .SetPositiveButtonText("No, I want to keep it")
                .SetButtonsColor(DialogButtonColor.Green)
                .SetFadeDuration(0.1f)
                .OnNegativeButtonClicked(() => DeleteAchievement(achievementUI))
                .OnPositiveButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("cancel_delete_achievement");
                })
                .OnCloseButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("close_delete_achievement");
                })
                .Show();
        }
    }
}