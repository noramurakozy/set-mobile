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
            
            CurrentTutorial.HandleOptionCardClick(clickedCard);
        }
    }
}
