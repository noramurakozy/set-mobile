using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using Firebase.Analytics;
using GameScene.GUtils;
using GameScene.Statistics;
using Newtonsoft.Json;
using Statistics;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
using UpdateType = Achievements.AchievementTypes.UpdateType;

namespace GameScene
{
    public class Game
    {
        public Deck Deck { get; set; }
        public Set Set { get; set; }
        private List<CardView.CardView> CardsOnTable { get; set; }
        public List<CardView.CardView> ClickedCards { get; set; }
        private GridManager CenterGrid { get; set; }
        private CardView.CardView CardPrefab { get; set; }
        private Sprite CardBack { get; set; }

        // private Stopwatch _stopwatch;
        private GameStopwatch _gameStopwatch;

        // public bool IsGameRunning { get; set; }

        public Game(CardView.CardView cardPrefab, Sprite cardBack, GridManager centerGrid, GameStopwatch gameStopwatch)
        {
            CardsOnTable = new List<CardView.CardView>();
            Set = new Set();
            ClickedCards = new List<CardView.CardView>();
            CardPrefab = cardPrefab;
            CenterGrid = centerGrid;
            CardBack = cardBack;
            _gameStopwatch = gameStopwatch;
        }

        public void StartNewGame()
        {
            _gameStopwatch.ResetStopwatch();
            // IsGameRunning = true;
            Deck = new Deck();
            Deck.CreateDeck();
            CenterGrid.cols = 4;
            CenterGrid.rows = 3;
            var starterSetCards = Deck.CreateCardsToPlay(CenterGrid.cols * CenterGrid.rows);
            DestroyCards(CardsOnTable);
            CardsOnTable = GameUtils.InstantiateCardViews(starterSetCards, CardPrefab);
            AnimateCardsIntoGridFromDeck(CardsOnTable);
            GameStatisticsManager.Instance.ResetStatistics();
            PlayerPrefs.SetInt("gameInProgress", 1);
            _gameStopwatch.StartStopwatch();
        }

        private void AnimateCardsIntoGridFromDeck(List<CardView.CardView> cardsToAnimate)
        {
            CenterGrid.cols = cardsToAnimate.Count / 3;
            CenterGrid.GenerateGrid(cardsToAnimate, "center");
        }

        public void AddToSet(SetCard card)
        {
            Set.AddToSet(card);
        }

        private (CardView.CardView, int) ReplaceCardOnIndex(int i)
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
                var newCards = new List<CardView.CardView>();
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

        private void InsertCardViewsInGridColumn(List<CardView.CardView> cardViews, int colIndex)
        {
            CenterGrid.InsertInColumn(cardViews, colIndex);
        }

        public bool IsSetClicked()
        {
            GameManager.Instance.EnableHintBtn(true);
            if (Set.IsSet())
            {
                FirebaseAnalytics.LogEvent("set_clicked",
                    new Parameter("card1_type", Set.GetSet().ToList()[0].ToString()),
                    new Parameter("card1_index", Set.GetSet().ToList()[0].Index),
                    new Parameter("card2_type", Set.GetSet().ToList()[1].ToString()),
                    new Parameter("card2_index", Set.GetSet().ToList()[1].Index),
                    new Parameter("card3_type", Set.GetSet().ToList()[2].ToString()),
                    new Parameter("card3_index", Set.GetSet().ToList()[2].Index)
                    );
                var statistics = GameStatisticsManager.Instance.GameStatistics;
                statistics.SetsFound++;
                statistics.MaxSetsFoundInARow++;
                statistics.LastSetFound = Set;
                statistics.CurrentElapsedSeconds = (int)_gameStopwatch.Value;
                switch (Set.DiffPropsCount)
                {
                    case 1:
                        FirebaseAnalytics.LogEvent("set_clicked_diff_props",
                            new Parameter("nr_of_diff_props", 1));
                        statistics.NumSets1DiffProp++;
                        break;
                    case 2:
                        FirebaseAnalytics.LogEvent("set_clicked_diff_props",
                            new Parameter("nr_of_diff_props", 2));
                        statistics.NumSets2DiffProp++;
                        break;
                    case 3:
                        FirebaseAnalytics.LogEvent("set_clicked_diff_props",
                            new Parameter("nr_of_diff_props", 3));
                        statistics.NumSets3DiffProp++;
                        break;
                    case 4:
                        FirebaseAnalytics.LogEvent("set_clicked_diff_props",
                            new Parameter("nr_of_diff_props", 4));
                        statistics.NumSets4DiffProp++;
                        break;
                }

                FirebaseAnalytics.LogEvent("update_achievement_progresses",
                    new Parameter("type", "during_game"));
                GameManager.Instance.UpdateAchievementProgresses(statistics, UpdateType.DuringGame);
                return true;
            }
            return false;
        }

        private void DrawCardOnIndex(CardView.CardView newCard, int i)
        {
            CenterGrid.Insert(newCard, i);
        }

