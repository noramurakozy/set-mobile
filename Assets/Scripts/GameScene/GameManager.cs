using Achievements;
using DG.Tweening;
using EasyUI.Dialogs;
using Firebase.Analytics;
using GameScene.GUtils;
using GameScene.Statistics;
using SettingsScene;
using TMPro;
using UnityEngine;
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
        [SerializeField] private Fader fader;
        [SerializeField] private GameStopwatch gameStopwatch;
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
            fader.EnterSceneAnimation();
            _gameStatsSaved = false;
            Game = new Game(cardPrefab, cardBack, gridManager, gameStopwatch);
            if (PlayerPrefs.GetInt("gameInProgress", 0) == 1)
            {
                Game.Load();
                ShowContinuePopup();
            }
            else
            {
                FirebaseAnalytics.LogEvent("start_new_game",
                    new Parameter("reason", "previous_game_finished"));
                Game.StartNewGame();
            }
            // Game.StartNewGame();

            btnHint.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("hint_clicked");
                Game.SelectHint();
            });
            btnShuffle.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("shuffle_clicked");
                Game.RearrangeActualCards();
            });
            btnDeal.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("deal_cards",
                    new Parameter("reason", "user_click"));
                Game.DealAdditionalCards(3);
            });
            btnHowTo.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("pause_game",
                    new Parameter("reason", "user_attempt_exit_to_tutorial"));
                Game.PauseGame();
                ShowConfirmationPopup("TutorialScene");
                // SceneChanger.Instance.LoadScene("TutorialScene");
            });
            btnSettings.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("pause_game",
                    new Parameter("reason", "user_attempt_exit_to_settings"));
                Game.PauseGame();
                ShowConfirmationPopup("SettingsScene");
                // SceneChanger.Instance.LoadScene("SettingsScene");
            });
            btnHome.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("pause_game",
                    new Parameter("reason", "user_attempt_exit_to_home"));
                Game.PauseGame();
                ShowConfirmationPopup("MainMenu");
                // SceneChanger.Instance.LoadScene("MainMenu");
            });
            pausedOverlayGroup.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("resume_game",
                    new Parameter("reason", "user_resume_from_pause"));
                ResumeGame();
            });

            _txtHintCount = hintCountBg.GetComponentInChildren<TMP_Text>();
            _txtShuffleCount = shuffleCountBg.GetComponentInChildren<TMP_Text>();

            SetupUIPlayerPrefs();
        }

        private void SaveGameInProgress()
        {
            if (PlayerPrefs.GetInt("gameInProgress", 0) == 1)
            {
                Game.Save();
            }
        }

        private void ShowContinuePopup()
        {
            FirebaseAnalytics.LogEvent("open_continue_game_dialog");
            confirmDialogUI.gameObject.SetActive(true);
            confirmDialogUI
                .SetTitle("Continue game")
                .SetMessage(
                    "Do you want to continue your game in progress?")
                .SetNegativeButtonText("No, start a new game")
                .SetPositiveButtonText("Yes, continue")
                .SetButtonsColor(DialogButtonColor.Blue)
                .SetFadeDuration(0.1f)
                .OnNegativeButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("continue_game_dialog_new_game");
                    Game.EndGame();
                    Game.StartNewGame();
                })
                .OnPositiveButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("continue_game_dialog_resume_game");
                    Game.ResumeGame();
                })
                .OnCloseButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("close_continue_game_dialog_resume_game");
                    Game.ResumeGame();
                })
                .Show();
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

            if (Game.IsGameEnded() && !_gameStatsSaved)
            {
                FirebaseAnalytics.LogEvent("end_game",
                    new Parameter("reason", "game_ended"));
                Game.EndGame();
                FirebaseAnalytics.LogEvent("update_user_statistics");
                UserStatisticsManager.Instance.UpdateUserStatistics(gameStatistics);
                _gameStatsSaved = true;
                DOTween.CompleteAll();
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "GameScene"), 
                    new Parameter("to", "GameSummaryScene"));
                fader.ExitSceneAnimation("GameSummaryScene");
            }
            if (Settings.Instance.GetAutoDeal() && !Game.IsGameEnded())
            {
                if (Game.GetNumOfSetsOnTable() == 0)
                {
                    FirebaseAnalytics.LogEvent("deal_cards",
                        new Parameter("reason", "no_sets_on_table_auto_deal"));
                    Game.DealAdditionalCards(3);
                }
            }
        }

        public void UpdateAchievementProgresses(GameStatistics gameStatistics, UpdateType updateType)
        {
            AchievementManager.Instance.
                UpdateProgressOfAchievements(gameStatistics, updateType);
        }

        private void OnDestroy()
        {
            FirebaseAnalytics.LogEvent("save_game_in_progress", 
                new Parameter("reason", "unexpected_exit"));
            SaveGameInProgress();
            // Game.EndGame();
        }

        public void PauseGame()
        {
            FirebaseAnalytics.LogEvent("timer_pause_clicked");
            FirebaseAnalytics.LogEvent("pause_game",
                new Parameter("reason", "user_paused_game"));
            Game.PauseGame();
            DisplayPauseOverlay();
        }

        private void ResumeGame()
        {
            RemovePauseOverlay();
            Game.ResumeGame();
        }

        private void DisplayPauseOverlay()
        {
            pausedOverlayGroup.SetActive(true);
        }

        private void RemovePauseOverlay()
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
            FirebaseAnalytics.LogEvent("open_exit_game_dialog");
            confirmDialogUI.gameObject.SetActive(true);
            confirmDialogUI
                .SetTitle("Exit game")
                .SetMessage(
                    "Are you sure you want to exit the game?" +
                    "\n" +
                    "(Don't worry, your progress will be saved and you can continue the game later)")
                .SetNegativeButtonText("No, continue playing")
                .SetPositiveButtonText("Yes, exit")
                .SetButtonsColor(DialogButtonColor.Blue)
                .SetFadeDuration(0.1f)
                .OnNegativeButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("exit_game_dialog_continue");
                    FirebaseAnalytics.LogEvent("resume_game",
                        new Parameter("reason", "user_decided_to_stay_in_game"));
                    Game.ResumeGame();
                })
                .OnPositiveButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("save_game_in_progress", 
                        new Parameter("reason", "user_exit_to_other_screen"));
                    SaveGameInProgress();
                    FirebaseAnalytics.LogEvent("switch_scene", 
                        new Parameter("from", "GameScene"), 
                        new Parameter("to", scene));
                    FirebaseAnalytics.LogEvent("exit_game_dialog_exit");
                    fader.ExitSceneAnimation(scene);
                })
                .OnCloseButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("close_exit_game_dialog");
                    FirebaseAnalytics.LogEvent("resume_game",
                        new Parameter("reason", "user_closed_exit_dialog"));
                    Game.ResumeGame();
                })
                .Show();
        }
    }
}