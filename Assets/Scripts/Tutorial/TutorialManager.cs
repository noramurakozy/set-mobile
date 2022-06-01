using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        [SerializeField] private Button btnPractice;
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
                fader.ExitSceneAnimation("PracticeScene");
            });
        }
    }
}