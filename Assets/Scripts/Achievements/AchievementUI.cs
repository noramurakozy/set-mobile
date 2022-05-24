using Achievements.AchievementTypes;
using TMPro;
using UnityEngine;

namespace Achievements
{
    public class AchievementUI : MonoBehaviour
    {
        public Difficulty Difficulty { get; set; }
        public string Text { get; set; }

        [SerializeField] private DifficultyUI difficultyUI;
        [SerializeField] private TMP_Text textUI;

        private Achievement _achievement;

        private void Start()
        {
            difficultyUI.SetDifficulty(Difficulty);
            textUI.text = Text;
            // _achievement = new XGamesYHints(Difficulty, Text, 2, 100);
        }
    }
}