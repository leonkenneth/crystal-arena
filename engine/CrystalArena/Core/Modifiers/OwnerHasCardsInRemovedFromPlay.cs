namespace CrystalArena.Modifiers
{
  using System;
  using System.Linq;
  using Events;
  using Infrastructure;

  public class OwnerHasCardsInRemovedFromPlay : Lifetime, IReceive<ZoneChangedEvent>
  {
    private readonly Func<Card, bool> _selector;

    private OwnerHasCardsInRemovedFromPlay() { }

    public OwnerHasCardsInRemovedFromPlay(Func<Card, bool> selector)
    {
      _selector = selector ?? delegate { return true; };
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (message.From != Zone.RemovedFromPlay)
        return;

      if (_selector(message.Card) && !(Modifier.SourceCard.Controller.RemovedFromPlay.Any(_selector)))
      {
        End();
      }
    }
  }
}
