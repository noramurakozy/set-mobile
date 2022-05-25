using Achievements.AchievementTypes;
using TMPro;
using UnityEngine;

namespace Achievements
{
    public class AchievementUI : MonoBehaviour
    {
        [SerializeField] private DifficultyUI difficultyUI;
        [SerializeField] private TMP_Text textUI;

        public Achievement Achievement { get; set; }

        private void Start()
        {
            difficultyUI.SetDifficulty(Achievement.Difficulty);
            textUI.text = Achievement.Text;
            // _achievement = new XGamesYHints(Difficulty, Text, 2, 100);
        }
    }
}