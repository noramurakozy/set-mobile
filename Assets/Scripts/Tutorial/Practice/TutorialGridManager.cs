using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tutorial.Practice
{
    public class TutorialGridManager : MonoBehaviour
    {
        public int rows = 1;
        public int cols = 3;
        public float padding = 0.2f;

        public SpriteRenderer background;

        public void GenerateGrid(List<GameObject> cardViews, string position)
        {
            if (position == "center-left")
            {
                ScaleUpElements(cardViews, 1.5f);
                var (tileWidth, tileHeight) = GetTileSize(cardViews[0]);
                GenerateCenterLeftGrid(cardViews, tileWidth, tileHeight);
            }
            else if (position == "center")
            {
                var (tileWidth, tileHeight) = GetTileSize(cardViews[0]);
                GenerateCenterHorizontalGrid(cardViews, tileWidth, tileHeight);
            }
        }

        private (float, float) GetTileSize(GameObject cardView)
        {
            var tileWidth = cardView.GetComponent<BoxCollider2D>().bounds.size.x + padding;
            var tileHeight = cardView.GetComponent<BoxCollider2D>().bounds.size.y + padding;

            return (tileWidth, tileHeight);
        }

        private void GenerateCenterHorizontalGrid(List<GameObject> cardViews, float tileWidth, float tileHeight)
        {
            PlaceElementsInGrid(cardViews, tileWidth, tileHeight);
            PositionGrid("center", tileWidth, tileHeight);
            // Move it a bit to the right and up
            transform.Translate(new Vector3(1.8f, 0.5f, 0));
        }

        private void GenerateCenterLeftGrid(List<GameObject> cardViews, float tileWidth, float tileHeight)
        {
            PlaceElementsInGrid(cardViews, tileWidth, tileHeight);
            PositionGrid("center-left", tileWidth, tileHeight);
        }

        private void PlaceElementsInGrid(List<GameObject> cardViews, float tileWidth, float tileHeight)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var card = cardViews[row * cols + col];
                    var cardTransform = card.transform;
                    cardTransform.parent = transform;
                    float posX = col * tileWidth;
                    float posY = row * -tileHeight;
                    cardTransform.position = new Vector3(0, -50, 0);
                    card.transform.DOLocalMove(new Vector2(posX, posY), 1f).SetEase(Ease.OutBack, 0.4f);
                }
            }
        }

        private void ScaleUpElements(List<GameObject> gameObjects, float scale)
        {
            foreach (var obj in gameObjects)
            {
                obj.transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        private void PositionGrid(string position, float tileWidth, float tileHeight)
        {
            if (position == "center-left")
            {
                MoveToCenter(tileWidth, tileHeight);
                MoveToLeft();
            }
            else if (position == "center")
            {
                MoveToCenter(tileWidth, tileHeight);
            }
        }

        private void MoveToLeft()
        {
            transform.Translate(new Vector3(-4, 0, 0));
        }

        private void MoveToCenter(float tileWidth, float tileHeight)
        {
            float gridWidth = cols * tileWidth;
            float gridHeight = rows * tileHeight;
            transform.position = new Vector2(-gridWidth / 2 + tileWidth / 2, gridHeight / 2 - tileHeight / 2);
            if (background != null)
            {
                background.transform.localPosition = new Vector2(tileWidth, 0);
                background.GetComponent<SpriteRenderer>().size = new Vector2(gridWidth + padding/2, gridHeight + padding/2);
            }
        }
    }
}