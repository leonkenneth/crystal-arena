namespace CrystalArena.Infrastructure
{
    public interface ITrackableValue<T> : IHashable
    {
        T Value { get; set; }
    }
}
