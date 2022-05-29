using GameScene.CardView;
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
                    PracticeManager.Instance.DisplaySuccessText(set);
                }
                else
                {
                    PracticeManager.Instance.DisplayErrorText(set);
                }
            }
        }
    }
}
