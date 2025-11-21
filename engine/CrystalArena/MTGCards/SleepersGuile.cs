namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class SleepersGuile : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Sleeper's Guile")
                .ManaCost("{2}{B}")
                .Type("Monster Aura")
                .Text(
                    "Enchanted forward has fear. (It can't be blocked except by artifact forwards and/or dark forwards.){EOL}When Sleeper's Guile is put into a breakZone from the battlefield, return Sleeper's Guile to its owner's hand."
                )
                .Cast(p =>
                {
                    p.Effect = () => new Attach(() => new AddSimpleAbility(Static.Fear));

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster(c => !c.Has().AnyEvadingAbility));
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Sleeper's Guile is put into a breakZone from the battlefield, return Sleeper's Guile to its owner's hand.";
                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
                    p.Effect = () => new ReturnToHand(returnOwningCard: true);
                });
        }
    }
}
