using System;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements.CreateAchievement
{
    public class LineUI : MonoBehaviour
    {
        private Image _background;
        private Color _grey;
        private Color _purple;
        private void Start()
        {
            _background = GetComponent<Image>();
            _grey = new Color32(217, 217, 217, 255);
            _purple = new Color32(86, 26, 110, 255);
            _background.color = _purple;
        }

        public void SetAsActiveLine()
        {
            _background.color = _grey;
        }
    }
}