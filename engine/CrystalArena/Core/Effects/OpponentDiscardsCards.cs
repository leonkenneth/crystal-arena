namespace CrystalArena.Effects
{
    using System;

    public class OpponentDiscardsCards : Effect
    {
        private readonly Func<Card, bool> _filter;
        private readonly DynParam<int> _randomCount;
        private readonly DynParam<int> _selectedCount;
        private readonly DynParam<bool> _youChooseDiscardedCards;

        private OpponentDiscardsCards() { }

        public OpponentDiscardsCards(
            DynParam<int> randomCount = null,
            DynParam<int> selectedCount = null,
            DynParam<bool> youChooseDiscardedCards = null,
            Func<Card, bool> filter = null
        )
        {
            _randomCount = randomCount ?? 0;
            _selectedCount = selectedCount ?? 0;

            _filter =
                filter
                ?? delegate
                {
                    return true;
                };
            _youChooseDiscardedCards = youChooseDiscardedCards ?? false;

            RegisterDynamicParameters(randomCount, selectedCount, youChooseDiscardedCards);
        }

        protected override void ResolveEffect()
        {
            var opponent = Players.GetOpponent(Controller);

            if (_youChooseDiscardedCards.Value)
            {
                opponent.RevealHand();

                Enqueue(
                    new Decisions.DiscardCards(
                        Controller,
                        p =>
                        {
                            p.Count = _selectedCount.Value;
                            p.Filter = _filter;
                            p.DiscardOpponentsCards = true;
                        }
                    )
                );

                return;
            }

            for (var i = 0; i < _randomCount.Value; i++)
            {
                opponent.DiscardRandomCard();
            }

            if (_selectedCount.Value == 0)
                return;

            Enqueue(
                new Decisions.DiscardCards(
                    opponent,
                    p =>
                    {
                        p.Count = _selectedCount.Value;
                        p.Filter = _filter;
                    }
                )
            );
        }
    }
}
