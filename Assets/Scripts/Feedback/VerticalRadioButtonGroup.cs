using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feedback
{
    public class VerticalRadioButtonGroup : MonoBehaviour
    {
        public List<string> LabelTexts { get; set; }
        public int SelectedIndex { get; set; }
        private List<Toggle> _radioButtons;
        private ToggleGroup _toggleGroup;

        private void Awake()
        {
            // Disable all buttons at first
            _radioButtons = GetComponentsInChildren<Toggle>().ToList();
            foreach (var button in _radioButtons)
            {
                button.gameObject.SetActive(false);
            }

            _toggleGroup = GetComponent<ToggleGroup>();
        }

        private void Start()
        {
            _toggleGroup.SetAllTogglesOff();
            // Activate the ones needed, and clear selection
            for (var i = 0; i < LabelTexts.Count; i++)
            {
                _radioButtons[i].gameObject.SetActive(true);
                _radioButtons[i].GetComponentInChildren<TMP_Text>().text = LabelTexts[i];
                var i1 = i;
                _radioButtons[i].onValueChanged.AddListener(on =>
                {
                    if (on)
                    {
                        SelectedIndex = _radioButtons.IndexOf(_radioButtons[i1]);
                        Debug.Log(SelectedIndex);
                    }
                });
            }

            // Set default value
            SelectedIndex = _radioButtons.IndexOf(_toggleGroup.GetFirstActiveToggle());
        }
    }
}