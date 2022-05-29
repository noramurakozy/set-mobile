using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using GameScene.CardView;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 3;
    public int cols = 4;
    public float padding = 0.2f;

    private DeckUI _deckUI;

    private List<List<CardView>> _gridData;
    private float _tileWidth;
    private float _tileHeight;

    private void Start()
    {
        _deckUI = FindObjectOfType<DeckUI>();
    }

    public void GenerateGrid(List<CardView> cardViews, string position)
    {
        _gridData = new List<List<CardView>>();
        
        _tileWidth = cardViews[0].GetWidth() + padding;
        _tileHeight = cardViews[0].GetHeight() + padding;
        transform.position = Vector2.zero;


        for (int row = 0; row < rows; row++)
        {
            _gridData.Add(new List<CardView>());
            for (int col = 0; col < cols; col++)
            {
                CardView card = cardViews[row * cols + col];
                _gridData[row].Add(card);
                var cardTransform = card.transform;
                cardTransform.parent = transform;
                float posX = col * _tileWidth;
                float posY = row * -_tileHeight;
                // cardTransform.position = _deckUI.transform.position;
                // card.transform.DOLocalMove(new Vector2(posX, posY), 1f);
                cardTransform.position = new Vector2(posX, posY);
            }
        }
        PositionGrid(position);
    }

    public void Insert(CardView newCardView, int i)
    {
        int row = i / cols;
        int col = i % cols;
        
        _gridData[row][col] = newCardView;

        var cardTransform = newCardView.transform;
        cardTransform.parent = transform;
        float posX = col * _tileWidth;
        float posY = row * -_tileHeight;
            
        cardTransform.position = _deckUI.transform.position;
        cardTransform.DOLocalMove(new Vector2(posX, posY), 1f);
    }

    public void InsertInColumn(List<CardView> cardViews, int colIndex)
    {
        // Add 1 more column
        cols++;
        for (int i = 0; i < cardViews.Count; i++)
        {
            _gridData[i].Insert(colIndex, cardViews[i]);
            Insert(cardViews[i], i*cols+colIndex);
        }
        
        CenterGridHorizontally();
    }

    public void ShuffleCards(List<CardView> cardsOnTable)
    {
        for (int i = 0; i < _gridData.Count; i++)
        {
            for (int j = 0; j < _gridData[i].Count; j++)
            {
                _gridData[i][j] = cardsOnTable[i * cols + j];
            }
        }

        ArrangeCards();
    }

    private void ArrangeCards()
    {
        List<List<Vector2>> targetPositions = GetTargetPositions();
        for (int i = 0; i < _gridData.Count; i++)
        {
            for (int j = 0; j < _gridData[i].Count; j++)
            {
                var card = _gridData[i][j];
                var targetPosition = targetPositions[i][j];

                if (new Vector2(card.transform.position.x, card.transform.position.y) != targetPosition)
                {
                    card.transform.DOLocalMove(targetPosition, 1);
                }
            }
        }
        CenterGridHorizontally();
    }

    private List<List<Vector2>> GetTargetPositions()
    {
        List<List<Vector2>> targetPositions = new();
        for (int row = 0; row < rows; row++)
        {
            targetPositions.Add(new List<Vector2>());
            for (int col = 0; col < cols; col++)
            {
                float posX = col * _tileWidth;
                float posY = row * -_tileHeight;
                targetPositions[row].Add(new Vector2(posX, posY));
            }
        }

        return targetPositions;
    }

    private void CenterGrid()
    {
        float gridWidth = cols * _tileWidth;
        float gridHeight = rows * _tileHeight;
        transform.position = new Vector2(-gridWidth / 2 + _tileWidth/2, gridHeight / 2 - _tileHeight/2);
    }

    private void CenterGridHorizontally()
    {
        float gridWidth = cols * _tileWidth;
        var gridTransform = transform;
        gridTransform.position = new Vector2(-gridWidth / 2 + _tileWidth/2, gridTransform.position.y);
    }
    
    private void BottomCenterGrid()
    {
        float gridWidth = cols * _tileWidth;
        transform.position = new Vector2(-gridWidth / 2 + _tileWidth/2, - _tileHeight);
    }
    
    private void PositionGrid(string position)
    {
        if (position == "center")
        {
            CenterGrid();
            // Move a bit lower than the exact center
            transform.Translate(new Vector3(0,-0.5f, 0));
        }
        else if (position == "bottom")
        {
            BottomCenterGrid();
        }
    }

    public List<CardView> KeepCards(List<CardView> cardsOnTable)
    {
        // List<CardView> cardsToRemoveFromGrid = new List<CardView>();
        var gridDataCopy = new List<List<CardView>>();
        for (int i = 0; i < _gridData.Count; i++)
        {
            gridDataCopy.Add(new List<CardView>(_gridData[i]));
        }
        for (int i = 0; i < gridDataCopy.Count; i++)
        {
            for (int j = 0; j < gridDataCopy[i].Count; j++)
            {
                if (!cardsOnTable.Contains(gridDataCopy[i][j]))
                {
                    // cardsToRemoveFromGrid.Add(_gridData[i][j]);
                    _gridData[i].Remove(gridDataCopy[i][j]);
                }
            }
        }

        cols = cardsOnTable.Count / 3;
        var rearrangedData = ArrangeGridData();
        ArrangeCards();
        return rearrangedData;
    }

    private List<CardView> ArrangeGridData()
    { 
        List<List<CardView>> arrangedData = new();

        var flattenedData = _gridData.SelectMany(cardList => cardList).ToList();

        for (int i = 0; i < rows; i++)
        {
            arrangedData.Add(new List<CardView>());
            for (int j = 0; j < cols; j++)
            {
                var data = flattenedData[i * cols + j];
                arrangedData[i].Add(data);
            }
        }

        _gridData = arrangedData;
        return _gridData.SelectMany(cardList => cardList).ToList();
    }
}