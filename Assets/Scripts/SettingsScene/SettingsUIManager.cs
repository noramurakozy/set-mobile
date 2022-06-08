using System;
using System.IO;
using EasyUI.Dialogs;
using Firebase.Analytics;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsScene
{
    public class SettingsUIManager : MonoBehaviour
    {
        [SerializeField] private Toggle showTimer;
        [SerializeField] private Toggle showNumOfSets;
        [SerializeField] private Toggle showHintsUsed;
        [SerializeField] private Toggle showShufflesUsed;
        [SerializeField] private Toggle autoDeal;
        [SerializeField] private Button btnResetGame;
        [SerializeField] private Button btnHome;
        [SerializeField] private ConfirmDialogUI confirmDialogUI;
        [SerializeField] private Fader fader;

        private void Start()
        {
            fader.EnterSceneAnimation();
            SetupToggles();
            showTimer.onValueChanged.AddListener(on =>
            {
                FirebaseAnalytics.LogEvent($"settings_show_timer_set_{(on ? "on" : "off")}");
                Settings.Instance.SetShowTimer(on ? 1 : 0);
            });
            showNumOfSets.onValueChanged.AddListener(on =>
            {
                FirebaseAnalytics.LogEvent($"settings_num_of_sets_set_{(on ? "on" : "off")}");
                Settings.Instance.SetShowNumOfSets(on ? 1 : 0);
            });
            showHintsUsed.onValueChanged.AddListener(on =>
            {
                FirebaseAnalytics.LogEvent($"settings_show_hints_used_set_{(on ? "on" : "off")}");
                Settings.Instance.SetShowHintsUsed(on ? 1 : 0);
            });
            showShufflesUsed.onValueChanged.AddListener(on =>
            {
                FirebaseAnalytics.LogEvent($"settings_shuffles_used_set_{(on ? "on" : "off")}");
                Settings.Instance.SetShowShufflesUsed(on ? 1 : 0);
            });
            autoDeal.onValueChanged.AddListener(on =>
            {
                FirebaseAnalytics.LogEvent($"settings_auto_deal_set_{(on ? "on" : "off")}");
                Settings.Instance.SetAutoDeal(on ? 1 : 0);
            });
            
            btnResetGame.onClick.AddListener(ShowConfirmationPopup);
            btnHome.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "SettingsScene"), 
                    new Parameter("to", "MainMenu"));
                fader.ExitSceneAnimation("MainMenu");
            });
        }

        private void SetupToggles()
        {
            showTimer.isOn = Settings.Instance.GetShowTimer();
            showNumOfSets.isOn = Settings.Instance.GetShowNumOfSets();
            showHintsUsed.isOn = Settings.Instance.GetShowHintsUsed();
            showShufflesUsed.isOn = Settings.Instance.GetShowShufflesUsed();
            autoDeal.isOn = Settings.Instance.GetAutoDeal();
        }

        private void ResetGame()
        {
            FirebaseAnalytics.LogEvent("reset_game");
            Settings.Instance.ClearSettings();
            // Reload toggles
            SetupToggles();
            File.Delete(Application.persistentDataPath + "/achievements.json");
            File.Delete(Application.persistentDataPath + "/userStatistics.json");
        }

        private void ShowConfirmationPopup()
        {
            FirebaseAnalytics.LogEvent("open_reset_game_dialog");
            confirmDialogUI.gameObject.SetActive(true);
            confirmDialogUI
                .SetTitle("Reset game")
                .SetMessage(
                    "Are you sure you want reset all your game data? " +
                    "This action cannot be undone and will reset your settings, achievements and statistics back to default.")
                .SetNegativeButtonText("Yes, clear everything")
                .SetPositiveButtonText("No, keep my data")
                .SetButtonsColor(DialogButtonColor.Green)
                .SetFadeDuration(0.1f)
                .OnNegativeButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("reset_game_dialog_click_reset");
                    ResetGame();
                })
                .OnPositiveButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("reset_game_dialog_click_cancel");
                })
                .OnCloseButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("close_reset_game_dialog");
                })
                .Show();
        }
    }
}