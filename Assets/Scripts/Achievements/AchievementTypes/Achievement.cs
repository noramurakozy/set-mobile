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
        public bool HasProgress { get; set; }

        protected Achievement(string text, CreationType creationType)
        {
            Text = text;
            Status = Status.InProgress;
            ID = Guid.NewGuid();
            CreationType = creationType;
            HasProgress = true;
        }

        public abstract void UpdateProgress(GameStatistics statistics);
        public abstract void CalculateDifficulty();
        public abstract float GetProgressValue();
        public abstract string GetProgressText();
    }
}