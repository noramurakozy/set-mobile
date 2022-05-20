using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public Game Game { get; set; }

        [SerializeField] private CardView cardPrefab;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Button btnHint;
        [SerializeField] private Button btnShuffle;
        [SerializeField] private Button btnDeal;
        [SerializeField] private Button btnHowTo;
        [SerializeField] private Button btnSettings;
        [SerializeField] private TMP_Text txtCardsLeft;
        [SerializeField] private TMP_Text txtSetCount;
        [SerializeField] private TMP_Text txtTimer;
        [SerializeField] private GameObject pausedOverlayGroup;

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

            btnHint.onClick.AddListener(Game.SelectHint);
            btnShuffle.onClick.AddListener(Game.RearrangeActualCards);
            btnDeal.onClick.AddListener(Game.GetRandomThreeCards);
            btnHowTo.onClick.AddListener(() => SceneManager.LoadScene("TutorialScene"));
            pausedOverlayGroup.GetComponentInChildren<Button>().onClick.AddListener(ResumeGame);
        }
        
        private void Update()
        {
            txtCardsLeft.text = Game.AllCards.Count.ToString();
            txtSetCount.text = Game.SetsFoundCount.ToString();
            txtTimer.text = Game.GetStopwatchString();
        }

        private void OnDestroy()
        {
            Game.EndGame();
        }

        public void PauseGame()
        {
            Game.PauseGame();
            DisplayPauseOverlay();
        }

        private void ResumeGame()
        {
            RemoveDisplayOverlay();
            Game.ResumeGame();
        }

        private void DisplayPauseOverlay()
        {
            pausedOverlayGroup.SetActive(true);
        }

        private void RemoveDisplayOverlay()
        {
            pausedOverlayGroup.SetActive(false);
        }
    }
}