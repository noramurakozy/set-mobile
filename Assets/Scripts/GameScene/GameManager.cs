using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Achievements;
using DG.Tweening;
using EasyUI.Dialogs;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using GameScene.GUtils;
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
        
        void FetchComplete(Task fetchTask) {
            if (fetchTask.IsCanceled) {
                Debug.Log("Fetch canceled.");
            } else if (fetchTask.IsFaulted) {
                Debug.Log("Fetch encountered an error.");
            } else if (fetchTask.IsCompleted) {
                Debug.Log("Fetch completed successfully!");
            }

            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus) {
                case Firebase.RemoteConfig.LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                        .ContinueWithOnMainThread(task => {
                            Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                        });

                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason) {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }

        private void Start()
        {
            var defaults =  new Dictionary<string, object>
            {
                // yet, or if we ask for values that the server doesn't have:
                // server
                // These are the values that are used if we haven't fetched data from the
                { "testString", "defaulttring" }
            };

            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
                .ContinueWithOnMainThread(task => { });
            
            Debug.Log("Fetching data...");
            System.Threading.Tasks.Task fetchTask =
                FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                    TimeSpan.Zero);
            fetchTask.ContinueWithOnMainThread(FetchComplete);

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
                Game.StartNewGame();
            }
            // Game.StartNewGame();

            btnHint.onClick.AddListener(Game.SelectHint);
            btnShuffle.onClick.AddListener(Game.RearrangeActualCards);
            btnDeal.onClick.AddListener(() => Game.DealAdditionalCards(3));
            btnHowTo.onClick.AddListener(() =>
            {
                Game.PauseGame();
                ShowConfirmationPopup("TutorialScene");
                // SceneChanger.Instance.LoadScene("TutorialScene");
            });
            btnSettings.onClick.AddListener(() =>
            {
                Game.PauseGame();
                ShowConfirmationPopup("SettingsScene");
                // SceneChanger.Instance.LoadScene("SettingsScene");
            });
            btnHome.onClick.AddListener(() =>
            {
                Game.PauseGame();
                ShowConfirmationPopup("MainMenu");
                // SceneChanger.Instance.LoadScene("MainMenu");
            });
            pausedOverlayGroup.GetComponentInChildren<Button>().onClick.AddListener(ResumeGame);

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
                    Game.EndGame();
                    Game.StartNewGame();
                })
                .OnPositiveButtonClicked(Game.ResumeGame)
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
            // Debug.Log(FirebaseRemoteConfig.DefaultInstance.GetValue("testString").StringValue);
            var gameStatistics = GameStatisticsManager.Instance.GameStatistics;
            txtCardsLeft.text = Game.Deck.Count.ToString();
            txtSetCount.text = gameStatistics.SetsFound.ToString();
            txtTimer.text = Game.GetTimerString();
            _txtHintCount.text = gameStatistics.HintsUsed.ToString();
            _txtShuffleCount.text = gameStatistics.ShufflesUsed.ToString();
            txtSetCountOnTable.text = Game.GetNumOfSetsOnTable() + " SET(s) available";

            if (Game.IsGameEnded() && !_gameStatsSaved)
            {
                Game.EndGame();
                UserStatisticsManager.Instance.UpdateUserStatistics(gameStatistics);
                _gameStatsSaved = true;
                DOTween.CompleteAll();
                fader.ExitSceneAnimation("GameSummaryScene");
            }
            if (Settings.Instance.GetAutoDeal() && !Game.IsGameEnded())
            {
                if (Game.GetNumOfSetsOnTable() == 0)
                {
                    Game.DealAdditionalCards(3);
                }
            }
        }

        public void UpdateAchievementProgresses(GameStatistics gameStatistics, UpdateType updateType)
        {
            AchievementManager.Instance.UpdateProgressOfAchievements(gameStatistics, updateType);
        }

        private void OnDestroy()
        {
            SaveGameInProgress();
            // Game.EndGame();
        }

        public void PauseGame()
        {
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
                .OnNegativeButtonClicked(Game.ResumeGame)
                .OnPositiveButtonClicked(() =>
                {
                    SaveGameInProgress();
                    fader.ExitSceneAnimation(scene);
                })
                .Show();
        }
    }
}