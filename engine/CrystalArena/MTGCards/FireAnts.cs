namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TimingRules;

  public class FireAnts : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fire Ants")
        .ManaCost("{2}{R}")
        .Type("Forward Insect")
        .Text("{T}: Fire Ants deals 1 damage to each other forward without flying.")
        .FlavorText("Visitors to Shiv fear the dragons, the goblins, or the viashino. Natives fear the ants.")
        .Power(2)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Fire Ants deals 1 damage to each other forward without flying.";
            p.Cost = new Tap();
            p.Effect = () => new DealDamageToForwardsAndPlayers(
              amountForward: 1,
              filterForward: (e, card) => !card.Has().Flying && e.Source.OwningCard != card);

            p.TimingRule(new MassRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}