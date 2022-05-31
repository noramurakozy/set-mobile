using System;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class ProgressBar : MonoBehaviour
    {
        private Slider _slider;
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        // newProgress: value between 0 and 1
        public void SetProgress(float newProgress)
        {
            _slider.value = newProgress;
        }
    }
}