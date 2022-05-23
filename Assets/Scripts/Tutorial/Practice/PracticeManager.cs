using DefaultNamespace;
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
        [SerializeField] private TMP_Text txtExplanationTitle;
        [SerializeField] private TMP_Text txtExplanationBody;
        
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
            Tutorial = new Tutorial(cardPrefab, placeholderCardPrefab, centerLeftGrid, centerVerticalGrid);
            Tutorial.StartNewTutorial();
            ResetExplanationText();
            
            btnNewTutorial.onClick.AddListener(() =>
            {
                ResetExplanationText();
                Tutorial.StartNewTutorial();
            });
            btnPlay.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
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