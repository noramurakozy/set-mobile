using Statistics;

namespace Achievements.AchievementTypes
{
    public abstract class Achievement
    {
        public Difficulty Difficulty { get; set; }
        public string Text { get; set; }
        public Status Status { get; set; }

        public AchievementTemplate Template { get; set; }

        protected Achievement(Difficulty difficulty, string text, AchievementTemplate template)
        {
            Difficulty = difficulty;
            Text = text;
            Status = Status.InProgress;
            Template = template;
        }

        protected abstract void UpdateProgress(GameStatistics statistics);
    }
}