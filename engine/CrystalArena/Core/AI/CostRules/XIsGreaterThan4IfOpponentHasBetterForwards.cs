namespace CrystalArena.AI.CostRules
{
    using System.Linq;

    public class XIsGreaterThan4IfOpponentHasBetterForwards : CostRule
    {
        private readonly int _x;

        private XIsGreaterThan4IfOpponentHasBetterForwards() { }

        public XIsGreaterThan4IfOpponentHasBetterForwards(int x)
        {
            _x = x;
        }

        public override int CalculateX(CostRuleParameters p)
        {
            var maxX =
                p.Controller.GetAvailableManaCount(
                    new ConvokeAndDelveOptions
                    {
                        CanUseConvoke = p.OwningCard.Has().Convoke,
                        CanUseDelve = p.OwningCard.Has().Delve,
                    },
                    ManaUsage.Spells
                ) - p.OwningCard.ManaCost.Converted;

            if (maxX >= _x)
            {
                var yourScore = p.Controller.Battlefield.Forwards.Sum(x => x.Score);
                var opponentScore = p.Controller.Opponent.Battlefield.Forwards.Sum(x => x.Score);

                return opponentScore >= yourScore ? maxX : 4;
            }

            return maxX;
        }
    }
}
