namespace CrystalArena.AI.TimingRules
{
  public class AfterOpponentDeclaresBlockers : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return IsAfterOpponentDeclaresBlocker(p.Controller);
    }
  }
}