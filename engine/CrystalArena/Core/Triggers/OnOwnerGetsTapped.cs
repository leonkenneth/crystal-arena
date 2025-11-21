namespace CrystalArena.Triggers
{
    using CrystalArena.Events;
    using CrystalArena.Infrastructure;

    public class OnOwnerGetsTapped : Trigger, IReceive<PermanentTappedEvent>
    {
        public void Receive(PermanentTappedEvent message)
        {
            if (message.Card != OwningCard)
                return;

            Set(message);
        }
    }
}
