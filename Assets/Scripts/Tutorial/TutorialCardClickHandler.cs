using DefaultNamespace;
using DefaultNamespace.Tutorial;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial
{
    public class TutorialCardClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private Tutorial CurrentTutorial { get; set; }
    
        // Start is called before the first frame update
        void Start()
        {
            CurrentTutorial = TutorialManager.Instance.Tutorial;
        }

        // Update is called once per frame
        void Update()
        {
        
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
