namespace CrystalArena.AI.CostRules
{
    public class XIsBreakZoneCardCount : CostRule
    {
        public override int CalculateX(CostRuleParameters p)
        {
            return p.Controller.BreakZone.Count;
        }
    }
}
