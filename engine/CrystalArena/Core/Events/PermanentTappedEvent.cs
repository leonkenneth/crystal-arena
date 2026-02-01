using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class PermanentTappedEvent : ITriggerMessage
    {
        public readonly Card Card;

        public PermanentTappedEvent(Card card)
        {
            Card = card;
        }
    }
}
