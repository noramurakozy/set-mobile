using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements.CreateAchievement
{
    public class StepUI : MonoBehaviour
    {
        private Image _background;
        private TMP_Text _stepNum;
        private Color _grey;
        private Color _purple;
        public bool IsActiveStep { get; set; }

        private void Start()
        {
            _background = GetComponent<Image>();
            _stepNum = GetComponentInChildren<TMP_Text>();
            _grey = new Color32(217, 217, 217, 255);
            _purple = new Color32(86, 26, 110, 255);
        }

        public void SetAsActiveStep(bool active)
        {
            IsActiveStep = active;
            if (active)
            {
                _background.color = _grey;
                _stepNum.color = _purple;
            }
            else
            {
                _background.color = _purple;
                _stepNum.color = _grey;
            }
        }
    }
}