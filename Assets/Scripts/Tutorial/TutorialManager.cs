using UnityEngine;

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

            // btnHint.onClick.AddListener(Game.SelectHint);
            // btnRearrange.onClick.AddListener(Game.RearrangeActualCards);
            // btnAddThree.onClick.AddListener(Game.GetRandomThreeCards);
        }
    }
}