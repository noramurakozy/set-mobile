using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class DifficultyUI : MonoBehaviour
    {
        [SerializeField] private Image difficultyEasy;
        [SerializeField] private Image difficultyMedium;
        [SerializeField] private Image difficultyHard;
        // private Difficulty _difficulty = Difficulty.Medium;

        public void SetDifficulty(Difficulty difficulty)
        {
            // _difficulty = difficulty;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    InstantiateDifficulty(difficultyEasy);
                    break;
                case Difficulty.Medium:
                    InstantiateDifficulty(difficultyMedium);
                    break;
                case Difficulty.Hard:
                    InstantiateDifficulty(difficultyHard);
                    break;
                default:
                    InstantiateDifficulty(difficultyMedium);
                    break;
            }
            
        }

        private void InstantiateDifficulty(Image difficulty)
        {
            DestroyPreviousDifficulty();
            Instantiate(difficulty, transform, false);
        }

        private void DestroyPreviousDifficulty()
        {
            var previousDifficulty = GetComponentInChildren<Image>();
            Destroy(previousDifficulty.gameObject);
        }
    }
}
