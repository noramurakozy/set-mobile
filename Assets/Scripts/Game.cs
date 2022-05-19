using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class Game
    {
        public List<SetCard> AllCards { get; set; }
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
        private GridManager CenterGrid { get; set; }
        
        public int SetsFoundCount { get; set; }

        public Game(CardView cardPrefab, GridManager centerGrid)
        {
            AllCards = new List<SetCard>();
            ActualCards = new List<SetCard>();
            cardViews = new List<CardView>();
            Set = new Set();
            Clicked = new List<CardView>();
            CardPrefab = cardPrefab;
            CenterGrid = centerGrid;

            // TODO: add card back
            // this.cardBackPlaceholder = cardBackPlaceholder;
            // TODO: needed to save a game in progress
            // prefs = PreferenceManager.getDefaultSharedPreferences(context);
        }

        public virtual void StartNewGame()
        {
            // score = 0;
            Deck deck = new Deck();
            AllCards = deck.CreateDeck();
            ActualCards = deck.CreateCardsToPlay(AllCards, CenterGrid.Cols * CenterGrid.Rows);
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

                SetsFoundCount++;
                // increaseScore(100);
            }

            three.Clear();
            return ret;
        }

        public void SelectHint()
        {
            List<CardView> set = FindSetInViews();
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

        public virtual void DrawCards(List<SetCard> cardList)
        {
            ClearCardViews();
            CenterGrid.Cols = cardList.Count / 3;

            for (var i = 0; i < cardList.Count; i++)
            {
                var setCard = cardList[i];
                var cardView = Object.Instantiate(CardPrefab);
                cardView.Card = setCard;
                cardViews.Add(cardView);
            }

            CenterGrid.GenerateGrid(new List<GameObject>(cardViews.Select(c => c.gameObject)), "center");
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

        private List<CardView> FindSetInViews() {
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

        public void RearrangeActualCards() {
            Utils.Shuffle(ActualCards);
            DrawCards(ActualCards);
        }
    }
}