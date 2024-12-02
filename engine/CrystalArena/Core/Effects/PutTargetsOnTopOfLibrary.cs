namespace CrystalArena.Effects
{
  public class PutTargetsOnTopOfMainDeck : Effect
  {
    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().PutOnTopOfMainDeck();
      }
    }
  }
}