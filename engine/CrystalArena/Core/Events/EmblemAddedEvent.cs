namespace CrystalArena.Events
{
    public class EmblemAddedEvent
    {
        public readonly Emblem Emblem;

        public EmblemAddedEvent(Emblem emblem)
        {
            Emblem = emblem;
        }
    }
}
