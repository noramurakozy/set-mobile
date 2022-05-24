using System;

namespace Achievements.AchievementTypes
{
    public class AchievementTemplate
    {
        public string Text { get; set; }
        public Type Type { get; set; }

        public AchievementTemplate(string text, Type type)
        {
            Text = text;
            Type = type;
        }
    }
}