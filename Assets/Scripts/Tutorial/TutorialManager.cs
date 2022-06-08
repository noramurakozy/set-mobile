using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        [SerializeField] private Button btnPractice;
        [SerializeField] private Button btnHome;
        [SerializeField] private Fader fader;
        
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
            btnPractice.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "TutorialScene"), 
                    new Parameter("to", "PracticeScene"));
                fader.ExitSceneAnimation("PracticeScene");
            });
            btnHome.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "TutorialScene"), 
                    new Parameter("to", "MainMenu"));
                fader.ExitSceneAnimation("MainMenu");
            });
        }
    }
}