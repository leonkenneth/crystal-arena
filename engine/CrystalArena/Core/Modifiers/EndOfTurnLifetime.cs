namespace CrystalArena.Modifiers
{
    using CrystalArena.Events;
    using CrystalArena.Infrastructure;

    public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurnEvent>
    {
        public void Receive(EndOfTurnEvent message)
        {
            End();
        }
    }
}
