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
        public bool IsActiveStep { get; set; }

        private void Start()
        {
            _background = GetComponent<Image>();
            _stepNum = GetComponentInChildren<TMP_Text>();
        }

        public void SetAsActiveStep(bool active)
        {
            IsActiveStep = active;
            if (active)
            {
                _background.color = CustomColors.grey;
                _stepNum.color = CustomColors.darkPurple;
            }
            else
            {
                _background.color = CustomColors.purple;
                _stepNum.color = CustomColors.grey;
            }
        }
    }
}