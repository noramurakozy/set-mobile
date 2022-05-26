using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogs
{
    public class ToastManager : MonoBehaviour
    {
        public static ToastManager Instance { get; private set; }
        
        
        [SerializeField] private Toast toastPrefab;
        [SerializeField] private Canvas toastParent;
        [SerializeField] private Transform toastEndPosition;
        [SerializeField] private Transform toastStartPositon;
        
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void ShowToast(string bodyText, float duration)
        {
            var toastInstance = Instantiate(toastPrefab, toastParent.transform, false);
            toastInstance.body.text = bodyText;
            toastInstance.transform.position = toastStartPositon.position;
            toastInstance.transform.DOMoveY(toastEndPosition.position.y, 1);
            toastInstance.GetComponent<CanvasGroup>().DOFade(0, duration).SetDelay(5f).onComplete += () =>
            {
                Destroy(toastInstance.gameObject);
            };
        }
        
    }
}