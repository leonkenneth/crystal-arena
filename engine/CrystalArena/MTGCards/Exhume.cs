namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Exhume : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Exhume")
        .ManaCost("{1}{B}")
        .Type("Sorcery")
        .Text("Each player puts a forward card from his or her breakZone onto the battlefield.")
        .FlavorText("Death—an outmoded concept. We sleep, and we change.")
        .Cast(p =>
          {
            p.Effect = () => new EachPlayerPutsACardToBattlefield(Zone.BreakZone, c => c.Is().Forward);
            p.TimingRule(new WhenYourBreakZoneCountIs(minCount: 1, selector: c => c.Is().Forward));
          });
    }
  }
}