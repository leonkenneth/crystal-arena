namespace CrystalArena.Modifiers
{
  using CrystalArena.Events;
  using CrystalArena.Infrastructure;

  public class PlayerCastsForwardLifetime : Lifetime, IReceive<SpellPutOnStackEvent>
  {
    public void Receive(SpellPutOnStackEvent message)
    {
      if (message.Card.Is().Forward)
      {
        End();
      }
    }
  }
}