using UnityEngine;

namespace SettingsScene
{
    public class Settings
    {
        private static Settings _instance;
        public static Settings Instance => _instance ??= new Settings();
        private Settings() {}

        public void SetShowTimer(int on)
        {
            PlayerPrefs.SetInt("showTimer", on);
        }

        public void SetShowNumOfSets(int on)
        {
            PlayerPrefs.SetInt("showNumOfSets", on);
        }

        public void SetShowHintsUsed(int on)
        {
            PlayerPrefs.SetInt("showHintsUsed", on);
        }

        public void SetShowShufflesUsed(int on)
        {
            PlayerPrefs.SetInt("showShufflesUsed", on);
        }

        public void SetAutoDeal(int on)
        {
            PlayerPrefs.SetInt("autoDeal", on);
        }

        public bool GetShowTimer()
        {
            return PlayerPrefs.GetInt("showTimer", 1) == 1;
        }

        public bool GetShowNumOfSets()
        {
            return PlayerPrefs.GetInt("showNumOfSets",0) == 1;
        }

        public bool GetShowHintsUsed()
        {
            return PlayerPrefs.GetInt("showHintsUsed", 0) == 1;
        }

        public bool GetShowShufflesUsed()
        {
            return PlayerPrefs.GetInt("showShufflesUsed", 0) == 1;
        }

        public bool GetAutoDeal()
        {
            return PlayerPrefs.GetInt("autoDeal", 1) == 1;
        }

        public void ClearSettings()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}