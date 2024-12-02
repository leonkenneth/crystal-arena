namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class DiabolicServitude : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Diabolic Servitude")
        .ManaCost("{3}{B}")
        .Type("Monster")
        .Text(
          "When Diabolic Servitude enters the battlefield, return target forward card from your breakZone to the battlefield.{EOL}When the forward put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.{EOL}When Diabolic Servitude leaves the battlefield, exile the forward put onto the battlefield with Diabolic Servitude.")
        .Cast(p => p.TimingRule(new WhenYourBreakZoneCountIs(minCount: 1, selector: c => c.Is().Forward)))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Diabolic Servitude enters the battlefield, return target forward card from your breakZone to the battlefield.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CompoundEffect(
              new PutTargetsToBattlefield(),
              new Attach());

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().In.YourBreakZone());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When the forward put onto the battlefield with Diabolic Servitude dies, exile it and return Diabolic Servitude to its owner's hand.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.BreakZone,
              selector: (c, ctx) => ctx.OwningCard.AttachedTo == c));

            p.Effect = () => new CompoundEffect(
              new RemoveFromPlayCard(P(e => e.Source.OwningCard.AttachedTo), Zone.BreakZone),
              new ReturnToHand(returnOwningCard: true));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Diabolic Servitude leaves the battlefield, exile the forward put onto the battlefield with Diabolic Servitude.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              selector: (c, ctx) => ctx.OwningCard == c && ctx.OwningCard.AttachedTo != null));

            p.Effect = () => new RemoveFromPlayCard(P(e => e.Source.OwningCard.AttachedTo), Zone.Battlefield);
          }
        );
    }
  }
}