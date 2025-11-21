namespace CrystalArena.Events
{
    public class PermanentTappedEvent
    {
        public readonly Card Card;

        public PermanentTappedEvent(Card card)
        {
            Card = card;
        }
    }
}
