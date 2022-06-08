using DefaultNamespace;
using Firebase.Analytics;
using GameScene.CardView;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace Tutorial.Practice
{
    public class PracticeManager : MonoBehaviour
    {
        public static PracticeManager Instance { get; private set; }
        
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private SpriteRenderer placeholderCardPrefab;
        [SerializeField] private TutorialGridManager centerLeftGrid;
        [SerializeField] private TutorialGridManager centerVerticalGrid;
        [SerializeField] private Button btnNewTutorial;
        [SerializeField] private Button btnPlay;
        [SerializeField] private Button btnBackToRules;
        [SerializeField] private TMP_Text txtExplanationTitle;
        [SerializeField] private TMP_Text txtExplanationBody;
        [SerializeField] private Fader fader;
        
        public Tutorial Tutorial { get; set; }
        
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
            Tutorial = new Tutorial(cardPrefab, placeholderCardPrefab, centerLeftGrid, centerVerticalGrid);
            FirebaseAnalytics.LogEvent("start_new_tutorial");
            Tutorial.StartNewTutorial();
            ResetExplanationText();
            
            btnNewTutorial.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("get_another_tutorial");
                ResetExplanationText();
                Tutorial.StartNewTutorial();
            });
            btnPlay.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "PracticeScene"), 
                    new Parameter("to", "GameScene"));
                fader.ExitSceneAnimation("GameScene");
            });
            btnBackToRules.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "PracticeScene"), 
                    new Parameter("to", "TutorialScene"));
                fader.ExitSceneAnimation("TutorialScene");
            });
        }

        private void ResetExplanationText()
        {
            txtExplanationTitle.enabled = false;
            txtExplanationBody.text = "Guess which card completes the SET!";
        }

        public void DisplaySuccessText(Set set)
        {
            txtExplanationTitle.enabled = true;
            txtExplanationTitle.text = FeedbackUtils.GetRandomPositiveFeedback();
            txtExplanationTitle.text = txtExplanationTitle.text.ToUpper();
            txtExplanationTitle.color = Color.green;
            
            txtExplanationBody.text = set.GetReason();
        }

        public void DisplayErrorText(Set set)
        {
            txtExplanationTitle.enabled = true;
            txtExplanationTitle.text = FeedbackUtils.GetRandomNegativeFeedback();
            txtExplanationTitle.text = txtExplanationTitle.text.ToUpper();
            txtExplanationTitle.color = Color.red;
            
            txtExplanationBody.text = set.GetReason();
        }
    }
}