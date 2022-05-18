using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using DG.Tweening;
using Color = UnityEngine.Color;

namespace Tutorial
{
    public class Tutorial
    {
        private List<SetCard> _allCards;
        private List<CardView> _cardViews;
        private CardView ClickedCardView { get; set; }
        private Set Set { get; set; }
        private CardView _cardPrefab;
        private List<SetCard> _cardOptions;
        private List<SetCard> _cardsToComplete;
        private SpriteRenderer _placeholderCard;
        private SpriteRenderer _placeholderCardInstance;
        private GridManager _centerGrid;
        private GridManager _bottomGrid;

        public Tutorial(CardView cardPrefab, SpriteRenderer placeholderCard, GridManager centerGrid,
            GridManager bottomGrid)
        {
            _allCards = new List<SetCard>();
            _cardViews = new List<CardView>();
            Set = new Set();
            _cardPrefab = cardPrefab;
            _centerGrid = centerGrid;
            _bottomGrid = bottomGrid;
            _placeholderCard = placeholderCard;
        }

        public void StartNewTutorial()
        {
            DestroyCardViews();
            
            Deck deck = new Deck();
            _allCards = deck.CreateDeck();
            CreateCardsForTutorial(out _cardOptions, out _cardsToComplete);
            DrawCardsInGrid(_cardsToComplete, false);
            DrawCardsInGrid(_cardOptions, true);
        }

        private List<CardView> SetCardsToCardViews(List<SetCard> cardList, bool interactable)
        {
            var generated = new List<CardView>();
            for (var i = 0; i < cardList.Count; i++)
            {
                var setCard = cardList[i];
                var cardView = Object.Instantiate(_cardPrefab);
                cardView.Card = setCard;
                cardView.GetComponent<TutorialCardClickHandler>().enabled = interactable;
                generated.Add(cardView);
            }

            return generated;
        }
        
        private void DrawCardsInGrid(List<SetCard> cardList, bool areOptionCards)
        {
            // Clear cardViews list
            ClearCardViews();
            
            if (areOptionCards)
            {
                _cardViews = SetCardsToCardViews(cardList, true);
                _bottomGrid.GenerateGrid(new List<GameObject>(_cardViews.Select(c => c.gameObject)), "bottom");
            }
            else
            {
                _cardViews = SetCardsToCardViews(cardList, false);
                foreach (var cardView in _cardViews)
                {
                    AddToSet(cardView.Card);
                }
                _placeholderCardInstance = Object.Instantiate(_placeholderCard);
                _centerGrid.GenerateGrid(new List<GameObject>(_cardViews.Select(c => c.gameObject)) { _placeholderCardInstance.gameObject }, "center");
            }
        }

        // Get 5 cards for the tutorial (3 options to choose from and 2 to complete)
        private void CreateCardsForTutorial(out List<SetCard> options, out List<SetCard> toComplete)
        {
            options = new List<SetCard>();
            toComplete = new List<SetCard>();
            
            Utils.Shuffle(_allCards);
            List<SetCard> setToGuess = FindSetInCards(_allCards);
            // Remove these cards from all cards
            _allCards = _allCards.Except(setToGuess).ToList();
            
            // Add 1 card from the set to the card options as the correct card
            SetCard correctCard = setToGuess[0];
            options.Add(correctCard);
            // Remove that card from the set
            setToGuess.RemoveAt(0);
            // Add remaining 2 cards to the set to complete by the user
            toComplete.AddRange(setToGuess);
            
            // Get 2 "wrong" cards as options, which do not complete the set
            List<SetCard> wrongCards = GetIncorrectCardsToSet(correctCard, _allCards);
            // Remove these cards from all cards
            _allCards = _allCards.Except(wrongCards).ToList();
            // Add these to the card options
            options.AddRange(wrongCards);
            
            // Shuffle card options
            Utils.Shuffle(options);
        }

        private List<SetCard> FindSetInCards(List<SetCard> cards)
        {
            Set hintSet = new Set();
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = i + 1; j < cards.Count; j++)
                {
                    for (int k = j + 1; k < cards.Count; k++)
                    {
                        hintSet.AddToSet(cards[i]);
                        hintSet.AddToSet(cards[j]);
                        hintSet.AddToSet(cards[k]);
                        
                        if (hintSet.IsSet())
                        {
                            List<SetCard> foundSet = new List<SetCard>();
                            foundSet.Add(cards[i]);
                            foundSet.Add(cards[j]);
                            foundSet.Add(cards[k]);

                            return foundSet;
                        }
                    }
                }
            }

            return null;
        }

        // Return with 2 cards that do not form a SET with this card from a list of cards
        private List<SetCard> GetIncorrectCardsToSet(SetCard card, List<SetCard> cards)
        {
            Set incorrectSet = new Set();
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = i + 1; j < cards.Count; j++)
                {
                    incorrectSet.AddToSet(cards[i]);
                    incorrectSet.AddToSet(cards[j]);
                    incorrectSet.AddToSet(card);
                    
                    if (!incorrectSet.IsSet())
                    {
                        List<SetCard> wrongCards = new List<SetCard>();
                        wrongCards.Add(cards[i]);
                        wrongCards.Add(cards[j]);

                        return wrongCards;
                    }
                }
            }

            return null;
        }

        private void ClearCardViews()
        {
            // Clear cardViews list
            _cardViews.Clear();
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
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponent<TutorialCardClickHandler>().enabled = false;
                Color color = cards[i].GetComponent<SpriteRenderer>().color;
                color.a = 0.5f;
                cards[i].GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}