using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements.CreateAchievement
{
    public class StepUI : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text stepNum;
        [SerializeField] private TMP_Text title;
        public bool IsActiveStep { get; set; }

        public void SetAsActiveStep(bool active)
        {
            IsActiveStep = active;
            if (active)
            {
                background.color = CustomColors.grey;
                title.color = CustomColors.grey;
                stepNum.color = CustomColors.darkPurple;
            }
            else
            {
                background.color = CustomColors.purple;
                title.color = CustomColors.purple;
                stepNum.color = CustomColors.grey;
            }
        }
    }
}