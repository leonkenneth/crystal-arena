namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class ViashinoHeretic : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Viashino Heretic")
        .ManaCost("{2}{R}")
        .Type("Forward Viashino")
        .Text(
          "{1}{R},{T}: Destroy target artifact. Viashino Heretic deals damage to that artifact's controller equal to the artifact's converted mana cost.")
        .FlavorText("The past is buried for good reason.")
        .Power(1)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{R},{T}: Destroy target artifact. Viashino Heretic deals damage to that artifact's controller equal to the artifact's converted mana cost.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{R}".Parse()),
              new Tap());

            p.Effect = () => new CompoundEffect(
              new DestroyTargetPermanents(),
              new DealDamageToPlayer(
                amount: P(e => e.Target.Card().ConvertedCost),
                player: P(e => e.Target.Card().Controller)));


            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Destroy));
          });
    }
  }
}