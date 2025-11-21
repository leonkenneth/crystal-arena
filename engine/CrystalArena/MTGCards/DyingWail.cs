namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class DyingWail : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Dying Wail")
                .ManaCost("{1}{B}")
                .Type("Monster Aura")
                .Text("When enchanted forward dies, target player discards two cards.")
                .FlavorText(
                    "This is a world of spiteful wasps that sting and kill even as they die."
                )
                .Cast(p =>
                {
                    p.Effect = () => new Attach();
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectOrCostRankBy(c => c.Score, ControlledBy.SpellOwner));
                })
                .TriggeredAbility(p =>
                {
                    p.Text = "When enchanted forward dies, target player discards two cards.";

                    p.Trigger(
                        new OnZoneChanged(
                            @from: Zone.Battlefield,
                            to: Zone.BreakZone,
                            selector: (c, ctx) => ctx.OwningCard.AttachedTo == c
                        )
                    );

                    p.Effect = () => new DiscardCards(2);
                    p.TargetSelector.AddEffect(s => s.Is.Player());
                    p.TargetingRule(new EffectOpponent());

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
