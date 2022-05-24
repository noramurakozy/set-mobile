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

        public AchievementUI(Difficulty difficulty, string text)
        {
            Difficulty = difficulty;
            Text = text;
            _achievement = new Achievement(difficulty, text);
        }

        private void Start()
        {
            difficultyUI.SetDifficulty(Difficulty);
            textUI.text = Text;
        }
    }
}