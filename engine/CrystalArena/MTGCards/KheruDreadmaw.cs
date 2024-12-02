namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class KheruDreadmaw : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kheru Dreadmaw")
        .ManaCost("{4}{B}")
        .Type("Forward — Zombie Crocodile")
        .Text(
          "{Defender}{EOL}{1}{G}, Sacrifice another forward: You gain life equal to the sacrificed forward's toughness.")
        .FlavorText("Its hunting instincts have long since rotted away. Its hunger, however, remains.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Defender)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{G}, Sacrifice another forward: You gain life equal to the sacrificed forward's toughness.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{G}".Parse()),
              new Sacrifice());

            p.Effect =
              () =>
                new ChangeLife(amount: P(e => e.Target.Card().Toughness.GetValueOrDefault()), whos: P(e => e.Controller));

            p.TargetSelector.AddCost(
              trg => trg.Is.Forward(ControlledBy.SpellOwner, canTargetSelf: false)
                .On.Battlefield(),
              trg => trg.Message = "Select a forward to sacrifice.");

            p.TargetingRule(new CostSacrificeToGainLife());
          });
    }
  }
}