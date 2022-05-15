using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public Game Game { get; set; }

        [SerializeField] private CardView cardPrefab;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Button BtnHint;
        [SerializeField] private Button BtnRearrange;
        [SerializeField] private Button BtnAddThree;
        [SerializeField] private TMP_Text txtCardsLeft;

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
            Game = new Game(cardPrefab, gridManager);
            Game.StartNewGame();

            BtnHint.onClick.AddListener(Game.SelectHint);
            BtnRearrange.onClick.AddListener(Game.RearrangeActualCards);
            BtnAddThree.onClick.AddListener(Game.GetRandomThreeCards);
        }
        
        private void Update()
        {
            txtCardsLeft.text = "Cards left in deck: " + Game.AllCards.Count;
        }
    }
}