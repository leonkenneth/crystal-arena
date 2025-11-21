namespace CrystalArena.Events
{
    public interface ICardActivationEvent
    {
        Player Controller { get; }
        string GetTitle();
    }
}
