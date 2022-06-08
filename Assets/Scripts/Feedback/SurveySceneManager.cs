using System;
using EasyUI.Dialogs;
using Firebase.Analytics;
using FirebaseHandlers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Feedback
{
    public class SurveySceneManager : MonoBehaviour
    {
        [SerializeField] private Button btnHome;
        [SerializeField] private ConfirmDialogUI confirmDialogUI;
        [SerializeField] private Fader fader;

        private void Start()
        {
            FirebaseAnalytics.LogEvent("enter_survey_scene", 
                new Parameter("custom_achievements", RemoteConfigValueManager.Instance.CustomAchievements.ToString()));
            fader.EnterSceneAnimation();
            btnHome.onClick.AddListener(ShowConfirmationPopup);
        }

        private void ShowConfirmationPopup()
        {
            FirebaseAnalytics.LogEvent("experiment_survey_exit_confirm_open");
            confirmDialogUI.gameObject.SetActive(true);
            confirmDialogUI
                .SetTitle("Exit to main menu")
                .SetMessage(
                    "Are you sure you want to exit? Your progress in the survey will be lost.")
                .SetNegativeButtonText("Yes, exit")
                .SetPositiveButtonText("No, stay here")
                .SetButtonsColor(DialogButtonColor.Green)
                .SetFadeDuration(0.1f)
                .OnNegativeButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("experiment_survey_confirm_exit");
                    FirebaseAnalytics.LogEvent("switch_scene",
                        new Parameter("from", "SurveyScene"), 
                        new Parameter("to", "MainScene"));
                    fader.ExitSceneAnimation("MainMenu");
                })
                .OnPositiveButtonClicked(() =>
                {
                    FirebaseAnalytics.LogEvent("experiment_survey_confirm_stay");
                })
                .Show();
        }
    }
}