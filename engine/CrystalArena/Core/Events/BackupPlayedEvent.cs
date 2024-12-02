namespace CrystalArena.Events
{
  using System;

  public class BackupPlayedEvent
  {
    public readonly Card Card;

    public BackupPlayedEvent(Card card)
    {
      Card = card;
    }

    public override string ToString()
    {
      return String.Format("{0} played {1}", Card.Controller, Card);
    }
  }
}