using Statistics;

namespace Achievements.AchievementTypes
{
    public abstract class Achievement
    {
        private Difficulty Difficulty { get; set; }
        private string Text { get; set; }
        protected Status Status { get; set; }

        protected Achievement(string text, int[] conditions)
        {
            Text = text;
            Status = Status.InProgress;
            Difficulty = CalculateDifficulty(conditions);
        }

        protected abstract void UpdateProgress(GameStatistics statistics);
        protected abstract Difficulty CalculateDifficulty(int[] conditions);
    }
}