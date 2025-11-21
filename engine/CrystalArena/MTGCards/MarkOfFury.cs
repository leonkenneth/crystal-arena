namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class MarkOfFury : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Mark of Fury")
                .Type("Monster Aura")
                .ManaCost("{R}")
                .Text(
                    "Enchanted forward has haste.{EOL}At the beginning of the end step, return Mark of Fury to its owner's hand."
                )
                .FlavorText("Many Keldon warriors bear the mark of intentional insanity.")
                .Cast(p =>
                {
                    p.Effect = () => new Attach(() => new AddSimpleAbility(Static.Haste));

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of the end step, return Mark of Fury to its owner's hand.";
                    p.Trigger(
                        new OnStepStart(step: Step.EndOfTurn, activeTurn: true, passiveTurn: true)
                    );

                    p.Effect = () => new ReturnToHand(returnOwningCard: true);
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
