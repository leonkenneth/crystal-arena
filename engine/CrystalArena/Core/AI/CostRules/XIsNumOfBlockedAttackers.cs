namespace CrystalArena.AI.CostRules
{
  using System.Linq;

  public class XIsNumOfBlockedAttackers : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.Controller.Battlefield.Forwards.Count(x => x.HasBlocker);
    }
  }
}