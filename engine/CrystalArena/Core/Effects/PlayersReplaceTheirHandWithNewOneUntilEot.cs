namespace CrystalArena.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Events;
  using CrystalArena.Infrastructure;

  public class PlayersReplaceTheirHandWithNewOneUntilEot : Effect
  {
    protected override void ResolveEffect()
    {
      ReplacePlayersHandWithNewOneUntilEot(Players.Active);
      ReplacePlayersHandWithNewOneUntilEot(Players.Passive);
    }

    private void ReplacePlayersHandWithNewOneUntilEot(Player player)
    {
      var removedFromPlay = player.Hand.ToList();

      foreach (var card in removedFromPlay)
      {
        card.RemoveFromPlay(this);
      }

      player.DrawCards(7);
      Subscribe(new ReturnRemovedFromPlayCardsToHand(removedFromPlay, player, Game));
    }

    [Copyable]
    public class ReturnRemovedFromPlayCardsToHand : GameObject, IReceive<EndOfTurnEvent>
    {
      private readonly Player _controller;
      private readonly List<Card> _removedFromPlayCards;

      private ReturnRemovedFromPlayCardsToHand() {}

      public ReturnRemovedFromPlayCardsToHand(List<Card> removedFromPlayCards, Player controller, Game game)
      {
        _controller = controller;
        Game = game;
        _removedFromPlayCards = removedFromPlayCards;
      }

      public void Receive(EndOfTurnEvent message)
      {
        _controller.DiscardHand();

        foreach (var removedFromPlayCard in _removedFromPlayCards)
        {
          _controller.PutCardToHand(removedFromPlayCard);
        }

        Unsubscribe(this);
      }
    }
  }
}