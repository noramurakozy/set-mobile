using System.Collections.Generic;
using System.IO;
using Achievements.AchievementTypes;
using Firebase.Analytics;
using FirebaseHandlers;
using Newtonsoft.Json;
using Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Achievements.CreateAchievement
{
    public class CreateAchievementManager : MonoBehaviour
    {
        public static CreateAchievementManager Instance { get; private set; }
        [SerializeField] private StepperController stepperController;
        [SerializeField] private Button btnBack;
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
        }
        
        // Start is called before the first frame update
        void Start()
        {
            FirebaseAnalytics.LogEvent("enter_create_achievement_scene", 
                        new Parameter("custom_achievements", RemoteConfigValueManager.Instance.CustomAchievements.ToString()));
            fader.EnterSceneAnimation();
            stepperController.AchievementTemplates = CreateAchievementTemplates();
            stepperController.MoveToStep(1);
            btnBack.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "CreateAchievementScene"), 
                    new Parameter("to", "AchievementsScene"));
                fader.ExitSceneAnimation("AchievementsScene");
            });
        }

        public void CreateAchievement(Achievement createdAchievement)
        {
            AchievementManager.Instance.AddToExistingAchievements(createdAchievement);
            fader.ExitSceneAnimation("AchievementsScene");
        }

        private List<AchievementTemplate> CreateAchievementTemplates()
        {
            return new List<AchievementTemplate>
            {
                new(
                    "Complete {0} game(s) using maximum {1} hint(s)",
                    typeof(XGamesYHints),
                    new[] {1, 0},
                    new [] {10000, 10000}
                ),
                new(
                    "Complete {0} game(s) in {1} minute(s) and {2} second(s)",
                    typeof(XGamesYSeconds),
                    new[] {1, 0, 0},
                    new [] {10000, 10000, 10000}
                ),
                new(
                    "Find {0} SET(s) within the first {1} second(s) in {2} game(s)",
                    typeof(XSetsYSecondsZGames),
                    new[] {1, 1, 1},
                    new [] {10000, 10000, 10000}
                ),
                new(
                    "Complete {0} game(s) without shuffling the cards",
                    typeof(XGamesNoShuffle),
                    new[] {1},
                    new [] {10000}
                ),
                new(
                    "Find {0} SET(s) with {1} different properties in total",
                    typeof(XSetsYDiffProps),
                    new[] {1, 1},
                    new [] {10000, 4}
                ),
                new(
                    "Complete {0} game(s) with maximum {1} mistake(s)",
                    typeof(XGamesYMistakes),
                    new[] {1, 0},
                    new [] {10000, 10000}
                ),
                new(
                    "Find {0} SET(s) in a row without making a mistake or using hints",
                    typeof(XSetsInARowNoMistakesNoHints),
                    new[] {1},
                    new [] {10000}
                ),
                new(
                    "Complete {0} game(s) in a row using maximum {1} hint(s)",
                    typeof(XGamesInARowYHints),
                    new[] {1, 0},
                    new [] {10000, 10000}
                ),
            };
        }
    }
}
