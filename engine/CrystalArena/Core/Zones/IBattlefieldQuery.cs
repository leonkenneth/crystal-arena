namespace CrystalArena
{
  using System.Collections.Generic;

  public interface IBattlefieldQuery : IZoneQuery
  {
    IEnumerable<Card> Attackers { get; }
    IEnumerable<Card> Blockers { get; }
    IEnumerable<Card> Forwards { get; }
    IEnumerable<Card> ForwardsThatCanAttack { get; }
    IEnumerable<Card> ForwardsThatCanBlock { get; }
    bool HasForwardsThatCanAttack { get; }
    IEnumerable<Card> Backups { get; }
    IEnumerable<Card> Legends { get; }
    IEnumerable<CardColor> PermanentsColors { get; }
    IEnumerable<Card> Planewalkers { get; }
  }
}