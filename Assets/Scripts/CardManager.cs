using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Notifications.Android;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;

    private static System.Random rng = new();

    // Start is called before the first frame update
    void Start()
    {
        InitiateDeck(out var starterCards, out var deck);

        for (int i = 0; i < starterCards.Count; i++)
        {
            var card = starterCards[i];
            var x = (i % 4-1.5f) * (card.GetComponent<BoxCollider2D>().bounds.size.x+0.5f);
            var y = (i / 4-1) * (card.GetComponent<BoxCollider2D>().bounds.size.y+0.5f);
            card.transform.position = new Vector2(x,y);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void InitiateDeck(out List<Card> starterCards, out List<Card> deck)
    {
        // Create whole deck
        List<Card> cardList = new();
        for (int i = 0; i < 81; i++)
        {
            var card = Instantiate(cardPrefab);
            card.cardSpriteIndex = i;
            card.transform.position = new Vector2(-100, -100);
            cardList.Add(card);
        }
        
        // Get random 12 cards
        List<Card> tableCards = new();
        for (int i = 0; i < 12; i++)
        {
            var rndIdx = rng.Next(cardList.Count);
            tableCards.Add(cardList[rndIdx]);
            cardList.RemoveAt(rndIdx);
        }
        
        starterCards = tableCards;
        deck = cardList;
    }

    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}