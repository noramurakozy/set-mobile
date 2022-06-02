using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feedback
{
    public class LinearScaleQuestion : MonoBehaviour
    {
        [SerializeField] private string question;
        [SerializeField] private int scaleMin;
        [SerializeField] private int scaleMax;
        private TMP_Text _qLabel;
        private HorizontalRadioButtonGroup _toggleGroup;
        private GFormQuestion _gFormQuestion;

        private void Awake()
        {
            _qLabel = GetComponentInChildren<TMP_Text>();
            _toggleGroup = GetComponentInChildren<HorizontalRadioButtonGroup>();
            _gFormQuestion = GetComponent<GFormQuestion>();
            _toggleGroup.ScaleMin = scaleMin;
            _toggleGroup.ScaleMax = scaleMax;
        }

        private void Start()
        {
            _qLabel.text = question;
        }

        private void Update()
        {
            _gFormQuestion.Answer = _toggleGroup.SelectedIndex;
        }
    }
}