using DG.Tweening;
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
    }
}
