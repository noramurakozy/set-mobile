using Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameScene
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public Game Game { get; set; }

        [SerializeField] private CardView cardPrefab;
        [SerializeField] private Sprite cardBack;
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

        private GameStatistics _finalGameStatistics;

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
            Game = new Game(cardPrefab, cardBack, gridManager);
            Game.StartNewGame();

            btnHint.onClick.AddListener(Game.SelectHint);
            btnShuffle.onClick.AddListener(Game.RearrangeActualCards);
            btnDeal.onClick.AddListener(() => Game.DealAdditionalCards(3));
            btnHowTo.onClick.AddListener(() => SceneManager.LoadScene("TutorialScene"));
            pausedOverlayGroup.GetComponentInChildren<Button>().onClick.AddListener(ResumeGame);
        }
        
        private void Update()
        {
            txtCardsLeft.text = Game.Deck.Count.ToString();
            txtSetCount.text = Game.SetsFoundCount.ToString();
            txtTimer.text = Game.GetStopwatchString();
            if (Game.IsGameEnded())
            {
                // TODO: save in file so AchievementManager can read it and
                // evaluate whether one of the achievements became completed
                _finalGameStatistics = Game.EndGame();
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void OnDestroy()
        {
            Game.EndGameWithoutSave();
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

        public void EnableHintBtn(bool hintBtnEnabled)
        {
            btnHint.enabled = hintBtnEnabled;
        }
    }
}