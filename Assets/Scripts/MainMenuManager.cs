using DG.Tweening;
using EasyUI.Dialogs;
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

    [SerializeField] private ConfirmDialogUI confirmDialogUI;
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
            .SetButtonsVisibility(false)
            .SetFadeDuration(0.1f)
            .Show();
    }
}
