using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using GameScene.Statistics;
using UnityEngine;
using UnityEngine.Rendering;
using UpdateType = Achievements.AchievementTypes.UpdateType;

namespace GameScene
{
    public class Game
    {
        public Deck Deck { get; set; }
        public Set Set { get; set; }
        private List<CardView> CardsOnTable { get; set; }
        public List<CardView> ClickedCards { get; set; }
        private GridManager CenterGrid { get; set; }
        private CardView CardPrefab { get; set; }
        private Sprite CardBack { get; set; }

        private Stopwatch _stopwatch;
        public GameStatistics Statistics { get; set; }
        
        public Game(CardView cardPrefab, Sprite cardBack, GridManager centerGrid)
        {
            CardsOnTable = new List<CardView>();
            Set = new Set();
            ClickedCards = new List<CardView>();
            CardPrefab = cardPrefab;
            CenterGrid = centerGrid;
            CardBack = cardBack;
            _stopwatch = new Stopwatch();
            Statistics = new GameStatistics();

            // TODO: needed to save a game in progress
            // prefs = PreferenceManager.getDefaultSharedPreferences(context);
        }

        public void StartNewGame()
        {
            Deck = new Deck();
            var starterSetCards = Deck.CreateCardsToPlay(CenterGrid.cols * CenterGrid.rows);
            CardsOnTable = GameUtils.InstantiateCardViews(starterSetCards, CardPrefab);
            AnimateCardsIntoGridFromDeck(CardsOnTable);
            _stopwatch.Start();

            // prefs.edit().putBoolean("gameInProgress", true).apply();
        }

        private void AnimateCardsIntoGridFromDeck(List<CardView> cardsToAnimate)
        {
            CenterGrid.cols = cardsToAnimate.Count / 3;
            CenterGrid.GenerateGrid(cardsToAnimate, "center");
        }

        public void AddToSet(SetCard card)
        {
            Set.AddToSet(card);
        }

        private (CardView, int) ReplaceCardOnIndex(int i)
        {
            var cardView = GameUtils.InstantiateCardView(Deck.GetAt(0), CardPrefab);
            if (i >= CardsOnTable.Count)
            {
                CardsOnTable.Add(cardView);
            }
            else
            {
                CardsOnTable.Insert(i, cardView);
            }
            Deck.RemoveAt(0);

            return (CardsOnTable[i], CardsOnTable.IndexOf(cardView));
        }

        public void DealAdditionalCards(int numberOfCards)
        {
            if (CardsOnTable.Count < 21 && !Deck.IsEmpty())
            {
                var newCards = new List<CardView>();
                for (int i = 0; i < numberOfCards; i++)
                {
                    var cardView = GameUtils.InstantiateCardView(Deck.GetAt(0), CardPrefab);
                    CardsOnTable.Add(cardView);
                    Deck.RemoveAt(0);
                    newCards.Add(cardView);
                }
                InsertCardViewsInGridColumn(newCards, CenterGrid.cols);
            }
        }

        private void InsertCardViewsInGridColumn(List<CardView> cardViews, int colIndex)
        {
            CenterGrid.InsertInColumn(cardViews, colIndex);
        }

        public bool IsSetClicked()
        {
            if (Set.IsSet())
            {
                Statistics.SetsFound++;
                Statistics.MaxSetsFoundInARow++;
                Statistics.LastSetFound = Set;
                Statistics.CurrentElapsedSeconds = (int)_stopwatch.Elapsed.TotalSeconds;
                switch (Set.DiffPropsCount)
                {
                    case 1:
                        Statistics.NumSets1DiffProp++;
                        break;
                    case 2:
                        Statistics.NumSets2DiffProp++;
                        break;
                    case 3:
                        Statistics.NumSets3DiffProp++;
                        break;
                    case 4:
                        Statistics.NumSets4DiffProp++;
                        break;
                }
                GameManager.Instance.UpdateAchievementProgresses(Statistics, UpdateType.DuringGame);
                GameManager.Instance.EnableHintBtn(true);
                return true;
            }

            return false;
        }

        private void DrawCardOnIndex(CardView newCard, int i)
        {
            CenterGrid.Insert(newCard, i);
        }

        public void SelectHint()
        {
            Statistics.HintsUsed++;
            List<CardView> set = FindSetOnTable();
            if (set == null)
            {
                DealAdditionalCards(3);
            }
            else
            {
                foreach (CardView cv in set)
                {
                    cv.Select(SelectType.HINT);
                }

                GameManager.Instance.EnableHintBtn(false);
            }
        }

        public void RemoveSelectionsOnCards()
        {
            foreach (CardView cv in CardsOnTable)
            {
                cv.Select(SelectType.NONE);
            }

            ClickedCards.Clear();
        }

