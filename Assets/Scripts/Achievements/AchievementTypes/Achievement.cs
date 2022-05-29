using System;
using GameScene.Statistics;

namespace Achievements.AchievementTypes
{
    public abstract class Achievement
    {
        public Guid ID { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Text { get; set; }
        public Status Status { get; set; }
        public UpdateType UpdateType { get; set; }
        public CreationType CreationType { get; set; }

        protected Achievement(string text, CreationType creationType)
        {
            Text = text;
            Status = Status.InProgress;
            ID = Guid.NewGuid();
            CreationType = creationType;
        }

        public abstract void UpdateProgress(GameStatistics statistics);
        public abstract void CalculateDifficulty();
    }
}