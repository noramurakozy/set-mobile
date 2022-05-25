using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using Statistics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class Game
{
    public Deck Deck { get; set; }
    private List<CardView> CardsOnTable { get; set; }
    public Set Set { get; set; }
    public List<CardView> ClickedCards { get; set; }
    public GameStatistics Statistics { get; set; }

    private CardView CardPrefab { get; set; }
    private GridManager CenterGrid { get; set; }
    private Sprite CardBack { get; set; }

    private Stopwatch _stopwatch;
    public int SetsFoundCount { get; set; }

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

    public string GetStopwatchString()
    {
        var elapsedTime = _stopwatch.Elapsed;
        if (elapsedTime.Hours == 0)
        {
            return elapsedTime.ToString(@"mm\:ss");
        }

        return elapsedTime.ToString(@"hh\:mm\:ss");
    }

    public void AddToSet(SetCard card)
    {
        Set.AddToSet(card);
    }

    private (CardView, int) ReplaceCardOnIndex(int i)
    {
        var cardView = GameUtils.InstantiateCardView(Deck.GetAt(0), CardPrefab);
        // CardsOnTable[i] = cardView;
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
            // if (findSet() != null) {
            //     decreaseScore(50);
            // }
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
        return Set.IsSet();
    }

    private void DrawCardOnIndex(CardView newCard, int i)
    {
        CenterGrid.Insert(newCard, i);
    }

    public void SelectHint()
    {
        List<CardView> set = FindSetOnTable();
        if (set == null)
        {
            DealAdditionalCards(3);
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

    // public virtual void DrawCards(List<CardView> cardList)
    // {
    //     ClearCardViews();
    //     CenterGrid.Cols = cardList.Count / 3;
    //
    //     var newCardViews = InstantiateCardViews(cardList);
    //     cardViews.AddRange(newCardViews);
    //
    //     CenterGrid.GenerateGrid(new List<GameObject>(cardViews.Select(c => c.gameObject)), "center");
    // }

    // private void ClearCardViews()
    // {
    //     // Clear cardViews list
    //     cardViews.Clear();
    //     // Remove all card gameobjects
    //     var objects = Object.FindObjectsOfType<CardView>();
    //     foreach (var cardView in objects)
    //     {
    //         Object.Destroy(cardView.gameObject);
    //     }
    // }

    public void RemoveSelectionsOnCards()
    {
        foreach (CardView cv in ClickedCards)
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

    public void RearrangeActualCards()
    {
        Utils.Shuffle(CardsOnTable);
        CenterGrid.ShuffleCards(CardsOnTable);
    }

    public void EndGame()
    {
        _stopwatch.Stop();
        _stopwatch.Reset();
    }

    public void ResumeGame()
    {
        _stopwatch.Start();
    }

    public void PauseGame()
    {
        _stopwatch.Stop();
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
            Debug.Log(index);
            var (newCard, newIndex) = ReplaceCardOnIndex(index);
            Debug.Log("Insert this card " + newCard + " on this index " + newIndex);
            DrawCardOnIndex(newCard, newIndex);
        }
        // CenterGrid.KeepCards(CardsOnTable);
    }

    public void RearrangeRemainingCards()
    {
        // Debug.Log("BEFORE START+++++++++++++++++");
        // Debug.Log(CardsOnTable.Count);
        // foreach (var t in CardsOnTable)
        // {
        //     Debug.Log(t);
        // }
        // Debug.Log("BEFORE END+++++++++++++++++");
        // Returns with the cards in the new order
        var arrangedCardsOnTable = CenterGrid.KeepCards(CardsOnTable);
        CardsOnTable = arrangedCardsOnTable;
        // Debug.Log("AFTER START+++++++++++++++++");
        // Debug.Log(CardsOnTable.Count);
        // foreach (var t in CardsOnTable)
        // {
        //     Debug.Log(t);
        // }
        // Debug.Log("AFTER END+++++++++++++++++");
    }

    public void MoveSetToTargetAndDestroy()
    {
        var setCounterCards = Object.FindObjectsOfType<AnimationTarget>();
        var cardsToDestroy = new List<CardView>(ClickedCards);
        for (var i = 0; i < cardsToDestroy.Count; i++)
        {
            CardView c = cardsToDestroy[i];
            // CardsOnTable.Remove(c);
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
        // Shake if the selected three cards do not form a SET
        foreach (CardView c in ClickedCards)
        {
            // c.transform.DOComplete();
            c.transform.DOPunchRotation(new Vector3(0, 0, 2), 1);
        }
    }

    public List<int> GetIndexOfCards(List<CardView> setCards)
    {
        Debug.Log("GET INDEX OF THESE CARDS:-------------------");
        var indices = setCards.Select(card =>
        {
            Debug.Log("Card: " + card + " Found idx: " + CardsOnTable.IndexOf(card) + " Cards on table: " + CardsOnTable.Count);
            return CardsOnTable.IndexOf(card);
        }).OrderBy(i => i).ToList();

        return indices;
        // return setCards.Select(card =>
        // {
        //     var cardViewOfSetCard = CardsOnTable.FirstOrDefault(cardView => cardView.Card == card);
        //     return CardsOnTable.IndexOf(cardViewOfSetCard);
        // }).ToList();
    }

    public void RemoveCardsFromTable(List<CardView> cardsToRemove)
    {
        CardsOnTable = CardsOnTable.Except(cardsToRemove).ToList();
    }

    public bool ThreeClicked()
    {
        return ClickedCards.Count >= 3;
    }
}