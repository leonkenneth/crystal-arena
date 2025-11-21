namespace CrystalArena.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AI;

    public class RemoveFromPlayAllCards : Effect
    {
        private readonly Func<Effect, Card, bool> _filter;
        private readonly Zone _from;

        private RemoveFromPlayAllCards() { }

        public RemoveFromPlayAllCards(
            Zone from = Zone.Battlefield,
            Func<Effect, Card, bool> filter = null
        )
        {
            _from = from;
            _filter =
                filter
                ?? delegate
                {
                    return true;
                };
            SetTags(EffectTag.RemoveFromPlay);
        }

        protected override void ResolveEffect()
        {
            var permanents = GetCards();

            foreach (var permanent in permanents)
            {
                permanent.RemoveFromPlayFrom(_from, this);
            }
        }

        private IEnumerable<Card> GetCards()
        {
            var forTarget = Target != null && Target.IsPlayer();

            if (_from == Zone.Battlefield)
            {
                if (forTarget)
                {
                    return Target.Player().Battlefield.Where(c => _filter(this, c)).ToList();
                }

                return Players.Permanents().Where(c => _filter(this, c)).ToList();
            }

            if (_from == Zone.BreakZone)
            {
                if (forTarget)
                {
                    return Target.Player().BreakZone.Where(c => _filter(this, c)).ToList();
                }

                return Players
                    .Player1.BreakZone.Where(c => _filter(this, c))
                    .Concat(Players.Player2.BreakZone.Where(c => _filter(this, c)))
                    .ToList();
            }

            throw new NotSupportedException("Zone is not supported: " + _from);
        }
    }
}
