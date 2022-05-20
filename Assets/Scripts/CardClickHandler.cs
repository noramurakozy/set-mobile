using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    private Game CurrentGame { get; set; }

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

        if (CurrentGame.Set.GetSize() == 0)
        {
            CurrentGame.RemoveSelectionsOnCards();
        }

        
        if (clickedCard.IsSelected) {
            clickedCard.Select(SelectType.NONE);
            CurrentGame.Set.RemoveFromSet(clickedCard.Card);
            CurrentGame.Clicked.Remove(clickedCard);
        } else {
            clickedCard.Select(SelectType.CLICK);
            CurrentGame.AddToSet(clickedCard.Card);
            CurrentGame.Clicked.Add(clickedCard);
        }
        if (CurrentGame.Set.GetSize() == 3) {
            if (CurrentGame.IsSetClicked()) {
                foreach(CardView c in CurrentGame.Clicked) {
                    c.Select(SelectType.NONE);
                    c.transform.position = new Vector2(-100, -100);
                    // TODO: move card to the corner and show back of the card
                    // c.getImage().setImageDrawable(context.getResources().getDrawable(R.drawable.card_back));
                    // translate(c.getImage(), ivSets, 1000);
                }
                CurrentGame.DrawCards(CurrentGame.ActualCards);
            }
            else
            {
                // Shake if the selected three cards do not form a SET
                foreach (CardView c in CurrentGame.Clicked)
                {
                    // c.transform.DOComplete();
                    c.transform.DOPunchRotation(new Vector3(0, 0, 2), 1);
                }
            }
            CurrentGame.RemoveSelectionsOnCards();
        }
    }
}
