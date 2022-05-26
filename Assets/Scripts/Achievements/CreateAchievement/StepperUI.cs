using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Achievements.AchievementTypes;
using TMPro;
using UnityEngine;

namespace Achievements.CreateAchievement
{
    public class StepperUI : MonoBehaviour
    {
        [SerializeField] private List<StepUI> stepUIs;
        [SerializeField] private List<LineUI> connectingLineUIs;
        [SerializeField] private List<GameObject> stepContents;
        [SerializeField] private TMP_Text textPrefab;
        [SerializeField] private TMP_InputField inputFieldPrefab;
        [SerializeField] private RectTransform textHolder;
        [SerializeField] private DifficultyUI difficultyUI;
        private List<TMP_InputField> _inputFields;
        public List<AchievementTemplate> AchievementTemplates { get; set; }
        private AchievementTemplate _selectedTemplate;
        private string[] _textParts;
        private string _concatenatedText;
        public Achievement CreatedAchievement { get; private set; }

        private void Start()
        {
            _inputFields = new List<TMP_InputField>();
            
            for (var i = 0; i < stepUIs.Count; i++)
            {
                var stepUI = stepUIs[i];
                stepUI.SetAsActiveStep(false);
                stepContents[i].SetActive(false);
            }
        }

        public void MoveToStep(int currentStep)
        {
            if (currentStep <= stepUIs.Count)
            {
                stepUIs[currentStep - 1].SetAsActiveStep(true);
                foreach (var stepContent in stepContents)
                {
                    stepContent.SetActive(false);
                }

                SetupStepContent(currentStep);
                stepContents[currentStep -1].SetActive(true);
                
                if (currentStep >= 2)
                {
                    connectingLineUIs[currentStep-2].SetAsActiveLine();
                }
            }

        }

        private void SetupStepContent(int currentStep)
        {
            if (currentStep == 1)
            {
                SetupFirstStep();
            }
            else if (currentStep == 2)
            {
                SetupSecondStep(_selectedTemplate);
            }
            else if (currentStep == 3)
            {
                SetupThirdStep(_selectedTemplate);
            }
        }

        private void SetupThirdStep(AchievementTemplate selectedTemplate)
        {
            var textField = stepContents[2].GetComponentInChildren<TMP_Text>();
            _concatenatedText = "";
            for (var i = 0; i < _textParts.Length; i++)
            {
                _concatenatedText += _textParts[i];

                if (i != _textParts.Length - 1)
                {
                    _concatenatedText += _inputFields[i].text;
                }
            }
            textField.text = _concatenatedText;
            CreatedAchievement = AchievementManager.Instance.InitiateAchievement(_selectedTemplate, _concatenatedText, 
                _inputFields.Select(field => (object)int.Parse(field.text)).ToList());
            difficultyUI.SetDifficulty(CreatedAchievement.Difficulty);
        }

        private void SetupSecondStep(AchievementTemplate selectedTemplate)
        {
            _textParts = Regex.Split(selectedTemplate.Text, @"{\d+}");
            for (var i = 0; i < _textParts.Length; i++)
            {
                var textPart = _textParts[i];
                var text = Instantiate(textPrefab, textHolder, false);
                text.text = textPart;

                if (i != _textParts.Length - 1)
                {
                    var inputField = Instantiate(inputFieldPrefab, textHolder, false);
                    _inputFields.Add(inputField);
                    var i1 = i;
                    inputField.onEndEdit.AddListener(value =>
                    {
                        if (int.Parse(value) < selectedTemplate.MinInputValues[i1])
                        {
                            inputField.text = selectedTemplate.MinInputValues[i1].ToString();
                        }
                        if (int.Parse(value) > selectedTemplate.MaxInputValues[i1])
                        {
                            inputField.text = selectedTemplate.MaxInputValues[i1].ToString();
                        }
                    });
                }
            }
        }

        private void SetupFirstStep()
        {
            var achievementsDropdown = stepContents[0].GetComponentInChildren<TMP_Dropdown>();
            achievementsDropdown.options = new List<TMP_Dropdown.OptionData>(
                AchievementTemplates.Select(template => new TMP_Dropdown.OptionData(string.Format(template.Text, "X", "Y", "Z"))));
            
            _selectedTemplate = AchievementTemplates[0];
            achievementsDropdown.onValueChanged.AddListener(DropdownValueChanged);
        }
        
        private void DropdownValueChanged(int index)
        {
            _selectedTemplate = AchievementTemplates[index];
        }
    }
}
