namespace CrystalArena.Effects
{
    using System;
    using System.Linq;

    public class ShuffleTargetBreakZoneIntoMainDeck : Effect
    {
        private readonly Func<Card, bool> _selector;

        private ShuffleTargetBreakZoneIntoMainDeck() { }

        public ShuffleTargetBreakZoneIntoMainDeck(Func<Card, bool> selector)
        {
            _selector = selector;
        }

        protected override void ResolveEffect()
        {
            var player = Target.Player();
            var cards = player.BreakZone.Where(_selector).ToList();
            player.ShuffleIntoMainDeck(cards);
        }
    }
}
