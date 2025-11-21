namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class ParasiticBond : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Parasitic Bond")
                .ManaCost("{3}{B}")
                .Type("Monster Aura")
                .Text(
                    "At the beginning of the upkeep of enchanted forward's controller, Parasitic Bond deals 2 damage to that player."
                )
                .FlavorText("All bonds are parasitic. Only rulership is pure.")
                .Cast(p =>
                {
                    p.Effect = () => new Attach();
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.Opponent));
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of the upkeep of enchanted forward's controller, Parasitic Bond deals 2 damage to that player.";

                    p.Trigger(
                        new OnStepStart(step: Step.Upkeep, passiveTurn: true, activeTurn: true)
                        {
                            Condition = ctx => ctx.OwningCard.AttachedTo.Controller.IsActive,
                        }
                    );

                    p.Effect = () =>
                        new DealDamageToPlayer(amount: 2, player: P((e, g) => g.Players.Active));

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
