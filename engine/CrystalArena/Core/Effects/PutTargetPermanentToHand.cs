namespace CrystalArena.Effects
{
  public class PutTargetPermanentToHand : Effect
  {
    public int Discard = 0;
    
    protected override void ResolveEffect()
    {
      //Target.Card().ReturnToHand();

      if (Discard > 0)
      {
        //Decisions.EnqueueDiscardCards(Target.Card().Controller, Discard);
      }
    }
  }
}