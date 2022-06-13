using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text subTitle;
    [SerializeField] private Transform titleOutsidePositionRight;
    [SerializeField] private Transform titleOutsidePositionLeft;
    [SerializeField] private Transform titleInsidePosition;
    [SerializeField] private Transform subTitleOutsidePositionRight;
    [SerializeField] private Transform subTitleOutsidePositionLeft;
    [SerializeField] private Transform subTitleInsidePosition;
    
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = transform.GetComponent<CanvasGroup>();
    }

    public void EnterSceneAnimation()
    {
        gameObject.SetActive(true);
        
        // Setup starting position
        _canvasGroup.DOFade(1, 0);
        title.transform.DOMove(titleInsidePosition.position, 0);
        subTitle.transform.DOMove(subTitleInsidePosition.position, 0);
        
        // The actual enter transition
        _canvasGroup.DOFade(0, 1f).SetEase(Ease.InOutQuad).onComplete += 
            () => gameObject.SetActive(false);
        title.transform.DOMove(titleOutsidePositionLeft.position, 0.5f).SetEase(Ease.InOutQuad);
        subTitle.transform.DOMove(subTitleOutsidePositionRight.position, 0.5f).SetEase(Ease.InOutQuad);
    }
        
    public void ExitSceneAnimation(string sceneToLoad)
    {
        gameObject.SetActive(true);
        
        // Setup starting position
        _canvasGroup.DOFade(0, 0);
        title.transform.DOMove(titleOutsidePositionRight.position, 0);
        subTitle.transform.DOMove(subTitleOutsidePositionLeft.position, 0);
        
        // The actual exit transition
        _canvasGroup.DOFade(1, 1f).SetEase(Ease.InOutQuad).onComplete += 
            () =>
            {
                SceneChanger.Instance.LoadScene(sceneToLoad);
            };
        title.transform.DOMove(titleInsidePosition.position, 0.5f).SetEase(Ease.InOutQuad);
        subTitle.transform.DOMove(subTitleInsidePosition.position, 0.5f).SetEase(Ease.InOutQuad);
    }
}