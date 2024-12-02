namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Events;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class PatternOfRebirth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pattern of Rebirth")
        .ManaCost("{3}{G}")
        .Type("Monster Aura")
        .Text(
          "When enchanted forward dies, that forward's controller may search his or her library for a forward card and put that card onto the battlefield. If that player does, he or she shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectOrCostRankBy(c => c.Toughness.GetValueOrDefault(), ControlledBy.SpellOwner));
            p.TimingRule(new OnFirstMain());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When enchanted forward dies, that forward's controller may search his or her library for a forward card and put that card onto the battlefield. If that player does, he or she shuffles his or her library.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.BreakZone,
              selector: (c, ctx) => ctx.OwningCard.AttachedTo == c));

            p.Effect = () => new SearchMainDeckPutToZone(
              zone: Zone.Battlefield,
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is().Forward,
              text: "Search your library for a forward.",
              player: P(
                e => e.TriggerMessage<ZoneChangedEvent>().Controller,
                EvaluateAt.OnResolve));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}