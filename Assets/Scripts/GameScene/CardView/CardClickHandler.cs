using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using GameScene;
using GameScene.CardView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

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
        if (clickedCard == null)
        {
            return;
        }

        if (CurrentGame.ThreeClicked())
        {
            CurrentGame.RemoveSelectionsOnCards();
        }

        if (clickedCard.IsSelected) {
            clickedCard.Select(SelectType.NONE);
            CurrentGame.Set.RemoveFromSet(clickedCard.Card);
            CurrentGame.ClickedCards.Remove(clickedCard);
        } else {
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
