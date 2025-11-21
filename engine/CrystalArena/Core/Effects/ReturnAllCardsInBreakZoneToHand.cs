namespace CrystalArena.Effects
{
    using System;
    using System.Linq;

    public class ReturnAllCardsInBreakZoneToHand : Effect
    {
        private readonly Func<Card, Game, bool> _filter;

        private ReturnAllCardsInBreakZoneToHand() { }

        public ReturnAllCardsInBreakZoneToHand(Func<Card, bool> filter)
            : this((c, g) => filter(c)) { }

        public ReturnAllCardsInBreakZoneToHand(Func<Card, Game, bool> filter = null)
        {
            _filter =
                filter
                ?? delegate
                {
                    return true;
                };
        }

        protected override void ResolveEffect()
        {
            foreach (
                var permanent in Controller.BreakZone.Where(card => _filter(card, Game)).ToList()
            )
            {
                permanent.PutToHand();
            }
        }
    }
}
