using Achievements;
using DG.Tweening;
using GameScene.Statistics;
using SettingsScene;
using Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UpdateType = Achievements.AchievementTypes.UpdateType;

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
        [SerializeField] private TMP_Text txtSetCountOnTable;
        [SerializeField] private Image hintCountBg;
        [SerializeField] private Image shuffleCountBg;
        [SerializeField] private Image timerBg;
        [SerializeField] private GameObject pausedOverlayGroup;
        private TMP_Text _txtHintCount;
        private TMP_Text _txtShuffleCount;

        private bool _gameStatsSaved;

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
            _gameStatsSaved = false;
            Game = new Game(cardPrefab, cardBack, gridManager);
            Game.StartNewGame();

            btnHint.onClick.AddListener(Game.SelectHint);
            btnShuffle.onClick.AddListener(Game.RearrangeActualCards);
            btnDeal.onClick.AddListener(() => Game.DealAdditionalCards(3));
            btnHowTo.onClick.AddListener(() => SceneChanger.Instance.LoadScene("TutorialScene"));
            btnSettings.onClick.AddListener(() => SceneChanger.Instance.LoadScene("SettingsScene"));
            pausedOverlayGroup.GetComponentInChildren<Button>().onClick.AddListener(ResumeGame);

            _txtHintCount = hintCountBg.GetComponentInChildren<TMP_Text>();
            _txtShuffleCount = shuffleCountBg.GetComponentInChildren<TMP_Text>();

            SetupUIPlayerPrefs();
        }

        private void SetupUIPlayerPrefs()
        {
            timerBg.gameObject.SetActive(Settings.Instance.GetShowTimer());
            hintCountBg.gameObject.SetActive(Settings.Instance.GetShowHintsUsed());
            shuffleCountBg.gameObject.SetActive(Settings.Instance.GetShowShufflesUsed());
            txtSetCountOnTable.gameObject.SetActive(Settings.Instance.GetShowNumOfSets());
        }

        private void Update()
        {
            var gameStatistics = GameStatisticsManager.Instance.GameStatistics;
            txtCardsLeft.text = Game.Deck.Count.ToString();
            txtSetCount.text = gameStatistics.SetsFound.ToString();
            txtTimer.text = Game.GetTimerString();
            _txtHintCount.text = gameStatistics.HintsUsed.ToString();
            _txtShuffleCount.text = gameStatistics.ShufflesUsed.ToString();
            txtSetCountOnTable.text = Game.GetNumOfSetsOnTable() + " SET(s) available";
            if (Settings.Instance.GetAutoDeal())
            {
                if (Game.GetNumOfSetsOnTable() == 0)
                {
                    Game.DealAdditionalCards(3);
                }
            }

            if (Game.IsGameEnded() && !_gameStatsSaved)
            {
                Game.EndGame();
                UpdateAchievementProgresses(gameStatistics, UpdateType.EndOfGame);
                UserStatisticsManager.Instance.UpdateUserStatistics(gameStatistics);
                _gameStatsSaved = true;
                DOTween.CompleteAll();
                SceneChanger.Instance.LoadScene("GameSummaryScene");
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
            if (!hintBtnEnabled)
            {
                btnHint.GetComponentInChildren<TMP_Text>().color = new Color32(123, 123, 123,255);
            }
            else
            {
                btnHint.GetComponentInChildren<TMP_Text>().color = new Color32(0, 0, 0,255);
            }
        }
    }
}