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
        [SerializeField] private TMP_Dropdown achievementsDropdown;
        
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
            var allAchievements = AddToExistingAchievements(newAchievement);
            SaveAchievements(allAchievements);
            
            Debug.Log(newAchievement);
        }

        private static void SaveAchievements(List<Achievement> allAchievements)
        {
            File.WriteAllText(Application.persistentDataPath + "/achievements.json",
                JsonConvert.SerializeObject(allAchievements, JsonUtils.SerializerSettings));
        }

        private List<Achievement> AddToExistingAchievements(Achievement newAchievement)
        {
            List<Achievement> allAchievements = new List<Achievement>();
            if (File.Exists(Application.persistentDataPath + "/achievements.json"))
            {
                allAchievements =
                    JsonConvert.DeserializeObject<List<Achievement>>(
                        File.ReadAllText(Application.persistentDataPath + "/achievements.json"), JsonUtils.SerializerSettings);
            }

            allAchievements?.Insert(0, newAchievement);
            return allAchievements;
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
                    typeof(XGamesYHints)
                ),
                new(
                    "Complete {0} games in {1} seconds",
                    typeof(XGamesYSeconds)
                ),
                new(
                    "Find {0} SETs within the first {1} seconds in {2} games",
                    typeof(XSetsYSecondsZGames)
                ),
                new(
                    "Complete {0} games without shuffling the cards",
                    typeof(XGamesNoShuffle)
                ),
                new(
                    "Find {0} SETs with {1} different properties in total",
                    typeof(XSetsYDiffProps)
                ),
                new(
                    "Complete {0} games with maximum {1} mistakes",
                    typeof(XGamesYMistakes)
                ),
                new(
                    "Find {0} SETs in a row without making a mistake",
                    typeof(XSetsInARowNoMistakes)
                ),
                new(
                    "Complete {0} games in a row using maximum {1} hints",
                    typeof(XGamesInARowYHints)
                ),
            };
        }
    }
}
