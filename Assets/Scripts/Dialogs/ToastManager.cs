using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Dialogs
{
    public class ToastManager : MonoBehaviour
    {
        public static ToastManager Instance { get; private set; }


        [SerializeField] private Toast toastPrefab;
        // [SerializeField] private Canvas toastParent;
        // [SerializeField] private Transform toastEndPosition;
        // [SerializeField] private Transform toastStartPosition;
        [SerializeField] private VerticalLayoutGroup toastLayoutGroup;
        [SerializeField] private Transform toastListStartPos;
        [SerializeField] private Transform toastListEndPos;

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

        // public void ShowToast(string bodyText, float duration)
        // {
        //     var toastInstance = Instantiate(toastPrefab, toastParent.transform, false);
        //     toastInstance.body.text = bodyText;
        //     toastInstance.transform.position = toastStartPosition.position;
        //     toastInstance.transform.DOMove(toastEndPosition.position, 0.7f).SetEase(Ease.OutExpo);
        //     toastInstance.GetComponent<CanvasGroup>().DOFade(0, duration).SetDelay(5f).onComplete += () =>
        //     {
        //         Destroy(toastInstance.gameObject);
        //     };
        // }

        public void ShowToastList(List<string> bodyTexts, float duration)
        {
            List<Toast> toastInstances = new List<Toast>();
            toastLayoutGroup.transform.position = toastListStartPos.position;
            foreach (var text in bodyTexts)
            {
                var toastInstance = Instantiate(toastPrefab, toastLayoutGroup.transform, false);
                toastInstance.body.text = text;
                toastInstances.Add(toastInstance);
            }
            toastLayoutGroup.transform.DOMove(toastListEndPos.position, 0.7f).SetEase(Ease.OutExpo);

            for (var i = 0; i < toastInstances.Count; i++)
            {
                var toast = toastInstances[i];
                toast.GetComponent<CanvasGroup>().DOFade(0, duration).SetDelay(5f*(i+1)).onComplete += () =>
                {
                    Destroy(toast.gameObject);
                };
            }
        }
    }
}