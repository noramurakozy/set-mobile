using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Feedback
{
    public class MultipleChoiceQ : MonoBehaviour
    {
        [SerializeField] private string question;
        [SerializeField] private List<string> choices;
        private VerticalRadioButtonGroup _verticalRadioButtonGroup;
        private TMP_Text _qLabel;
        private GFormQuestion _gFormQuestion;

        private void Awake()
        {
            _qLabel = GetComponentInChildren<TMP_Text>();
            _verticalRadioButtonGroup = GetComponentInChildren<VerticalRadioButtonGroup>();
            _gFormQuestion = GetComponent<GFormQuestion>();
            _verticalRadioButtonGroup.LabelTexts = choices;
            _qLabel.text = question + (_gFormQuestion.required ? " *" : string.Empty);
        }
        
        private void Update()
        {
            if (_verticalRadioButtonGroup.SelectedIndex != -1)
            {
                _gFormQuestion.Answer = choices[_verticalRadioButtonGroup.SelectedIndex];
            }
        }
    }
}