using Achievements;
using DG.Tweening;
using EasyUI.Dialogs;
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

        [SerializeField] private CardView.CardView cardPrefab;
        [SerializeField] private Sprite cardBack;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Button btnHint;
        [SerializeField] private Button btnShuffle;
        [SerializeField] private Button btnDeal;
        [SerializeField] private Button btnHowTo;
        [SerializeField] private Button btnSettings;
        [SerializeField] private Button btnHome;
        [SerializeField] private TMP_Text txtCardsLeft;
        [SerializeField] private TMP_Text txtSetCount;
        [SerializeField] private TMP_Text txtTimer;
        [SerializeField] private TMP_Text txtSetCountOnTable;
        [SerializeField] private Image hintCountBg;
        [SerializeField] private Image shuffleCountBg;
        [SerializeField] private Image timerBg;
        [SerializeField] private GameObject pausedOverlayGroup;
        [SerializeField] private ConfirmDialogUI confirmDialogUI;
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
            // if (PlayerPrefs.GetInt("gameInProgress", 0) == 1)
            // {
            //     Game.Load();
            //     ShowContinuePopup();
            // }
            // else
            // {
            //     Game.StartNewGame();
            // }
            Game.StartNewGame();

            btnHint.onClick.AddListener(Game.SelectHint);
            btnShuffle.onClick.AddListener(Game.RearrangeActualCards);
            btnDeal.onClick.AddListener(() => Game.DealAdditionalCards(3));
            btnHowTo.onClick.AddListener(() =>
            {
                // SaveGameInProgress();
                Game.PauseGame();
                ShowConfirmationPopup("TutorialScene");
                // SceneChanger.Instance.LoadScene("TutorialScene");
            });
            btnSettings.onClick.AddListener(() =>
            {
                // SaveGameInProgress();
                Game.PauseGame();
                ShowConfirmationPopup("SettingsScene");
                // SceneChanger.Instance.LoadScene("SettingsScene");
            });
            btnHome.onClick.AddListener(() =>
            {
                // SaveGameInProgress();
                Game.PauseGame();
                ShowConfirmationPopup("MainMenu");
                // SceneChanger.Instance.LoadScene("MainMenu");
            });
            pausedOverlayGroup.GetComponentInChildren<Button>().onClick.AddListener(ResumeGame);

            _txtHintCount = hintCountBg.GetComponentInChildren<TMP_Text>();
            _txtShuffleCount = shuffleCountBg.GetComponentInChildren<TMP_Text>();

            SetupUIPlayerPrefs();
        }

        // private void SaveGameInProgress()
        // {
        //     if (PlayerPrefs.GetInt("gameInProgress", 0) == 1)
        //     { 
        //         Game.Save();
        //     }
        // }

        // private void ShowContinuePopup()
        // {
        //     confirmDialogUI.gameObject.SetActive(true);
        //     confirmDialogUI
        //         .SetTitle("Continue game")
        //         .SetMessage(
        //             "Do you want to continue your game in progress?")
        //         .SetNegativeButtonText("No, start a new game")
        //         .SetPositiveButtonText("Yes, continue")
        //         .SetButtonsColor(DialogButtonColor.Blue)
        //         .SetFadeDuration(0.1f)
        //         .OnNegativeButtonClicked(() =>
        //         {
        //             Game.EndGame();
        //             Game.StartNewGame();
        //         })
        //         .OnPositiveButtonClicked(Game.ResumeGame)
        //         .Show();
        // }

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
            // SaveGameInProgress();
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
        
        private void ShowConfirmationPopup(string scene)
        {
            confirmDialogUI.gameObject.SetActive(true);
            confirmDialogUI
                .SetTitle("Exit game")
                .SetMessage(
                    "Are you sure you want to exit the game? Your progress will be lost...")
                .SetNegativeButtonText("No, continue playing")
                .SetPositiveButtonText("Yes, exit")
                .SetButtonsColor(DialogButtonColor.Blue)
                .SetFadeDuration(0.1f)
                .OnNegativeButtonClicked(Game.ResumeGame)
                .OnPositiveButtonClicked(() => SceneChanger.Instance.LoadScene(scene))
                .Show();
        }
    }
}