using DG.Tweening;
using EasyUI.Dialogs;
using Feedback;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnTutorial;
    [SerializeField] private Button btnAchievements;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnStatistics;
    [SerializeField] private Button btnAbout;
    [SerializeField] private Button btnExperimentInfo;
    
    [SerializeField] private ConfirmDialogUI confirmDialogUI;
    [SerializeField] private GFormFeedbackManager gFormFeedbackManager;
    [SerializeField] private Fader fader;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        
        fader.EnterSceneAnimation();
        btnPlay.onClick.AddListener(() =>
        {
            fader.ExitSceneAnimation("GameScene");
        });
        btnTutorial.onClick.AddListener(() =>
        {
            fader.ExitSceneAnimation("TutorialScene");
        });
        btnAchievements.onClick.AddListener(() =>
        {
            fader.ExitSceneAnimation("AchievementsScene");
        });
        btnSettings.onClick.AddListener(() =>
        {
            fader.ExitSceneAnimation("SettingsScene");
        });
        btnStatistics.onClick.AddListener(() =>
        {
            fader.ExitSceneAnimation("StatisticsScene");
        });
        btnAbout.onClick.AddListener(ShowAboutPopup);
        btnExperimentInfo.onClick.AddListener(ShowExperimentInfo);
    }

    private void ShowAboutPopup()
    {
        confirmDialogUI.gameObject.SetActive(true);
        confirmDialogUI
            .SetTitle("About")
            .SetMessage("This application was created based on the rules of the official card game SET® published by SET Enterprises." +
                        "\n" +
                        "If you found a bug, please report it through Google Play Store or App Store." +
                        "\n" +
                        "Have fun playing and don't forget to seek out for new challenges! :)")
            .SetNegativeButtonText("Give general feedback")
            .SetPositiveButtonText("Report a bug")
            .OnNegativeButtonClicked(ShowFeedbackDialog)
            .OnPositiveButtonClicked(ShowBugDialog)
            .SetButtonsColor(DialogButtonColor.Red)
            .SetFadeDuration(0.1f)
            .Show();
    }

    private void ShowFeedbackDialog()
    {
        confirmDialogUI.gameObject.SetActive(true);
        var feedbackQuestion = confirmDialogUI.GetComponentInChildren<GFormQuestion>(true);
        feedbackQuestion.entryID = "entry.1798791499";
        confirmDialogUI
            .SetTitle("Give feedback")
            .SetMessage("Here you can send me feedback about the game in general, feature ideas/improvement points if you have any.")
            .SetNegativeButtonText("Cancel")
            .SetPositiveButtonText("Send")
            .SetInputFieldVisibility()
            .OnPositiveButtonClicked(() =>
            {
                feedbackQuestion.Answer = feedbackQuestion.GetComponent<TMP_InputField>().text;
                gFormFeedbackManager.SendFeedback(feedbackQuestion);
                ShowFeedbackSuccessDialog();
            })
            .SetFadeDuration(0.1f)
            .Show();
    }

    private void ShowBugDialog()
    {
        confirmDialogUI.gameObject.SetActive(true);
        var bugQuestion = confirmDialogUI.GetComponentInChildren<GFormQuestion>(true);
        bugQuestion.entryID = "entry.1798791499";
        confirmDialogUI
            .SetTitle("Report a bug")
            .SetMessage("Please describe as many details as possible about the bug. The more I know about the circumstances, the faster the bug can be solved.")
            .SetNegativeButtonText("Cancel")
            .SetPositiveButtonText("Send")
            .SetInputFieldVisibility()
            .OnPositiveButtonClicked(() =>
            {
                bugQuestion.Answer = bugQuestion.GetComponent<TMP_InputField>().text;
                gFormFeedbackManager.SendBugReport(bugQuestion);
                ShowBugSuccessDialog();
            })
            .SetFadeDuration(0.1f)
            .Show();
    }

    private void ShowBugSuccessDialog()
    {
        confirmDialogUI.gameObject.SetActive(true);
        confirmDialogUI
            .SetTitle("Report a bug")
            .SetMessage("Thank you! The bug has been reported and will be fixed as soon as possible.")
            .SetButtonsVisibility(false)
            .SetInputFieldVisibility(false)
            .SetFadeDuration(0.1f)
            .Show();
    }
    
    private void ShowFeedbackSuccessDialog()
    {
        confirmDialogUI.gameObject.SetActive(true);
        confirmDialogUI
            .SetTitle("Give feedback")
            .SetMessage("Thank you! Your feedback has been sent.")
            .SetButtonsVisibility(false)
            .SetInputFieldVisibility(false)
            .SetFadeDuration(0.1f)
            .Show();
    }

    private void ShowExperimentInfo()
    {
        confirmDialogUI.gameObject.SetActive(true);
        confirmDialogUI
            .SetTitle("Experiment info")
            .SetMessage("If you're looking for the survey, you are at the right place! " +
                        "You can fill it out by clicking on the button below. \n" +
                        "Please note, you can give the most valuable feedback if you have used this app for at least 10 minutes already! " +
                        "If that's not true, please continue playing for a bit more :)\n" +
                        "Thank you for your help again! I hope you had a great time playing :)\n" +
                        "Tip: It's enough to fill out the survey once! ;)") 
            .SetNegativeButtonText("I'll do it later")
            .SetPositiveButtonText("Go to the survey")
            .OnPositiveButtonClicked(() =>
            {
                fader.ExitSceneAnimation("SurveyScene");
            })
            .SetFadeDuration(0.1f)
            .Show();
    }
}
