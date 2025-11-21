namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class TurnToFrog : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Turn to Frog")
                .ManaCost("{1}{U}")
                .Type("Summon")
                .Text(
                    "Until end of turn, target forward loses all abilities and becomes a water Frog with base power and toughness 1/1."
                )
                .FlavorText("\"Ribbit.\"")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToTargets(
                            () =>
                                new ChangeToForward(
                                    power: 1,
                                    toughness: 1,
                                    colors: L(CardColor.Water),
                                    type: t => t.Change(subTypes: "frog")
                                )
                                {
                                    UntilEot = true,
                                },
                            () =>
                                new DisableAllAbilities(
                                    activated: true,
                                    simple: true,
                                    triggered: true
                                )
                                {
                                    UntilEot = true,
                                }
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(
                        new TargetRemovalTimingRule(removalTag: EffectTag.Humble, combatOnly: true)
                    );
                });
        }
    }
}
