using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Feedback
{
    public class HorizontalRadioButtonGroup : MonoBehaviour
    {
        [SerializeField] private TMP_Text leftLabelField;
        [SerializeField] private string leftLabelText = "Left label";
        [SerializeField] private TMP_Text rightLabelField;
        [SerializeField] private string rightLabelText = "Right label";
        private List<Toggle> _radioButtons;
        public int SelectedIndex { get; set; }
        public int ScaleMin { get; set; }
        public int ScaleMax { get; set; }

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
            SelectedIndex = -1;
            _toggleGroup.SetAllTogglesOff();
            // Activate the ones needed, and clear selection
            for (var i = 0; i < _radioButtons.Count; i++)
            {
                if (i >= ScaleMin && i <= ScaleMax)
                {
                    _radioButtons[i].gameObject.SetActive(true);
                    _radioButtons[i].GetComponentInChildren<TMP_Text>().text = i.ToString();
                    var i1 = i;
                    _radioButtons[i].onValueChanged.AddListener(on =>
                    {
                        if (on)
                        {
                            SelectedIndex = _radioButtons.IndexOf(_radioButtons[i1]);
                            Debug.Log(SelectedIndex);
                            _toggleGroup.allowSwitchOff = false;
                        }
                    });
                }
            }

            leftLabelField.text = leftLabelText;
            rightLabelField.text = rightLabelText;
        }
    }
}