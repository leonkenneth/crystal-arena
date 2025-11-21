namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class HuntingMoa : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hunting Moa")
                .ManaCost("{2}{G}")
                .Type("Forward Bird Beast")
                .Text(
                    "{Echo} {2}{G}{EOL}When Hunting Moa enters the battlefield or dies, put a +1/+1 counter on target forward."
                )
                .Power(3)
                .Toughness(2)
                .Echo("{2}{G}")
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Hunting Moa enters the battlefield or dies, put a +1/+1 counter on target forward.";

                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));

                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddCounters(() => new PowerToughness(1, 1), count: 1)
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
