using Statistics;

namespace Achievements.AchievementTypes
{
    public class XGamesYSeconds : Achievement
    {
        public XGamesYSeconds(Difficulty difficulty, string text, AchievementTemplate template) : base(difficulty, text, template)
        {
            
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            throw new System.NotImplementedException();
        }
    }
}