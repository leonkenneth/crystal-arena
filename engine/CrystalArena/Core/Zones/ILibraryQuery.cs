namespace CrystalArena
{
  using System;

  public interface IMainDeckQuery : IZoneQuery
  {
    event EventHandler Shuffled;
    Card Top { get; }
    Card Bottom { get; }
  }
}