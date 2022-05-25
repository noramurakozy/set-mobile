using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class DifficultyUI : MonoBehaviour
    {
        [SerializeField] private Image difficultyEasy;
        [SerializeField] private Image difficultyMedium;
        [SerializeField] private Image difficultyHard;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

        public void SetDifficulty(Difficulty difficulty)
        {
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
            Instantiate(difficulty, verticalLayoutGroup.transform, false);
        }

        private void DestroyPreviousDifficulty()
        {
            var previousDifficulty = verticalLayoutGroup.GetComponentInChildren<Image>();
            Destroy(previousDifficulty.gameObject);
        }
    }
}
