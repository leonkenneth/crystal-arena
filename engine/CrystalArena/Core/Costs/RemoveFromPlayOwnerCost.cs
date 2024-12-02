namespace CrystalArena.Costs
{
  using System.Linq;
  public class RemoveFromPlayOwnerCost : Cost
  {        
    public RemoveFromPlayOwnerCost()
    {      
    }

    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      return true;      
    }

    public override void PayPartial(PayCostParameters p)
    {      
      Card.RemoveFromPlay(null);
    }
  }
}