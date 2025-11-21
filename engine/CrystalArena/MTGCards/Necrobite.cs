namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class Necrobite : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Necrobite")
                .ManaCost("{2}{B}")
                .Type("Summon")
                .Text(
                    "Target forward gains deathtouch until end of turn. Regenerate it. {I}(The next time that forward would be destroyed this turn, it isn't. Instead tap it, remove all damage from it, and remove it from combat. Any amount of damage a forward with deathtouch deals to a forward is enough to destroy it.){/I}"
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new ApplyModifiersToTargets(() =>
                                new AddSimpleAbility(Static.Deathtouch) { UntilEot = true }
                            ),
                            new RegenerateTarget()
                        );

                    p.TargetSelector.AddEffect(s => s.Is.Forward().On.Battlefield());

                    p.TimingRule(new RegenerateTargetTimingRule());
                    p.TargetingRule(new EffectGiveRegenerate());
                });
        }
    }
}
