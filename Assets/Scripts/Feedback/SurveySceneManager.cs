using System;
using EasyUI.Dialogs;
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
            fader.EnterSceneAnimation();
            btnHome.onClick.AddListener(ShowConfirmationPopup);
        }

        private void ShowConfirmationPopup()
        {
            confirmDialogUI.gameObject.SetActive(true);
            confirmDialogUI
                .SetTitle("Exit to main menu")
                .SetMessage(
                    "Are you sure you want to exit? Your progress in the survey will be lost.")
                .SetNegativeButtonText("Yes, exit")
                .SetPositiveButtonText("No, stay here")
                .SetButtonsColor(DialogButtonColor.Green)
                .SetFadeDuration(0.1f)
                .OnNegativeButtonClicked(() => fader.ExitSceneAnimation("MainMenu"))
                .Show();
        }
    }
}