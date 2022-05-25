using System.Collections.Generic;
using System.IO;
using System.Linq;
using Achievements.AchievementTypes;
using Newtonsoft.Json;
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
            foreach (Transform child in inProgressScrollViewContent.transform) {
                Destroy(child.gameObject);
            }
        }

        private void Start()
        {
            
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var allAchievements = JsonConvert.DeserializeObject<List<Achievement>>(File.ReadAllText(Application.persistentDataPath + "/achievements.json"),settings);

            if (allAchievements != null)
            {
                _achievementUIs = allAchievements.Select(x =>
                {
                    var achievementUI = Instantiate(achievementUIPrefab, inProgressScrollViewContent.transform, false);
                    achievementUI.Achievement = x;
                    return achievementUI;
                }).ToList();
            }

            btnAddNew.onClick.AddListener(() => SceneManager.LoadScene("CreateAchievementScene"));
        }
    }
}