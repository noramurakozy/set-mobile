using DefaultNamespace;

namespace GameScene.Statistics
{
    public class GameStatistics
    {
        public int HintsUsed { get; set; }
        public int ShufflesUsed { get; set; }
        public int MistakesCount { get; set; }
        public int DurationInSeconds { get; set; }
        public int MaxSetsFoundInARow { get; set; }
        public Set LastSetFound { get; set; }
        public int CurrentElapsedSeconds { get; set; }
        public int SetsFound { get; set; }
        public int NumSets1DiffProp { get; set; }
        public int NumSets2DiffProp { get; set; }
        public int NumSets3DiffProp { get; set; }
        public int NumSets4DiffProp { get; set; }
    }
}