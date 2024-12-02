namespace CrystalArena
{
  using System.Linq;
  using System.Security.Policy;
  using CrystalArena.Infrastructure;

  public class MainDeck : OrderedZone, IMainDeckQuery
  {
    public MainDeck(Player owner) : base(owner) {}

    private MainDeck()
    {
      /* for state copy */
    }

    public override Zone Name { get { return Zone.MainDeck; } }
    public Card Top { get { return this.FirstOrDefault(); } }
    public Card Bottom { get { return this.LastOrDefault(); } }

    public override int CalculateHash(HashCalculator calc)
    {      
      var visible = this
        .Where(x => x.IsVisibleToPlayer(Owner))
        .ToList();

      if (visible.Count == 0)
        return Count;
      
      return HashCalculator.Combine(Count, calc.Calculate(visible, true));
    }

    public void PutOnTop(Card card)
    {
      if (card.Zone == Name)
      {
        MoveToFront(card);
      }
      else
      {
        AddToFront(card);
      }
    }   
 
    public void PutOnBottom(Card card)
    {
      if (card.Zone == Name)
      {
        MoveToEnd(card);
      }
      else
      {
        AddToEnd(card);
      }
    }    
  }
}