﻿using System;
using DG.Tweening;
using UnityEngine;

public class Fader : MonoBehaviour
{

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = transform.GetComponent<CanvasGroup>();
    }

    public void EnterSceneAnimation()
    {
        gameObject.SetActive(true);
        // transform.DOScale(Vector3.one, 0);
        // transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad).onComplete +=
        //     () => gameObject.SetActive(false);
        _canvasGroup.DOFade(1, 0);
        _canvasGroup.DOFade(0, 1f).SetEase(Ease.InOutQuad).onComplete += 
            () => gameObject.SetActive(false);
    }
        
    public void ExitSceneAnimation(string sceneToLoad)
    {
        gameObject.SetActive(true);
        // Sequence s = DOTween.Sequence();
        // s.Append(transform.DOScale(Vector3.zero, 0)).SetEase(Ease.InOutQuad);
        // s.Join(transform.DORotate(new Vector3(0, 0, -900), 0.5f));
        // s.onComplete +=
            // () => SceneChanger.Instance.LoadScene(sceneToLoad);
        // transform.DOScale(Vector3.zero, 0);
        // transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad).onComplete +=
            // () => SceneChanger.Instance.LoadScene(sceneToLoad);
        _canvasGroup.DOFade(0, 0);
        _canvasGroup.DOFade(1, 1f).SetEase(Ease.InOutQuad).onComplete += 
            () =>
            {
                SceneChanger.Instance.LoadScene(sceneToLoad);
            };
    }
}