using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    using Effects;

    public class SpellCounteredEvent : ITriggerMessage
    {
        public readonly Card Card;
        public readonly SpellCounterReason Reason;

        public SpellCounteredEvent(Card card, SpellCounterReason reason)
        {
            Card = card;
            Reason = reason;
        }
    }
}
