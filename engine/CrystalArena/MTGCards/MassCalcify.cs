namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class MassCalcify : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mass Calcify")
        .ManaCost("{5}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all nonwhite forwards.")
        .FlavorText("The dead serve as their own tombstones.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyAllPermanents((c, ctx) => 
              c.Is().Forward && !(c.HasColor(CardColor.Light)));
            
            p.TimingRule(new WhenOpponentControllsPermanents(
              c => c.Is().Forward && !(c.HasColor(CardColor.Light))));            
          });
    }
  }
}