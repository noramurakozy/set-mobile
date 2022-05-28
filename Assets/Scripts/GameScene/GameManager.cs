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
        [SerializeField] private TMP_Text txtSetCountOnTable;
        [SerializeField] private Image hintCountBg;
        [SerializeField] private Image shuffleCountBg;
        [SerializeField] private Image timerBg;
        [SerializeField] private GameObject pausedOverlayGroup;
        private TMP_Text _txtHintCount;
        private TMP_Text _txtShuffleCount;

        private GameStatistics _finalGameStatistics;
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
            if (PlayerPrefs.GetInt("showHintsUsed") == 1)
            {
                hintCountBg.gameObject.SetActive(true);
            }

            if (PlayerPrefs.GetInt("showShufflesUsed") == 1)
            {
                shuffleCountBg.gameObject.SetActive(true);
            }

            if (PlayerPrefs.GetInt("showTimer") == 0)
            {
                timerBg.gameObject.SetActive(false);
            }
            if (PlayerPrefs.GetInt("showNumOfSets") == 1)
            {
                txtSetCountOnTable.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            txtCardsLeft.text = Game.Deck.Count.ToString();
            txtSetCount.text = Game.Statistics.SetsFound.ToString();
            txtTimer.text = Game.GetTimerString();
            _txtHintCount.text = Game.Statistics.HintsUsed.ToString();
            _txtShuffleCount.text = Game.Statistics.ShufflesUsed.ToString();
            txtSetCountOnTable.text = Game.GetNumOfSetsOnTable() + " SETs available";

            if (Game.IsGameEnded() && !_gameStatsSaved)
            {
                _finalGameStatistics = Game.EndGame();
                UpdateAchievementProgresses(_finalGameStatistics, UpdateType.EndOfGame);
                UserStatisticsManager.Instance.UpdateUserStatistics(_finalGameStatistics);
                _gameStatsSaved = true;
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