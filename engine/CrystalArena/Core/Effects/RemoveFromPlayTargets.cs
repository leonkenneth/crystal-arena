namespace CrystalArena.Effects
{
  using AI;

  public class RemoveFromPlayTargets : Effect
  {
    public RemoveFromPlayTargets()
    {
      SetTags(EffectTag.RemoveFromPlay);
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().RemoveFromPlay(this);
      }
    }
  }
}