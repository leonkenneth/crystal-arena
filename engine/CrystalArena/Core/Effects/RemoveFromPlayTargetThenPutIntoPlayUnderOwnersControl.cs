namespace CrystalArena.Effects
{
  public class RemoveFromPlayTargetThenPutIntoPlayUnderOwnersControl : Effect
  {
    protected override void ResolveEffect()
    {
      Target.Card().RemoveFromPlayFrom(Zone.Battlefield, this);
      Target.Card().PutToBattlefieldFrom(Zone.RemovedFromPlay);
    }
  }
}