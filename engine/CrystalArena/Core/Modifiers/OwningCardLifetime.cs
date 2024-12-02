namespace CrystalArena.Modifiers
{
  using CrystalArena.Events;
  using CrystalArena.Infrastructure;

  public class OwningCardLifetime : Lifetime, IReceive<ZoneChangedEvent>
  {
    public void Receive(ZoneChangedEvent message)
    {
      if (message.Card != Modifier.Owner)
        return;

      if (message.FromBattlefield)
        End();
    }
  }
}