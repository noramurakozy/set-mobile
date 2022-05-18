using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }
        public global::Tutorial.Tutorial Tutorial { get; set; }
        
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private SpriteRenderer placeholderCard;
        [SerializeField] private GridManager centerGrid;
        [SerializeField] private GridManager bottomGrid;
        [SerializeField] private Button btnPlay;
        [SerializeField] private Button btnGetNewTutorial;
        
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
            Tutorial = new global::Tutorial.Tutorial(cardPrefab, placeholderCard, centerGrid, bottomGrid);
            Tutorial.StartNewTutorial();

            btnGetNewTutorial.onClick.AddListener(Tutorial.StartNewTutorial);
            btnPlay.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
        }
    }
}