using GameScene.CardView;
using Sound;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial.Practice
{
    public class TutorialCardClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private Tutorial CurrentTutorial { get; set; }
    
        // Start is called before the first frame update
        void Start()
        {
            CurrentTutorial = PracticeManager.Instance.Tutorial;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var clickedCard = eventData.pointerClick.GetComponent<CardView>();
            if (clickedCard == null)
            {
                return;
            }
            
            var set = CurrentTutorial.HandleOptionCardClick(clickedCard);
            if (set != null)
            {
                if (set.IsSet())
                {
                    SoundManager.Instance.PlaySound(Sound.Sound.TutorialCorrect);
                    PracticeManager.Instance.DisplaySuccessText(set);
                }
                else
                {
                    SoundManager.Instance.PlaySound(Sound.Sound.TutorialWrong);
                    PracticeManager.Instance.DisplayErrorText(set);
                }
            }
        }
    }
}
