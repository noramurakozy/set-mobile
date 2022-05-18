using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GridManager : MonoBehaviour
    {
        public int Rows = 3;
        public int Cols = 4;
        public float Padding = 0;
        
        public void GenerateGrid(List<GameObject> cardViews, string position)
        {
            float tileWidth = 0;
            float tileHeight = 0;
            transform.position = Vector2.zero;
            
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    GameObject card = cardViews[row * Cols + col];
                    card.transform.parent = transform;
                    tileWidth = card.GetComponent<BoxCollider2D>().bounds.size.x + Padding;
                    tileHeight = card.GetComponent<BoxCollider2D>().bounds.size.y + Padding;
                    float posX = col * tileWidth;
                    float posY = row * -tileHeight;
                    
                    card.transform.position = new Vector2(posX, posY);
                }
            }

            float gridWidth = Cols * tileWidth;
            float gridHeight = Rows * tileHeight;

            if (position == "center")
            {
                transform.position = new Vector2(-gridWidth / 2 + tileWidth/2, gridHeight / 2 - tileHeight/2);
            }
            else if (position == "bottom")
            {
                transform.position = new Vector2(-gridWidth / 2 + tileWidth/2, - tileHeight);
            }
        }
    }
}