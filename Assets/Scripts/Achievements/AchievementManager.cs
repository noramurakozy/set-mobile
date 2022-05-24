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