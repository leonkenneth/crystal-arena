namespace CrystalArena.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.AI;
    using CrystalArena.Decisions;
    using CrystalArena.Infrastructure;

    public class PutAllPermanentsOnTopOfMainDeck : Effect
    {
        private readonly Func<Card, bool> _filter;

        private PutAllPermanentsOnTopOfMainDeck() { }

        public PutAllPermanentsOnTopOfMainDeck(Func<Card, bool> filter)
        {
            _filter = filter;
        }

        protected override void ResolveEffect()
        {
            PutPlayersPermanentsOnTopOfMainDeck(Players.Active);
            PutPlayersPermanentsOnTopOfMainDeck(Players.Passive);
        }

        private void PutPlayersPermanentsOnTopOfMainDeck(Player player)
        {
            var permanents = player.Battlefield.Where(x => _filter(x)).ToList();

            var orderAndPutOnTop = new OrderAndPutOnTopOfMainDeck(player, permanents);

            Enqueue(
                new OrderCards(
                    player,
                    p =>
                    {
                        p.Cards = permanents;
                        p.ProcessDecisionResults = orderAndPutOnTop;
                        p.ChooseDecisionResults = orderAndPutOnTop;
                        p.Title = "Order cards from top to bottom";
                    }
                )
            );
        }

        [Copyable]
        public class OrderAndPutOnTopOfMainDeck
            : IProcessDecisionResults<Ordering>,
                IChooseDecisionResults<List<Card>, Ordering>
        {
            private readonly Player _controller;
            private List<Card> _candidates;

            private OrderAndPutOnTopOfMainDeck() { }

            public OrderAndPutOnTopOfMainDeck(Player controller, List<Card> candidates)
            {
                _controller = controller;
                _candidates = candidates;
            }

            public Ordering ChooseResult(List<Card> candidates)
            {
                return QuickDecisions.OrderTopCards(_candidates, _controller);
            }

            public void ProcessResults(Ordering results)
            {
                _candidates.ShuffleInPlace(results.Indices);

                for (var i = _candidates.Count - 1; i >= 0; i--)
                {
                    _candidates[i].PutOnTopOfMainDeck();
                }
            }
        }
    }
}
