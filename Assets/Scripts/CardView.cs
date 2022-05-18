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
    public SpriteRenderer SpriteRenderer { get; set; }
    
    [field: SerializeField] public SpriteAtlas SpriteAtlas { get; set; }
    [SerializeField] private SpriteRenderer glow;
    [SerializeField] private SpriteRenderer overlay;
    


    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = SpriteAtlas.GetSprite($"cards_1_{CardSpriteIndex}");
        SpriteRenderer.sprite = sprite;
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
                overlay.enabled = true;
                break;
            case SelectType.NONE:
                IsSelected = false;
                glow.enabled = false;
                overlay.enabled = false;
                break;
            case SelectType.TUTORIAL_CORRECT:
                IsSelected = true;
                overlay.enabled = true;
                overlay.color = UnityEngine.Color.green;
                StartCoroutine(DeselectCardAfter(1));
                break;
            case SelectType.TUTORIAL_WRONG:
                IsSelected = true;
                overlay.enabled = true;
                overlay.color = UnityEngine.Color.red;
                break;
        }
    }
    
    private IEnumerator DeselectCardAfter(int secs)
    {
        yield return new WaitForSeconds(secs);
        Select(SelectType.NONE);
    }
    
    public override string ToString()
    {
        return CardSpriteIndex + ": " + Card.Fill + "," + Card.Color + "," + Card.Number + "," + Card.Shape;
    }
}
