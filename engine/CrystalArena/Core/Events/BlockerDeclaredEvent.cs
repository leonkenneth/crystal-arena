namespace CrystalArena.Events
{
  using System.Collections.Generic;

  public class BlockerDeclaredEvent
  {
    public readonly Blocker Blocker;
    
    public BlockerDeclaredEvent(Blocker blocker)
    {
      Blocker = blocker;
    }
  }
}