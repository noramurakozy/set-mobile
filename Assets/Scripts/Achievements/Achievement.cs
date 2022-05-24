using UnityEngine.TerrainUtils;

namespace Achievements
{
    public class Achievement
    {
        public Difficulty Difficulty { get; set; }
        public string Text { get; set; }
        public Status Status { get; set; }
        
        public Achievement(Difficulty difficulty, string text)
        {
            Difficulty = difficulty;
            Text = text;
            Status = Status.InProgress;
        }
    }
}