using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public static class GameUtils
{
    public static List<CardView> InstantiateCardViews(List<SetCard> cardList, CardView cardPrefab)
    {
        List<CardView> newCardViews = new();
        foreach (var setCard in cardList)
        {
            newCardViews.Add(InstantiateCardView(setCard, cardPrefab));
        }

        return newCardViews;
    }

    public static CardView InstantiateCardView(SetCard card, CardView cardPrefab)
    {
        var cardView = Object.Instantiate(cardPrefab);
        cardView.transform.position = new Vector2(-100, 100);
        cardView.Card = card;
        return cardView;
    }
}