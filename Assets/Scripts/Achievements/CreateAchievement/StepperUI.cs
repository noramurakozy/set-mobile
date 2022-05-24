using System;
using System.Collections.Generic;
using UnityEngine;

namespace Achievements.CreateAchievement
{
    public class StepperUI : MonoBehaviour
    {
        [SerializeField] private List<StepUI> stepUIs;
        [SerializeField] private List<LineUI> connectingLineUIs;

        private void Start()
        {
            foreach (var stepUI in stepUIs)
            {
                stepUI.SetAsActiveStep(false);
            }
        }

        public void MoveToStep(int currentStep)
        {
            if (currentStep <= stepUIs.Count)
            {
                stepUIs[currentStep - 1].SetAsActiveStep(true);
                
                if (currentStep >= 2)
                {
                    connectingLineUIs[currentStep-2].SetAsActiveLine();
                }
            }

        }
    }
}