        private List<CardView> FindSetOnTable()
        {
            Set hintSet = new Set();
            for (int i = 0; i < CardsOnTable.Count; i++)
            {
                for (int j = i + 1; j < CardsOnTable.Count; j++)
                {
                    for (int k = j + 1; k < CardsOnTable.Count; k++)
                    {
                        hintSet.AddToSet(CardsOnTable[i].Card);
                        hintSet.AddToSet(CardsOnTable[j].Card);
                        hintSet.AddToSet(CardsOnTable[k].Card);

                        if (hintSet.IsSet())
                        {
                            List<CardView> foundSet = new List<CardView>();
                            foundSet.Add(CardsOnTable[i]);
                            foundSet.Add(CardsOnTable[j]);
                            foundSet.Add(CardsOnTable[k]);

                            return foundSet;
                        }
                    }
                }
            }

            return null;
        }
        
        public int GetNumOfSetsOnTable()
        {
            List<Set> hintSetList = new List<Set>();
            for (int i = 0; i < CardsOnTable.Count; i++)
            {
                Set hintSet = new Set();
                for (int j = i + 1; j < CardsOnTable.Count; j++)
                {
                    for (int k = j + 1; k < CardsOnTable.Count; k++)
                    {
                        hintSet.AddToSet(CardsOnTable[i].Card);
                        hintSet.AddToSet(CardsOnTable[j].Card);
                        hintSet.AddToSet(CardsOnTable[k].Card);

                        if (hintSet.IsSet())
                        {
                            hintSetList.Add(hintSet);
                        }
                    }
                }
            }

            return hintSetList.Count;
        }

        public void RearrangeActualCards()
        {
            Utils.Shuffle(CardsOnTable);
            CenterGrid.ShuffleCards(CardsOnTable);
            Statistics.ShufflesUsed++;
        }

        public void ResumeGame()
        {
            _stopwatch.Start();
        }

        public void PauseGame()
        {
            _stopwatch.Stop();
        }

        public GameStatistics EndGame()
        {
            _stopwatch.Stop();
            Statistics.DurationInSeconds = (int)_stopwatch.Elapsed.TotalSeconds;
            _stopwatch.Reset();
            return Statistics;
        }

        private void DestroyCards(List<CardView> toDestroy)
        {
            // Destroy their gameobjects in the scene
            foreach (var cardView in toDestroy)
            {
                Object.Destroy(cardView.gameObject);
            }
        }

        public bool AreNewCardsNeeded()
        {
            return CardsOnTable.Count < 12 && !Deck.IsEmpty();
        }

        public void DealNewCardsAt(List<int> emptyCardSlots)
        {
            foreach (int index in emptyCardSlots)
            {
                var (newCard, newIndex) = ReplaceCardOnIndex(index);
                DrawCardOnIndex(newCard, newIndex);
            }
        }

        public void RearrangeRemainingCards()
        {
            var arrangedCardsOnTable = CenterGrid.KeepCards(CardsOnTable);
            CardsOnTable = arrangedCardsOnTable;
        }

        public void MoveSetToTargetAndDestroy()
        {
            var setCounterCards = Object.FindObjectsOfType<AnimationTarget>();
            var cardsToDestroy = new List<CardView>(ClickedCards);
            for (var i = 0; i < cardsToDestroy.Count; i++)
            {
                CardView c = cardsToDestroy[i];
                c.Select(SelectType.NONE);
                c.GetComponent<SpriteRenderer>().sprite = CardBack;
                c.GetComponent<SortingGroup>().sortingOrder +=
                    setCounterCards[i].GetComponent<AnimationTarget>().targetSortingOrder;
                c.transform.DORotateQuaternion(setCounterCards[i].transform.rotation, 0.5f);
                var moveCardTween =
                    c.transform.DOMove(
                        new Vector2(setCounterCards[i].transform.position.x,
                            setCounterCards[i].transform.position.y), 1);
                moveCardTween.onComplete += () => DestroyCards(cardsToDestroy);

            }

            Set.ClearSet();
        }

        public void InvalidSetSelected()
        {
            Statistics.MistakesCount++;
            Statistics.MaxSetsFoundInARow = 0;
            // Shake if the selected three cards do not form a SET
            foreach (CardView c in ClickedCards)
            {
                c.transform.DOPunchRotation(new Vector3(0, 0, 2), 1);
            }
        }

        public List<int> GetIndexOfCards(List<CardView> setCards)
        {
            var indices = setCards.Select(card => CardsOnTable.IndexOf(card)).OrderBy(i => i).ToList();

            return indices;
        }

        public void RemoveCardsFromTable(List<CardView> cardsToRemove)
        {
            CardsOnTable = CardsOnTable.Except(cardsToRemove).ToList();
        }

        public bool ThreeClicked()
        {
            return ClickedCards.Count >= 3;
        }

        public bool IsGameEnded()
        {
            return Deck.IsEmpty() && FindSetOnTable() == null;
        }

        public string GetTimerString()
        {
            return Utils.GetTimeSpanString(_stopwatch.Elapsed);
        }
    }
}