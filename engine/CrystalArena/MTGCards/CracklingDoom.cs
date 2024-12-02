namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TimingRules;
  using Effects;

  public class CracklingDoom : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crackling Doom")
        .ManaCost("{R}{W}{B}")
        .Type("Summon")
        .Text("Crackling Doom deals 2 damage to each opponent. Each opponent sacrifices a forward with the greatest power among forwards he or she controls.")
        .FlavorText("Do not fear the lightning. Fear the one it obeys.")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new DealDamageToPlayer(2, P(e => e.Controller.Opponent)),
            new PlayerSacrificePermanents(
              count: 1,
              player: P(e => e.Controller.Opponent),
              filter: c => c.Is().Forward && c.Controller.Battlefield.Forwards.All(cr => cr.Power <= c.Power),
              text: "Sacrifice a forward."));


          p.TimingRule(new Any(new NonTargetRemovalTimingRule(1), new OnMainStepsOfYourTurn()));
        });
    }
  }
}
