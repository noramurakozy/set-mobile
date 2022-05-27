using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnTutorial;
    [SerializeField] private Button btnAchievements;
    
    // Start is called before the first frame update
    void Start()
    {
        btnPlay.onClick.AddListener(() => SceneChanger.Instance.LoadScene("GameScene"));
        btnTutorial.onClick.AddListener(() => SceneChanger.Instance.LoadScene("TutorialScene"));
        btnAchievements.onClick.AddListener(() => SceneChanger.Instance.LoadScene("AchievementsScene"));
    }
}
