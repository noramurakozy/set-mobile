namespace GameScene.Statistics
{
    // Stores statistics of the last game
    public class GameStatisticsManager
    {
        private static GameStatisticsManager _instance;
        public static GameStatisticsManager Instance => _instance ??= new GameStatisticsManager();

        public GameStatistics GameStatistics { get; set; }

        private GameStatisticsManager()
        {
            GameStatistics = new GameStatistics();
        }

        public void ResetStatistics()
        {
            GameStatistics = new GameStatistics();
        }
    }
}