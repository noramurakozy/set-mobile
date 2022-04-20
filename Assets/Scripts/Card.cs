using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Card : MonoBehaviour
{

    private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private SpriteAtlas spriteAtlas;
    
    public int cardSpriteIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprite = spriteAtlas.GetSprite($"cards4_{cardSpriteIndex}");

        spriteRenderer.sprite = sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
