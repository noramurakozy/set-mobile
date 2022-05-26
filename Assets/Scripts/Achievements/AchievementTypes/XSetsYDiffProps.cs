using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Find X SETs with Y different properties in total
    public class XSetsYDiffProps : Achievement
    {
        public int setsCount;

        public int setsCountCondition;
        public int propsCountCondition;
        
        public XSetsYDiffProps(string text, int x, int y) : base(text)
        {
            setsCountCondition = x;
            propsCountCondition = y;
            UpdateType = UpdateType.DuringGame;
        }

        public override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.LastSetFound.DiffPropsCount == propsCountCondition)
            {
                setsCount++;
            }

            if (setsCount >= setsCountCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var setsCountCategory = DifficultyUtils.CalculateSetsInTotalCountCategory(setsCountCondition);
            var paramCountCategory = DifficultyUtils.CalculateSetsPropsCountCategory(propsCountCondition);

            switch (setsCountCategory)
            {
                case ParameterCountCategory.Low:
                case ParameterCountCategory.Medium when paramCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.High when paramCountCategory == ParameterCountCategory.Low:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.Medium when paramCountCategory == ParameterCountCategory.Medium:
                case ParameterCountCategory.Medium when paramCountCategory == ParameterCountCategory.High:
                case ParameterCountCategory.High when paramCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.High when paramCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Hard;
                    break;
            }
        }
    }
}