        public void SelectHint()
        {
            GameStatisticsManager.Instance.GameStatistics.HintsUsed++;
            GameStatisticsManager.Instance.GameStatistics.MaxSetsFoundInARow = 0;
            List<CardView.CardView> set = FindSetOnTable();
            if (set == null)
            {
                FirebaseAnalytics.LogEvent("deal_cards",
                    new Parameter("reason", "hint"));
                DealAdditionalCards(3);
            }
            else
            {
                foreach (CardView.CardView cv in set)
                {
                    cv.Select(SelectType.HINT);
                }
                FirebaseAnalytics.LogEvent("highlight_hint",
                    new Parameter("card1_type", set[0].ToString()),
                    new Parameter("card1_index", set[0].Card.Index),
                    new Parameter("card2_type", set[1].ToString()),
                    new Parameter("card2_index", set[1].Card.Index),
                    new Parameter("card3_type", set[2].ToString()),
                    new Parameter("card3_index", set[2].Card.Index)
                );

                GameManager.Instance.EnableHintBtn(false);
            }
        }

        public void RemoveSelectionsOnCards()
        {
            foreach (CardView.CardView cv in CardsOnTable)
            {
                cv.Select(SelectType.NONE);
            }

            ClickedCards.Clear();
        }

        private List<CardView.CardView> FindSetOnTable()
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
                            List<CardView.CardView> foundSet = new List<CardView.CardView>();
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
            GUtils.Utils.Shuffle(CardsOnTable);
            CenterGrid.ShuffleCards(CardsOnTable);
            GameStatisticsManager.Instance.GameStatistics.ShufflesUsed++;
        }

        public void ResumeGame()
        {
            _gameStopwatch.StartStopwatch();
        }

        public void PauseGame()
        {
            _gameStopwatch.Stop();
        }

        public void EndGame()
        {
            if (PlayerPrefs.GetInt("gameInProgress", 0) == 1)
            {
                // IsGameRunning = false;
                PlayerPrefs.SetInt("gameInProgress", 0);
                _gameStopwatch.Stop();
                GameStatisticsManager.Instance.GameStatistics.DurationInSeconds = (int)_gameStopwatch.Value;
                _gameStopwatch.ResetStopwatch();
            }
        }

        private void DestroyCards(List<CardView.CardView> toDestroy)
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
            var cardsToDestroy = new List<CardView.CardView>(ClickedCards);
            for (var i = 0; i < cardsToDestroy.Count; i++)
            {
                CardView.CardView c = cardsToDestroy[i];
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
            FirebaseAnalytics.LogEvent("invalid_set_clicked",
                new Parameter("card1_type", ClickedCards[0].ToString()),
                new Parameter("card1_index", ClickedCards[0].Card.Index),
                new Parameter("card2_type", ClickedCards[1].ToString()),
                new Parameter("card2_index", ClickedCards[1].Card.Index),
                new Parameter("card3_type", ClickedCards[2].ToString()),
                new Parameter("card3_index", ClickedCards[2].Card.Index)
            );

            var statistics = GameStatisticsManager.Instance.GameStatistics;
            statistics.MistakesCount++;
            statistics.MaxSetsFoundInARow = 0;
            // Shake if the selected three cards do not form a SET
            foreach (CardView.CardView c in ClickedCards)
            {
                c.transform.DOComplete();
                c.transform.DOPunchRotation(new Vector3(0, 0, 2), 1);
            }
        }

        public List<int> GetIndexOfCards(List<CardView.CardView> setCards)
        {
            var indices = setCards.Select(card => CardsOnTable.IndexOf(card)).OrderBy(i => i).ToList();

            return indices;
        }

        public void RemoveCardsFromTable(List<CardView.CardView> cardsToRemove)
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
            return Utils.GetTimeSpanString(TimeSpan.FromSeconds(_gameStopwatch.Value));
        }

        public void Save()
        {
            FirebaseAnalytics.LogEvent("save_game");
            PauseGame();
            FirebaseAnalytics.LogEvent("pause_game",
                new Parameter("reason", "save_game"));
            File.WriteAllText(Application.persistentDataPath + "/gameInProgress.json",
                JsonConvert.SerializeObject(new GameData(Deck, CardsOnTable.Select(cv => cv.Card).ToList(), 
                    (int)_gameStopwatch.Value, GameStatisticsManager.Instance.GameStatistics), JsonUtils.SerializerSettings));
        }

        public void Load()
        {
            FirebaseAnalytics.LogEvent("load_game");
            GameData data = null;
            if (File.Exists(Application.persistentDataPath + "/gameInProgress.json"))
            {
                data =
                    JsonConvert.DeserializeObject<GameData>(
                        File.ReadAllText(Application.persistentDataPath + "/gameInProgress.json"), JsonUtils.SerializerSettings);
            }
        
            if (data != null)
            {
                Deck = data.Deck;
                CardsOnTable = GameUtils.InstantiateCardViews(data.CardsOnTable, CardPrefab);
                _gameStopwatch.Value = data.ElapsedSeconds;
                AnimateCardsIntoGridFromDeck(CardsOnTable);
                GameStatisticsManager.Instance.GameStatistics = data.GameStatistics;
            }
        }
    }
}