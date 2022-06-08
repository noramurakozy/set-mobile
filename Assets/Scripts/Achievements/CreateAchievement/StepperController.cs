using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Achievements.AchievementTypes;
using Firebase.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Achievements.CreateAchievement
{
    public class StepperController : MonoBehaviour
    {
        [SerializeField] private List<StepUI> stepUIs;
        [SerializeField] private List<LineUI> connectingLineUIs;
        [SerializeField] private List<GameObject> stepContents;
        [SerializeField] private TMP_Text textPrefab;
        [SerializeField] private TMP_InputField inputFieldPrefab;
        [SerializeField] private RectTransform textHolder;
        [SerializeField] private DifficultyUI difficultyUI;
        [SerializeField] private Button btnChangeValues;
        [SerializeField] private Button btnChangeTemplate;
        [SerializeField] private Button btnCancelNewAchievement;
        [SerializeField] private Button btnNextStep;
        [SerializeField] private Button btnAcceptAndCreate;

        private List<TMP_InputField> _inputFields;
        public List<AchievementTemplate> AchievementTemplates { get; set; }
        private AchievementTemplate _selectedTemplate;
        private string[] _textParts;
        private string _concatenatedText;
        private Achievement CreatedAchievement { get; set; }
        private int CurrentStep { get; set; }

        private void Start()
        {
            _inputFields = new List<TMP_InputField>();
            CurrentStep = 1;
            for (var i = 0; i < stepUIs.Count; i++)
            {
                var stepUI = stepUIs[i];
                stepUI.SetAsActiveStep(false);
                stepContents[i].SetActive(false);
            }

            btnChangeTemplate.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("create_achievement_change_template");
                MoveToStep(1);
            });
            btnChangeValues.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("create_achievement_change_values");
                MoveToStep(2);
            });
            btnNextStep.onClick.AddListener(MoveToNextStep);
            btnAcceptAndCreate.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("accept_and_create_achievement");
                CreateAchievementManager.Instance.CreateAchievement(CreatedAchievement);
            });
        }

        public void MoveToStep(int currentStep)
        {
            FirebaseAnalytics.LogEvent("create_achievement_move_to_step",
                new Parameter("step", currentStep));
            CurrentStep = currentStep;
            if (currentStep <= stepUIs.Count)
            {
                foreach (var stepContent in stepContents)
                {
                    stepContent.SetActive(false);
                }

                foreach (var stepUI in stepUIs)
                {
                    stepUI.SetAsActiveStep(false);
                }

                foreach (var stepLine in connectingLineUIs)
                {
                    stepLine.SetAsActiveLine(false);
                }
                
                for (int i = 0; i < currentStep; i++)
                {
                    stepUIs[i].SetAsActiveStep(true);
                    if (i >= 1)
                    {
                        connectingLineUIs[i-1].SetAsActiveLine(true);
                    }
                }
                
                stepContents[currentStep-1].SetActive(true);
                SetupStepContent(currentStep);
            }
        }

        private void SetupStepContent(int currentStep)
        {
            if (currentStep == 1)
            {
                SetupFirstStep();
                btnCancelNewAchievement.gameObject.SetActive(true);
                btnChangeTemplate.gameObject.SetActive(false);
                btnChangeValues.gameObject.SetActive(false);
                btnAcceptAndCreate.gameObject.SetActive(false);
                btnNextStep.gameObject.SetActive(true);
            }
            else if (currentStep == 2)
            {
                SetupSecondStep(_selectedTemplate);
                btnCancelNewAchievement.gameObject.SetActive(false);
                btnChangeTemplate.gameObject.SetActive(true);
                btnChangeValues.gameObject.SetActive(false);
                btnAcceptAndCreate.gameObject.SetActive(false);
                btnNextStep.gameObject.SetActive(true);
            }
            else if (currentStep == 3)
            {
                SetupThirdStep(_selectedTemplate);
                btnCancelNewAchievement.gameObject.SetActive(false);
                btnChangeTemplate.gameObject.SetActive(false);
                btnChangeValues.gameObject.SetActive(true);
                btnAcceptAndCreate.gameObject.SetActive(true);
                btnNextStep.gameObject.SetActive(false);
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
            CreatedAchievement = AchievementManager.Instance.InitiateAchievement(CreationType.Custom, _selectedTemplate, _concatenatedText,
                _inputFields.Select(field => (object)int.Parse(field.text)).ToList());
            difficultyUI.SetDifficulty(CreatedAchievement.Difficulty);
            FirebaseAnalytics.LogEvent("create_achievement_setup_third_step",
                new Parameter("selected_template", _selectedTemplate.Type.ToString()),
                new Parameter("achievement_text", CreatedAchievement.Text),
                new Parameter("achievement_id", CreatedAchievement.ID.ToString()),
                new Parameter("achievement_difficulty", CreatedAchievement.Difficulty.ToString())
                );
        }

        private void SetupSecondStep(AchievementTemplate selectedTemplate)
        {
            FirebaseAnalytics.LogEvent("create_achievement_setup_second_step",
                new Parameter("selected_template", _selectedTemplate.Text));
            foreach (Transform child in textHolder.transform)
            {
                Destroy(child.gameObject);
            }

            // List<string> prevValues = new List<string>();
            if (_inputFields.Count != 0)
            {
                // prevValues = _inputFields.Select(field => field.text).ToList();
                _inputFields = new List<TMP_InputField>();
            }

            _textParts = Regex.Split(selectedTemplate.Text, @"{\d+}");
            for (var i = 0; i < _textParts.Length; i++)
            {
                var textPart = _textParts[i];
                var text = Instantiate(textPrefab, textHolder, false);
                text.text = textPart;

                if (i != _textParts.Length - 1)
                {
                    var inputField = Instantiate(inputFieldPrefab, textHolder, false);
                    // if (prevValues.Count != 0)
                    // {
                    //     inputField.text = prevValues[i];
                    // }
                    _inputFields.Add(inputField);
                    var i1 = i;
                    inputField.onEndEdit.AddListener(value =>
                    {
                        FirebaseAnalytics.LogEvent("create_achievement_input_changed",
                            new Parameter("selected_template", selectedTemplate.Text),
                            new Parameter("input_index", i1),
                            new Parameter("new_value", value));
                        if (int.Parse(value) < selectedTemplate.MinInputValues[i1])
                        {
                            FirebaseAnalytics.LogEvent("create_achievement_force_min_input",
                                new Parameter("selected_template", selectedTemplate.Text),
                                new Parameter("input_index", i1),
                                new Parameter("from", value),
                                new Parameter("to", selectedTemplate.MinInputValues[i1]));
                            inputField.text = selectedTemplate.MinInputValues[i1].ToString();
                        }

                        if (int.Parse(value) > selectedTemplate.MaxInputValues[i1])
                        {
                            FirebaseAnalytics.LogEvent("create_achievement_force_max_input",
                                new Parameter("selected_template", selectedTemplate.Text),
                                new Parameter("input_index", i1),
                                new Parameter("from", value),
                                new Parameter("to", selectedTemplate.MaxInputValues[i1]));
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
                AchievementTemplates.Select(template =>
                    new TMP_Dropdown.OptionData(string.Format(template.Text, "X", "Y", "Z"))));
            _selectedTemplate ??= AchievementTemplates[0];
            FirebaseAnalytics.LogEvent("create_achievement_setup_first_step",
                new Parameter("default_template", _selectedTemplate.Text));
            achievementsDropdown.onValueChanged.AddListener(DropdownValueChanged);
        }

        private void DropdownValueChanged(int index)
        {
            FirebaseAnalytics.LogEvent("create_achievement_dropdown_changed");
            _selectedTemplate = AchievementTemplates[index];
        }

        public void MoveToNextStep()
        {
            FirebaseAnalytics.LogEvent("create_achievement_next_step");
            CurrentStep++;
            MoveToStep(CurrentStep);
        }
    }
}