using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.U2D;

public class CardView : MonoBehaviour
{
    private SetCard _card;
    public SetCard Card
    {
        get => _card;
        set
        {
            _card = value;
            CardSpriteIndex = value.Index;
        }
    }

    private int CardSpriteIndex { get; set; }
    public bool IsSelected { get; set; }
    
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private SpriteAtlas spriteAtlas;
    
    [SerializeField] private SpriteRenderer glow;
    
    public CardView(SetCard card){
        Card = card;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = spriteAtlas.GetSprite($"cards_1_{CardSpriteIndex}");
        spriteRenderer.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Select(SelectType selectType) {

        switch (selectType){
            case SelectType.CLICK:
                IsSelected = true;
                glow.enabled = true;
                break;
            case SelectType.HINT:
                // TODO: set hint color
                // image.setForeground(context.getResources().getDrawable(R.drawable.card_hintselector, null));
                break;
            case SelectType.NONE:
                IsSelected = false;
                glow.enabled = false;
                // TODO: remove hint color and selection color
                // image.setForeground(null);
                break;
        }
    }
    
    public override string ToString()
    {
        return CardSpriteIndex + ": " + Card.Fill + "," + Card.Color + "," + Card.Number + "," + Card.Shape;
    }
}
