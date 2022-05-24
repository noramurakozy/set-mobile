using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<AchievementTemplate> AchievementTemplates { get; set; }
        private AchievementTemplate _selectedTemplate;

        private void Start()
        {
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
            textField.text = selectedTemplate.Text;
        }

        private void SetupSecondStep(AchievementTemplate selectedTemplate)
        {
            var textField = stepContents[1].GetComponentInChildren<TMP_Text>();
            textField.text = selectedTemplate.Text;
        }

        private void SetupFirstStep()
        {
            var achievementsDropdown = stepContents[0].GetComponentInChildren<TMP_Dropdown>();
            achievementsDropdown.options = new List<TMP_Dropdown.OptionData>(
                AchievementTemplates.Select(template => new TMP_Dropdown.OptionData(template.Text)));
            _selectedTemplate = AchievementTemplates[0];
            achievementsDropdown.onValueChanged.AddListener(DropdownValueChanged);
        }
        
        private void DropdownValueChanged(int index)
        {
            Debug.Log(index);
            _selectedTemplate = AchievementTemplates[index];
        }
    }
}
