using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class Deck
    {
        // creates shuffled deck
        public List<SetCard> CreateDeck() {
            int i = 0;
            List<SetCard> cards = new List<SetCard>();
            foreach(Fill itf in Enum.GetValues(typeof(Fill))) {
                foreach(Color itc in Enum.GetValues(typeof(Color))) {
                    foreach(Number itn in Enum.GetValues(typeof(Number))) {
                        foreach(Shape its in Enum.GetValues(typeof(Shape)))
                        {
                            cards.Add(new SetCard(itf, itc, itn, its, i));
                            i++;
                        }
                    }
                }
            }

            Utils.Shuffle(cards);
            return cards;
        }

        public List<SetCard> CreateCardsToPlay(List<SetCard> all, int numberOfCards) {

            List<SetCard> cards = new List<SetCard>();

            for (int i = 0; i < numberOfCards; i++) {
                cards.Add(all[0]);
                all.RemoveAt(0);
            }

            return cards;
        }
    }
}