namespace CrystalArena
{
    using System.Collections.Generic;

    public interface IHandQuery : IZoneQuery
    {
        IEnumerable<Card> Backups { get; }
    }
}
