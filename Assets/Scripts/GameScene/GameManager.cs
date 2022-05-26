using Achievements;
using Achievements.AchievementTypes;
using GameScene.Statistics;
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
            txtSetCount.text = Game.Statistics.SetsFound.ToString();
            txtTimer.text = Game.GetStopwatchString();
            if (Game.IsGameEnded())
            {
                _finalGameStatistics = Game.EndGame();
                UpdateAchievementProgresses(_finalGameStatistics, UpdateType.EndOfGame);
                // SceneManager.LoadScene("MainMenu");
            }
        }

        public void UpdateAchievementProgresses(GameStatistics gameStatistics, UpdateType updateType)
        {
            AchievementManager.Instance.UpdateProgressOfAchievements(gameStatistics, updateType);
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

        public void EnableHintBtn(bool hintBtnEnabled)
        {
            btnHint.enabled = hintBtnEnabled;
        }
    }
}