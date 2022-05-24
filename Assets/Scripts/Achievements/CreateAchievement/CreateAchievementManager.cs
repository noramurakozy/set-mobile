using System.Collections.Generic;
using System.Linq;
using Achievements.AchievementTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
            btnAcceptAndCreate.onClick.AddListener(CreateAchievement);
        }

        private void CreateAchievement()
        {
            Debug.Log("Create achievement");
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
                    "Complete \'X\' games using maximum \'Y\' hints",
                    typeof(XGamesYHints)
                ),
                new(
                    "Complete \'X\' games in \'Y\' seconds",
                    typeof(XGamesYSeconds)
                ),
                new(
                    "Find \'X\' SETs within the first \'Y\' seconds in \'Z\' games",
                    typeof(XGamesYSetsZSeconds)
                ),
                new(
                    "Complete \'X\' games without shuffling the cards",
                    typeof(XGamesNoShuffle)
                ),
                new(
                    "Find \'X\' SETs with \'Y\' different properties in total",
                    typeof(XSetsYDiffProps)
                ),
                new(
                    "Complete \'X\' games with maximum \'Y\' mistakes",
                    typeof(XGamesYMistakes)
                ),
                new(
                    "Find \'X\' SETs in a row without making a mistake",
                    typeof(XSetsInARowNoMistakes)
                ),
                new(
                    "Complete \'X\' games in a row using maximum \'Y\' hints",
                    typeof(XGamesInARowYHints)
                ),
            };
        }
    }
}
