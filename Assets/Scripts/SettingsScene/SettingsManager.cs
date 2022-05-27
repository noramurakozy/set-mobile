using System;
using System.IO;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsScene
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Toggle showTimer;
        [SerializeField] private Toggle showNumOfSets;
        [SerializeField] private Toggle showHintsUsed;
        [SerializeField] private Toggle showShufflesUsed;
        [SerializeField] private Button btnResetGame;

        private void Start()
        {
            SetupToggles();
            showTimer.onValueChanged.AddListener(on =>
            {
                PlayerPrefs.SetInt("showTimer", on ? 1 : 0);
            });
            showNumOfSets.onValueChanged.AddListener(on =>
            {
                PlayerPrefs.SetInt("showNumOfSets", on ? 1 : 0);
            });
            showHintsUsed.onValueChanged.AddListener(on =>
            {
                PlayerPrefs.SetInt("showHintsUsed", on ? 1 : 0);
            });
            showShufflesUsed.onValueChanged.AddListener(on =>
            {
                PlayerPrefs.SetInt("showShufflesUsed", on ? 1 : 0);
            });
            
            btnResetGame.onClick.AddListener(ResetGame);
        }

        private void SetupToggles()
        {
            showTimer.isOn = PlayerPrefs.GetInt("showTimer", 1) == 1;
            showNumOfSets.isOn = PlayerPrefs.GetInt("showNumOfSets",0) == 1;
            showHintsUsed.isOn = PlayerPrefs.GetInt("showHintsUsed", 0) == 1;
            showShufflesUsed.isOn = PlayerPrefs.GetInt("showShufflesUsed", 0) == 1;
        }

        private void ResetGame()
        {
            // TODO: confirmation popup
            PlayerPrefs.DeleteAll();
            // Reload toggles
            SetupToggles();
            File.Delete(Application.persistentDataPath + "/achievements.json");
            // TODO: delete player statistics
        }
    }
}