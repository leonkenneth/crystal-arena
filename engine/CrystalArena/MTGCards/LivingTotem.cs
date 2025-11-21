namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Effects;
    using Modifiers;
    using Triggers;

    public class LivingTotem : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Living Totem")
                .ManaCost("{3}{G}")
                .Type("Forward — Plant Elemental")
                .Text(
                    "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}When Living Totem enters the battlefield, you may put a +1/+1 counter on another target forward."
                )
                .Power(2)
                .Toughness(3)
                .SimpleAbilities(Static.Convoke)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Living Totem enters the battlefield, you may put a +1/+1 counter on another target forward.";

                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddCounters(() => new PowerToughness(1, 1), count: 1)
                        );

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward(canTargetSelf: false).On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = 0;
                            trg.MaxCount = 1;
                            trg.Message = "Select another target forward";
                        }
                    );

                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
