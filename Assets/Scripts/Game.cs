using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Game
    {
        private List<SetCard> AllCards { get; set; }
        public List<SetCard> ActualCards { get; set; }

        private List<CardView> cardViews;

        // private Bitmap sourceImage;
        // private GridLayout gridLayout;
        public Set Set { get; set; }

        // private int score;
        public List<CardView> Clicked { get; set; }

        // private SharedPreferences prefs;
        // private ViewGroup layoutSets;
        // private RelativeLayout rootLayout;
        // private ImageView ivSets;
        // private ImageView cardBackPlaceholder;
        // private int[] rootCenter = new int[2];
        
        private CardView CardPrefab { get; set; }

        public Game(CardView cardPrefab)
        {
            AllCards = new List<SetCard>();
            ActualCards = new List<SetCard>();
            cardViews = new List<CardView>();
            Set = new Set();
            Clicked = new List<CardView>();
            CardPrefab = cardPrefab;

            // TODO: add card back
            // this.cardBackPlaceholder = cardBackPlaceholder;
            // TODO: needed to save a game in progress
            // prefs = PreferenceManager.getDefaultSharedPreferences(context);
        }

        public void StartNewGame()
        {
            // score = 0;
            Deck deck = new Deck();
            AllCards = deck.CreateDeck();
            // TODO: later, instead of 12, gridLayoutColumns*gridLayoutRows
            ActualCards = deck.CreateCardsToPlay(AllCards, 12);
            DrawCards(ActualCards);

            // prefs.edit().putBoolean("gameInProgress", true).apply();
        }

        public void AddToSet(SetCard card)
        {
            Set.AddToSet(card);
        }

        private void InsertCardToIndex(int i)
        {
            if (ActualCards.Count < 12)
            {
                if (AllCards.Count > 0)
                {
                    ActualCards.Insert(i, AllCards[0]);
                    AllCards.RemoveAt(0);
                }
            }
        }

        public void GetRandomThreeCards()
        {
            if (ActualCards.Count < 21 && AllCards.Count > 0)
            {
                // if (findSet() != null) {
                //     decreaseScore(50);
                // }

                for (int i = 0; i < 3; i++)
                {
                    ActualCards.Add(AllCards[0]);
                    AllCards.RemoveAt(0);
                }

                DrawCards(ActualCards);
            }
        }

        public bool IsSetClicked()
        {
            bool ret = false;

            HashSet<SetCard> three = Set.GetSet();

            if (Set.IsSet())
            {
                foreach (SetCard card in three)
                {
                    int i = ActualCards.IndexOf(card);
                    ActualCards.Remove(card);
                    InsertCardToIndex(i);
                    ret = true;
                }
                Debug.Log("SET found!");
                Debug.Log("Remaining cards: " + AllCards.Count);
                // increaseScore(100);
            }
            else
            {
                Debug.Log("Not a SET!");
                bool isThereASet = FindSet() != null;
                Debug.Log("Is there a SET on the table: " + isThereASet);
                // decreaseScore(50);
                ret = false;
            }

            three.Clear();
            return ret;
        }

        public void SelectHint()
        {
            List<CardView> set = FindSet();
            if (set == null)
            {
                GetRandomThreeCards();
            }
            else
            {
                // decreaseScore(100);
                foreach (CardView cv in set)
                {
                    cv.Select(SelectType.HINT);
                }
                // ((GameActivity) context).Update();
            }
        }

        public void DrawCards(List<SetCard> cardList)
        {
            // gridLayout.removeAllViews();
            ClearCardViews();
            // gridLayout.setColumnCount(cardList.size() / 3);
            // gridLayout.setOrientation(GridLayout.VERTICAL);

            for (var i = 0; i < cardList.Count; i++)
            {
                var setCard = cardList[i];
                var cardView = Object.Instantiate(CardPrefab);
                cardView.Card = setCard;
                var x = (i % 4 - 1.5f) * (cardView.GetComponent<BoxCollider2D>().bounds.size.x + 0.5f);
                var y = (i / 4 - 1) * (cardView.GetComponent<BoxCollider2D>().bounds.size.y + 0.5f);
                cardView.transform.position = new Vector2(x, y);
                cardViews.Add(cardView);
            }
        }

        private void ClearCardViews()
        {
            // Clear cardViews list
            cardViews.Clear();
            // Remove all card gameobjects
            var objects = Object.FindObjectsOfType<CardView>();
            foreach (var cardView in objects)
            {
                Object.Destroy(cardView.gameObject);
            }
        }

        public void RemoveSelectionsOnCards() {
            if (Set.GetSize() == 0) {
                foreach(CardView cv in cardViews) {
                    cv.Select(SelectType.NONE);
                }
            }
        }
        
        public List<CardView> FindSet() {
            Set hintSet = new Set();
            for (int i = 0; i < cardViews.Count; i++) {
                for (int j = i + 1; j < cardViews.Count; j++) {
                    for (int k = j + 1; k < cardViews.Count; k++) {

                        hintSet.AddToSet(cardViews[i].Card);
                        hintSet.AddToSet(cardViews[j].Card);
                        hintSet.AddToSet(cardViews[k].Card);

                        if (hintSet.IsSet()) {
                            List<CardView> foundSet = new List<CardView>();
                            foundSet.Add(cardViews[i]);
                            foundSet.Add(cardViews[j]);
                            foundSet.Add(cardViews[k]);

                            return foundSet;
                        }
                    }
                }
            }

            return null;
        }
    }
}