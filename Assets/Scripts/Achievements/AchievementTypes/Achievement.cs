using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    public abstract class Achievement
    {
        public Difficulty Difficulty { get; set; }
        public string Text { get; set; }
        public Status Status { get; set; }

        protected Achievement(string text)
        {
            Text = text;
            Status = Status.InProgress;
        }

        protected abstract void UpdateProgress(GameStatistics statistics);
        public abstract void CalculateDifficulty();
    }
}