using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class SwitchToggle : MonoBehaviour
    {
        [SerializeField] private RectTransform uiHandle;
        [SerializeField] private Color backgroundActiveColor;
        [SerializeField] private Color handleActiveColor;
        private Toggle _toggle;
        private Vector2 _handlePosition;
        private Color _backgroundDefaultColor, _handleDefaultColor;
        private Image _backgroundImage, _handleImage;
        
        private void Start()
        {
            _toggle = GetComponent<Toggle>();
            _handlePosition = uiHandle.anchoredPosition;
            _toggle.onValueChanged.AddListener(OnSwitch);
            
            _backgroundImage = uiHandle.parent.GetComponent<Image>();
            _handleImage = uiHandle.GetComponent<Image>();

            _backgroundDefaultColor = _backgroundImage.color;
            _handleDefaultColor = _handleImage.color;
            
            if (_toggle.isOn)
            {
                OnSwitch(true);
            }
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnSwitch);
        }

        private void OnSwitch(bool on)
        {
            // uiHandle.anchoredPosition = on ? _handlePosition * -1 : _handlePosition;
            uiHandle.DOAnchorPos(on ? _handlePosition * -1 : _handlePosition, .4f).SetEase(Ease.InOutBack);
            // _backgroundImage.color = on ? backgroundActiveColor : _backgroundDefaultColor;
            _backgroundImage.DOColor(on ? backgroundActiveColor : _backgroundDefaultColor, .6f);
            // _handleImage.color = on ? handleActiveColor : _handleDefaultColor;
            _handleImage.DOColor(on ? handleActiveColor : _handleDefaultColor, .4f);
        }
    }
}