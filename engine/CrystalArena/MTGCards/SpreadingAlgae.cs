namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class SpreadingAlgae : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Spreading Algae")
        .ManaCost("{G}")
        .Type("Monster Aura")
        .Text(
          "Enchant Swamp{EOL}When enchanted backup becomes tapped, destroy it.{EOL}When Spreading Algae is put into a breakZone from the battlefield, return Spreading Algae to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is("swamp")).On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectOrCostRankBy(x => x.IsTapped ? 1 : 0, ControlledBy.Opponent) {TargetLimit = 1});
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When enchanted backup becomes tapped, destroy it.";
            p.Trigger(new OnPermanentGetsTapped((a, c) => a.OwningCard.AttachedTo == c));
            p.Effect = () => new DestroyPermanent(P(e => e.Source.OwningCard.AttachedTo));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Spreading Algae is put into a breakZone from the battlefield, return Spreading Algae to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}