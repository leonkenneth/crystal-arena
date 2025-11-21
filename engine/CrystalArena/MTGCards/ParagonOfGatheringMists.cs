namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;

    public class ParagonOfGatheringMists : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Paragon of Gathering Mists")
                .ManaCost("{3}{U}")
                .Type("Forward — Human Wizard")
                .Text(
                    "Other water forwards you control get +1/+1.{EOL}{U},{T}: Another target water forward you control gains flying until end of turn."
                )
                .Power(2)
                .Toughness(2)
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CastPermanent().SetTags(
                            EffectTag.IncreasePower,
                            EffectTag.IncreaseToughness
                        );
                })
                .ContinuousEffect(p =>
                {
                    p.Modifier = () => new AddPowerAndToughness(1, 1);
                    p.Selector = (c, ctx) =>
                        c.Controller == ctx.You
                        && c.Is().Forward
                        && c.HasColor(CardColor.Water)
                        && c != ctx.Source;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{U},{T}: Another target water forward you control gains flying until end of turn.";
                    p.Cost = new AggregateCost(new PayMana("{U}".Parse()), new Tap());

                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddSimpleAbility(Static.Flying) { UntilEot = true }
                        );

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(
                                c => c.Is().Forward && c.HasColor(CardColor.Water),
                                controlledBy: ControlledBy.SpellOwner,
                                canTargetSelf: false
                            )
                            .On.Battlefield()
                    );

                    p.TimingRule(new BeforeYouDeclareAttackers());
                    p.TargetingRule(new EffectBigWithoutEvasions(c => !c.Has().Flying));
                });
        }
    }
}
