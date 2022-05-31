using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using GameScene.GUtils;
using Newtonsoft.Json;
using UnityEngine;
using Color = DefaultNamespace.Color;

public class Deck
{
    [JsonProperty]
    private List<SetCard> _cards;

    public int Count => _cards.Count;

    // creates shuffled deck
    public void CreateDeck()
    {
        _cards = new List<SetCard>();
        int i = 0;
        // List<SetCard> cards = new List<SetCard>();
        foreach(Fill itf in Enum.GetValues(typeof(Fill))) {
            foreach(Color itc in Enum.GetValues(typeof(Color))) {
                foreach(Number itn in Enum.GetValues(typeof(Number))) {
                    foreach(Shape its in Enum.GetValues(typeof(Shape)))
                    {
                        _cards.Add(new SetCard(itf, itc, itn, its, i));
                        i++;
                    }
                }
            }
        }

        Utils.Shuffle(_cards);
        // return _cards;
    }

    public List<SetCard> CreateCardsToPlay(int numberOfCards) {

        List<SetCard> cards = new List<SetCard>();

        for (int i = 0; i < numberOfCards; i++) {
            cards.Add(_cards[0]);
            _cards.RemoveAt(0);
        }

        return cards;
    }

    public bool IsEmpty()
    {
        return _cards.Count <= 0;
    }

    public SetCard GetAt(int i)
    {
        return _cards[i];
    }

    public void RemoveAt(int i)
    {
        _cards.RemoveAt(i);
    }

    public List<SetCard> FindSetInCards()
    {
        Set hintSet = new Set();
        for (int i = 0; i < _cards.Count; i++)
        {
            for (int j = i + 1; j < _cards.Count; j++)
            {
                for (int k = j + 1; k < _cards.Count; k++)
                {
                    hintSet.AddToSet(_cards[i]);
                    hintSet.AddToSet(_cards[j]);
                    hintSet.AddToSet(_cards[k]);

                    if (hintSet.IsSet())
                    {
                        List<SetCard> foundSet = new List<SetCard>
                        {
                            _cards[i],
                            _cards[j],
                            _cards[k]
                        };

                        return foundSet;
                    }
                }
            }
        }

        return null;
    }

    public void RemoveCards(List<SetCard> cardsToRemove)
    {
        _cards = _cards.Except(cardsToRemove).ToList();
    }
}