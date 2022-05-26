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
                    "Complete {0} games using maximum {1} hints",
                    typeof(XGamesYHints),
                    new[] {1, 0},
                    new [] {10000, 10000}
                ),
                new(
                    "Complete {0} games in {1} seconds",
                    typeof(XGamesYSeconds),
                    new[] {1, 1},
                    new [] {10000, 10000}
                ),
                new(
                    "Find {0} SETs within the first {1} seconds in {2} games",
                    typeof(XSetsYSecondsZGames),
                    new[] {1, 1, 1},
                    new [] {10000, 10000, 10000}
                ),
                new(
                    "Complete {0} games without shuffling the cards",
                    typeof(XGamesNoShuffle),
                    new[] {1},
                    new [] {10000}
                ),
                new(
                    "Find {0} SETs with {1} different properties in total",
                    typeof(XSetsYDiffProps),
                    new[] {1, 1},
                    new [] {10000, 4}
                ),
                new(
                    "Complete {0} games with maximum {1} mistakes",
                    typeof(XGamesYMistakes),
                    new[] {1, 0},
                    new [] {10000, 10000}
                ),
                new(
                    "Find {0} SETs in a row without making a mistake or using hints",
                    typeof(XSetsInARowNoMistakesNoHints),
                    new[] {1},
                    new [] {10000}
                ),
                new(
                    "Complete {0} games in a row using maximum {1} hints",
                    typeof(XGamesInARowYHints),
                    new[] {1, 0},
                    new [] {10000, 10000}
                ),
            };
        }
    }
}
