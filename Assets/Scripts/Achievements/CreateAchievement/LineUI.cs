﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements.CreateAchievement
{
    public class LineUI : MonoBehaviour
    {
        private Image _background;
        private void Start()
        {
            _background = GetComponent<Image>();
            _background.color = CustomColors.purple;
        }

        public void SetAsActiveLine(bool active)
        {
            if (active)
            {
                _background.color = CustomColors.grey;
            }
            else
            {
                _background.color = CustomColors.purple;
            }
        }
    }
}