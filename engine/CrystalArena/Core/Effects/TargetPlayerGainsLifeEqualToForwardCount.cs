namespace CrystalArena.Effects
{
  using System.Linq;

  public class TargetPlayerGainsLifeEqualToForwardCount : Effect
  {
    private readonly int _multiplier;

    private TargetPlayerGainsLifeEqualToForwardCount() {}

    public TargetPlayerGainsLifeEqualToForwardCount(int multiplier = 1)
    {
      _multiplier = multiplier;
    }

    protected override void ResolveEffect()
    {
      Target.Player().Life += Players.Permanents().Count(x => x.Is().Forward)*_multiplier;
    }
  }
}