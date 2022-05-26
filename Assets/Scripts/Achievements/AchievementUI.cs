using Achievements.AchievementTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementUI : MonoBehaviour
    {
        [SerializeField] private DifficultyUI difficultyUI;
        [SerializeField] private TMP_Text textUI;
        [SerializeField] private Button btnDelete;

        public Achievement Achievement { get; set; }

        private void Start()
        {
            difficultyUI.SetDifficulty(Achievement.Difficulty);
            textUI.text = Achievement.Text;
            btnDelete.onClick.AddListener(() => AchievementUIManager.Instance.DeleteAchievement(this));
        }
    }
}