namespace CrystalArena
{
  using System.Collections.Generic;

  public interface IBreakZoneQuery : IZoneQuery
  {
    IEnumerable<Card> Forwards { get; }
  }
}