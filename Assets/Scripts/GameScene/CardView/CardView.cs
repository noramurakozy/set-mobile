using System.Collections;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.U2D;

namespace GameScene.CardView
{
    public class CardView : MonoBehaviour
    {
        [JsonProperty]
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
        [JsonProperty] private int CardSpriteIndex { get; set; }
        public bool IsSelected { get; set; }
    
        private Sprite _sprite;
        private SpriteRenderer SpriteRenderer { get; set; }
        [field: SerializeField] private SpriteAtlas SpriteAtlas { get; set; }
        [SerializeField] private SpriteRenderer glow;
        [SerializeField] private SpriteRenderer overlay;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _sprite = SpriteAtlas.GetSprite($"cards_1_{CardSpriteIndex}");
            SpriteRenderer.sprite = _sprite;
        }

        public void Select(SelectType selectType) {

            switch (selectType){
                case SelectType.CLICK:
                    IsSelected = true;
                    glow.enabled = true;
                    overlay.enabled = false;
                    break;
                case SelectType.HINT:
                    overlay.enabled = true;
                    glow.enabled = false;
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
                    StartCoroutine(DeselectCardAfter(0.5f));
                    break;
                case SelectType.TUTORIAL_WRONG:
                    IsSelected = true;
                    overlay.enabled = true;
                    overlay.color = UnityEngine.Color.red;
                    break;
            }
        }
    
        private IEnumerator DeselectCardAfter(float secs)
        {
            yield return new WaitForSeconds(secs);
            Select(SelectType.NONE);
        }

        public float GetWidth()
        {
            return GetComponent<BoxCollider2D>().bounds.size.x;
        }
    
        public float GetHeight()
        {
            return GetComponent<BoxCollider2D>().bounds.size.y;
        }
    
        public override string ToString()
        {
            return CardSpriteIndex + ": " + Card.Fill + "," + Card.Color + "," + Card.Number + "," + Card.Shape;
        }
    }
}
