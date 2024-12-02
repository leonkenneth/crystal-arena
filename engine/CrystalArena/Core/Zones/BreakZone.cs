namespace CrystalArena
{
  using System.Collections.Generic;
  using System.Linq;

  public class BreakZone : OrderedZone, IBreakZoneQuery
  {
    public BreakZone(Player owner) : base(owner) {}

    private BreakZone()
    {
      /* for state copy */
    }

    public override Zone Name { get { return Zone.BreakZone; } }
    public int Score { get { return this.Sum(x => x.Score); } }
    public IEnumerable<Card> Forwards { get { return this.Where(x => x.Is().Forward); } }
    
  }
}