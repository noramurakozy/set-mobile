using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 81; i++)
        {
            var card = Instantiate(cardPrefab);
            card.cardSpriteIndex = i;
            card.transform.position = new Vector2(i * card.GetComponent<BoxCollider2D>().bounds.size.x, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}