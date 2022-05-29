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
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        btnPlay.onClick.AddListener(() => SceneChanger.Instance.LoadScene("GameScene"));
        btnTutorial.onClick.AddListener(() => SceneChanger.Instance.LoadScene("TutorialScene"));
        btnAchievements.onClick.AddListener(() => SceneChanger.Instance.LoadScene("AchievementsScene"));
        btnSettings.onClick.AddListener(() => SceneChanger.Instance.LoadScene("SettingsScene"));
        btnStatistics.onClick.AddListener(() => SceneChanger.Instance.LoadScene("StatisticsScene"));
    }
}
