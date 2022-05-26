using System;

namespace Achievements.AchievementTypes
{
    public class AchievementTemplate
    {
        public string Text { get; set; }
        public Type Type { get; set; }
        
        public int[] MinInputValues { get; set; }
        public int[] MaxInputValues { get; set; }

        public AchievementTemplate(string text, Type type, int[] minInputValues, int[] maxInputValues)
        {
            Text = text;
            Type = type;
            MinInputValues = minInputValues;
            MaxInputValues = maxInputValues;
        }
    }
}