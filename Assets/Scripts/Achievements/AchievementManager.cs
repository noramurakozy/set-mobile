using System.Collections.Generic;
using Achievements.AchievementTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { get; private set; }
        
        private List<AchievementUI> _achievementUIs;
        [SerializeField] private AchievementUI achievementUIPrefab;
        [SerializeField] private VerticalLayoutGroup inProgressScrollViewContent;
        [SerializeField] private VerticalLayoutGroup completedScrollViewContent;
        public List<AchievementTemplate> achievementTemplates;

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

        private void Start()
        {
            _achievementUIs = new List<AchievementUI>();
            for (int i = 0; i < 10; i++)
            {
                InstantiateAchievement(i);
            }

            CreateAchievementTemplates();
        }

        private void CreateAchievementTemplates()
        {
            achievementTemplates = new List<AchievementTemplate>
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

        private void InstantiateAchievement(int i)
        {
            var achi = Instantiate(achievementUIPrefab, inProgressScrollViewContent.transform, false);
            achi.Difficulty = Difficulty.Easy;
            achi.Text = "xyzasdalsklaf laksdlakjrldmvlkadfml alksdjalw " + i;
            
            _achievementUIs.Add(achi);
        }
    }
}