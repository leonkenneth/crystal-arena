namespace CrystalArena.AI.TimingRules
{
  public class DefaultBackupsTimingRule : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return true;
    }
  }
}