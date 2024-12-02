namespace CrystalArena.Effects
{
  public class OpponentDiscardsHand : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.Opponent.DiscardHand();
    }
  }
}