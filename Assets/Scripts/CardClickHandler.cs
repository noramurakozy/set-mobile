using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer glow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // check if null
        var clickedCard = eventData.pointerClick.GetComponent<Card>();
        glow.enabled = !glow.enabled;
    }
}
