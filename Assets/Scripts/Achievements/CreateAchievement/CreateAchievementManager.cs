using System.Collections.Generic;
using System.IO;
using Achievements.AchievementTypes;
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
        [SerializeField] private StepperUI stepperUI;
        [SerializeField] private Button btnNextStep;
        [SerializeField] private Button btnAcceptAndCreate;
        
        private int CurrentStep { get; set; }
        
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
            CurrentStep = 1;
            stepperUI.AchievementTemplates = CreateAchievementTemplates();
            stepperUI.MoveToStep(CurrentStep);
            
            btnNextStep.onClick.AddListener(MoveToNextStep);
            btnAcceptAndCreate.onClick.AddListener(() =>
            {
                CreateAchievement();
                SceneManager.LoadScene("AchievementsScene");
            });
        }

        private void CreateAchievement()
        {
            var newAchievement = stepperUI.CreatedAchievement;
            AchievementManager.Instance.AddToExistingAchievements(newAchievement);
        }

        private void MoveToNextStep()
        {
            CurrentStep++;
            stepperUI.MoveToStep(CurrentStep);
            if (CurrentStep == 3)
            {
                btnAcceptAndCreate.gameObject.SetActive(true);
                btnNextStep.gameObject.SetActive(false);
            }
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
                    "Complete {0} game(s) in {1} second(s)",
                    typeof(XGamesYSeconds),
                    new[] {1, 1},
                    new [] {10000, 10000}
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
