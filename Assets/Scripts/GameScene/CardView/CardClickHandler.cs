using DefaultNamespace;
using Firebase.Analytics;
using FirebaseHandlers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScene.CardView
{
    public class CardClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private Game CurrentGame { get; set; }
        // [SerializeField] private Sprite cardBack;

        private void Start()
        {
            CurrentGame = GameManager.Instance.Game;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            // check if null
            var clickedCard = eventData.pointerClick.GetComponent<CardView>();
            FirebaseAnalytics.LogEvent("card_clicked", 
                new Parameter("type", clickedCard.ToString()),
                new Parameter("index", clickedCard.Card.Index)
                );
            if (clickedCard == null)
            {
                return;
            }

            if (CurrentGame.ThreeClicked())
            {
                CurrentGame.RemoveSelectionsOnCards();
            }

            if (clickedCard.IsSelected) {
                FirebaseAnalytics.LogEvent("card_clicked_deselect", 
                    new Parameter("type", clickedCard.ToString()),
                    new Parameter("index", clickedCard.Card.Index)
                    );
                clickedCard.Select(SelectType.NONE);
                CurrentGame.Set.RemoveFromSet(clickedCard.Card);
                CurrentGame.ClickedCards.Remove(clickedCard);
            } else {
                FirebaseAnalytics.LogEvent("card_clicked_select", 
                    new Parameter("type", clickedCard.ToString()),
                    new Parameter("index", clickedCard.Card.Index)
                    );
                clickedCard.Select(SelectType.CLICK);
                CurrentGame.AddToSet(clickedCard.Card);
                CurrentGame.ClickedCards.Add(clickedCard);
            }

            if (CurrentGame.Set.GetSize() == 3) 
            {
                if (CurrentGame.IsSetClicked())
                {
                    var emptyCardSlots = CurrentGame.GetIndexOfCards(CurrentGame.ClickedCards);
                    CurrentGame.MoveSetToTargetAndDestroy();
                    CurrentGame.RemoveCardsFromTable(CurrentGame.ClickedCards);
                
                    if (CurrentGame.AreNewCardsNeeded())
                    {
                        CurrentGame.DealNewCardsAt(emptyCardSlots);
                    }
                    else
                    {
                        CurrentGame.RemoveCardsFromTable(CurrentGame.ClickedCards);
                        CurrentGame.RearrangeRemainingCards();
                    }
                }
                else
                {
                    CurrentGame.InvalidSetSelected();
                }
                CurrentGame.RemoveSelectionsOnCards();
            }
        }
    }
}
