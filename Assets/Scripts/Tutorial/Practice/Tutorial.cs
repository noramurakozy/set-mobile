using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Tutorial.Practice
{
    public class Tutorial
    {
        private Deck _deck;
        private CardView ClickedCardView { get; set; }
        private Set Set { get; }
        private CardView _cardPrefab;
        private List<CardView> _cardOptions;
        private List<CardView> _cardsToComplete;
        private CardView _placeholderCardPrefab;
        private CardView _placeholderCardInstance;
        private GridManager _centerGrid;
        private GridManager _bottomGrid;

        public Tutorial(CardView cardPrefab, CardView placeholderCardPrefab, GridManager centerGrid,
            GridManager bottomGrid)
        {
            Set = new Set();
            _cardPrefab = cardPrefab;
            _centerGrid = centerGrid;
            _bottomGrid = bottomGrid;
            _placeholderCardPrefab = placeholderCardPrefab;
        }

        public void StartNewTutorial()
        {
            DestroyCardViews();
            _deck = new Deck();
            CreateCardsForTutorial(out _cardOptions, out _cardsToComplete);
            DrawCardsInGrid(_cardsToComplete, "center");
            DrawCardsInGrid(_cardOptions, "bottom");
        }

        // private List<CardView> SetCardsToCardViews(List<SetCard> cardList, bool interactable)
        // {
        //     var newCardViews = GameUtils.InstantiateCardViews(cardList, _cardPrefab);
        //     foreach (var cardView in newCardViews)
        //     {
        //         cardView.GetComponent<TutorialCardClickHandler>().enabled = interactable;
        //     }
        //     return newCardViews;
        // }

        private void DrawCardsInGrid(List<CardView> cardList, string position)
        {
            if (position == "bottom")
            {
                _bottomGrid.GenerateGrid(cardList, position);
            }
            else
            {
                foreach (var cardView in cardList)
                {
                    AddToSet(cardView.Card);
                }

                _placeholderCardInstance = Object.Instantiate(_placeholderCardPrefab);
                _centerGrid.GenerateGrid(
                    new List<CardView>(cardList)
                        { _placeholderCardInstance }, position);
            }
        }

        // Get 5 cards for the tutorial (3 options to choose from and 2 to complete)
        private void CreateCardsForTutorial(out List<CardView> options, out List<CardView> toComplete)
        {
            options = new List<CardView>();
            toComplete = new List<CardView>();

            List<SetCard> setToGuess = _deck.FindSetInCards();
            _deck.RemoveCards(setToGuess);

            // Add 1 card from the set to the card options as the correct card
            var correctCardView = GameUtils.InstantiateCardView(setToGuess[0], _cardPrefab);
            options.Add(correctCardView);
            setToGuess.RemoveAt(0);
            
            // Add remaining 2 cards to the set to complete by the user
            var setToGuessViews = GameUtils.InstantiateCardViews(setToGuess, _cardPrefab);
            DisableClickOnCards(setToGuessViews);
            toComplete.AddRange(setToGuessViews);

            // Get 2 "wrong" cards as options, which do not complete the set
            List<SetCard> wrongCards = GetIncorrectCardsForSetFromDeck(correctCardView);
            var wrongCardViews = GameUtils.InstantiateCardViews(wrongCards, _cardPrefab);
            // Add these to the card options
            options.AddRange(wrongCardViews);

            // Shuffle card options
            Utils.Shuffle(options);
        }

        private void DisableClickOnCard(CardView cardView)
        {
            cardView.GetComponent<TutorialCardClickHandler>().enabled = false;
        }
        
        private void DisableClickOnCards(List<CardView> cardViews)
        {
            foreach (var cardView in cardViews)
            {
                DisableClickOnCard(cardView);
            }
        }

        // private List<SetCard> FindSetInCards(List<SetCard> cards)
        // {
        //     Set hintSet = new Set();
        //     for (int i = 0; i < cards.Count; i++)
        //     {
        //         for (int j = i + 1; j < cards.Count; j++)
        //         {
        //             for (int k = j + 1; k < cards.Count; k++)
        //             {
        //                 hintSet.AddToSet(cards[i]);
        //                 hintSet.AddToSet(cards[j]);
        //                 hintSet.AddToSet(cards[k]);
        //
        //                 if (hintSet.IsSet())
        //                 {
        //                     List<SetCard> foundSet = new List<SetCard>();
        //                     foundSet.Add(cards[i]);
        //                     foundSet.Add(cards[j]);
        //                     foundSet.Add(cards[k]);
        //
        //                     return foundSet;
        //                 }
        //             }
        //         }
        //     }
        //
        //     return null;
        // }

        // Return with 2 cards that do not form a SET with this card from a list of cards
        private List<SetCard> GetIncorrectCardsForSetFromDeck(CardView card)
        {
            Set incorrectSet = new Set();
            for (int i = 0; i < _deck.Count; i++)
            {
                for (int j = i + 1; j < _deck.Count; j++)
                {
                    incorrectSet.AddToSet(_deck.GetAt(i));
                    incorrectSet.AddToSet(_deck.GetAt(j));
                    incorrectSet.AddToSet(card.Card);

                    if (!incorrectSet.IsSet())
                    {
                        List<SetCard> wrongCards = new List<SetCard>();
                        wrongCards.Add(_deck.GetAt(i));
                        wrongCards.Add(_deck.GetAt(j));
                        
                        _deck.RemoveCards(wrongCards);
                        
                        return wrongCards;
                    }
                }
            }

            return null;
        }

        private void DestroyCardViews()
        {
            // Remove all card gameobjects
            var objects = Object.FindObjectsOfType<CardView>();
            foreach (var cardView in objects)
            {
                Object.Destroy(cardView.gameObject);
            }
        }

        private void AddToSet(SetCard card)
        {
            Set.AddToSet(card);
        }

        private void RemoveFromSet(SetCard card)
        {
            Set.RemoveFromSet(card);
        }

        public void HandleOptionCardClick(CardView newClickedCard)
        {
            // If another card is already selected, deselect it
            if (ClickedCardView != null)
            {
                DeselectCard(ClickedCardView);
            }

            if (newClickedCard.IsSelected)
            {
                DeselectCard(newClickedCard);
            }
            else
            {
                AddToSet(newClickedCard.Card);
                ClickedCardView = newClickedCard;
                bool isSetSelected = CheckIfSelectionIsSet();
                var transform = newClickedCard.transform;

                if (isSetSelected)
                {
                    newClickedCard.Select(SelectType.TUTORIAL_CORRECT);
                    // Move to the placeholder card's place
                    transform.parent = _centerGrid.transform;
                    transform.DOMove(_placeholderCardInstance.transform.position, 0.5f);

                    // Disable remaining option cards
                    DisableCards(_bottomGrid.GetComponentsInChildren<CardView>().ToList());
                }
                else
                {
                    newClickedCard.Select(SelectType.TUTORIAL_WRONG);
                    // Complete animation in case clicking on the card before finishing the animation
                    transform.DOComplete();
                    transform.DOPunchRotation(new Vector3(0, 0, 2), 1);
                }
            }
        }

        private void DeselectCard(CardView clickedCard)
        {
            clickedCard.Select(SelectType.NONE);
            RemoveFromSet(clickedCard.Card);
            ClickedCardView = null;
        }

        private bool CheckIfSelectionIsSet()
        {
            return Set.GetSize() == 3 && Set.IsSet();
        }

        private void DisableCards(List<CardView> cards)
        {
            foreach (var t in cards)
            {
                t.GetComponent<TutorialCardClickHandler>().enabled = false;
                Color color = t.GetComponent<SpriteRenderer>().color;
                color.a = 0.5f;
                t.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}