namespace CrystalArena.AI.CostRules
{
    using System.Linq;

    public class XIsOpponentsForwardCount : CostRule
    {
        public override int CalculateX(CostRuleParameters p)
        {
            return p.Controller.Opponent.Battlefield.Forwards.Count();
        }
    }
}
