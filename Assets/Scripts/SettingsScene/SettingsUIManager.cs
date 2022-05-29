using System;
using System.IO;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsScene
{
    public class SettingsUIManager : MonoBehaviour
    {
        [SerializeField] private Toggle showTimer;
        [SerializeField] private Toggle showNumOfSets;
        [SerializeField] private Toggle showHintsUsed;
        [SerializeField] private Toggle showShufflesUsed;
        [SerializeField] private Toggle autoDeal;
        [SerializeField] private Button btnResetGame;

        private void Start()
        {
            SetupToggles();
            showTimer.onValueChanged.AddListener(on =>
            {
                Settings.Instance.SetShowTimer(on ? 1 : 0);
            });
            showNumOfSets.onValueChanged.AddListener(on =>
            {
                Settings.Instance.SetShowNumOfSets(on ? 1 : 0);
            });
            showHintsUsed.onValueChanged.AddListener(on =>
            {
                Settings.Instance.SetShowHintsUsed(on ? 1 : 0);
            });
            showShufflesUsed.onValueChanged.AddListener(on =>
            {
                Settings.Instance.SetShowShufflesUsed(on ? 1 : 0);
            });
            autoDeal.onValueChanged.AddListener(on =>
            {
                Settings.Instance.SetAutoDeal(on ? 1 : 0);
            });
            
            btnResetGame.onClick.AddListener(ResetGame);
        }

        private void SetupToggles()
        {
            showTimer.isOn = Settings.Instance.GetShowTimer();
            showNumOfSets.isOn = Settings.Instance.GetShowNumOfSets();
            showHintsUsed.isOn = Settings.Instance.GetShowHintsUsed();
            showShufflesUsed.isOn = Settings.Instance.GetShowShufflesUsed();
            autoDeal.isOn = Settings.Instance.GetAutoDeal();
        }

        private void ResetGame()
        {
            // TODO: confirmation popup
            Settings.Instance.ClearSettings();
            // Reload toggles
            SetupToggles();
            File.Delete(Application.persistentDataPath + "/achievements.json");
            File.Delete(Application.persistentDataPath + "/userStatistics.json");
        }
    }
}