using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tutorial.Practice
{
    public class PracticeManager : MonoBehaviour
    {
        public static PracticeManager Instance { get; private set; }
        
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private SpriteRenderer placeholderCardPrefab;
        [SerializeField] private TutorialGridManager centerLeftGrid;
        [SerializeField] private TutorialGridManager centerVerticalGrid;
        [SerializeField] private Button btnNewTutorial;
        [SerializeField] private Button btnPlay;
        
        public Tutorial Tutorial { get; set; }
        
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
            Tutorial = new Tutorial(cardPrefab, placeholderCardPrefab, centerLeftGrid, centerVerticalGrid);
            Tutorial.StartNewTutorial();
            
            btnNewTutorial.onClick.AddListener(Tutorial.StartNewTutorial);
            btnPlay.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
        }
    }
}