using UnityEngine;
using UnityEngine.UI;

namespace Achievements.CreateAchievement
{
    public class CreateAchievementManager : MonoBehaviour
    {
        public static CreateAchievementManager Instance { get; private set; }
        [SerializeField] private StepperUI stepperUI;
        [SerializeField] private Button btnNextStep;
        public int CurrentStep { get; set; }
        
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
        
        // Start is called before the first frame update
        void Start()
        {
            CurrentStep = 1;
            stepperUI.MoveToStep(CurrentStep);
            btnNextStep.onClick.AddListener(MoveToNextStep);
        }

        public void MoveToNextStep()
        {
            CurrentStep++;
            stepperUI.MoveToStep(CurrentStep);
        }
    }
}
