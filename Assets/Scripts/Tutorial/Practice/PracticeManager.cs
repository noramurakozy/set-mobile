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
        [SerializeField] private CardView placeholderCardPrefab;
        [SerializeField] private GridManager centerGrid;
        [SerializeField] private GridManager bottomGrid;
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
            Tutorial = new Tutorial(cardPrefab, placeholderCardPrefab, centerGrid, bottomGrid);
            Tutorial.StartNewTutorial();
            
            btnNewTutorial.onClick.AddListener(Tutorial.StartNewTutorial);
            btnPlay.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
        }
    }
}