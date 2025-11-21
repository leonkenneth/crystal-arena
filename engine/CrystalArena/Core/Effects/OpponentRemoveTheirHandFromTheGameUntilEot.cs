namespace CrystalArena.Effects
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Infrastructure;
    using Events;

    public class OpponentRemoveTheirHandFromTheGameUntilEot : Effect
    {
        protected override void ResolveEffect()
        {
            RemovePlayersHandUntilEot(Controller.Opponent);
        }

        private void RemovePlayersHandUntilEot(Player player)
        {
            var removedFromPlay = player.Hand.ToList();

            foreach (var card in removedFromPlay)
            {
                card.RemoveFromPlay(this);
            }

            Subscribe(new ReturnRemovedFromPlayCardsToHand(removedFromPlay, player, Game));
        }

        [Copyable]
        public class ReturnRemovedFromPlayCardsToHand : GameObject, IReceive<EndOfTurnEvent>
        {
            private readonly Player _controller;
            private readonly List<Card> _removedFromPlayCards;

            private ReturnRemovedFromPlayCardsToHand() { }

            public ReturnRemovedFromPlayCardsToHand(
                List<Card> removedFromPlayCards,
                Player controller,
                Game game
            )
            {
                _controller = controller;
                Game = game;
                _removedFromPlayCards = removedFromPlayCards;
            }

            public void Receive(EndOfTurnEvent message)
            {
                foreach (var removedFromPlayCard in _removedFromPlayCards)
                {
                    _controller.PutCardToHand(removedFromPlayCard);
                }

                Unsubscribe(this);
            }
        }
    }
}
