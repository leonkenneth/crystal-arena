namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class FlyingCraneTechnique : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Flying Crane Technique")
                .ManaCost("{3}{U}{R}{W}")
                .Type("Summon")
                .Text(
                    "Untap all forwards you control. They gain flying and double strike until end of turn."
                )
                .FlavorText(
                    "There are many Jeskai styles: Riverwalk imitates flowing water, Dragonfist the ancient hellkites, and Flying Crane the wild aven of the high peaks."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new UntapEachPermanent(
                                filter: c => c.Is().Forward,
                                controlledBy: ControlledBy.SpellOwner
                            ),
                            new ApplyModifiersToPermanents(
                                selector: (c, ctx) => c.Is().Forward && ctx.You == c.Controller,
                                modifiers: new CardModifierFactory[]
                                {
                                    () =>
                                        new AddSimpleAbility(Static.Flying) { UntilEot = true },
                                    () =>
                                        new AddSimpleAbility(Static.DoubleStrike)
                                        {
                                            UntilEot = true,
                                        },
                                }
                            )
                        );

                    p.TimingRule(
                        new Any(
                            new BeforeYouDeclareAttackers(),
                            new AfterOpponentDeclaresAttackers()
                        )
                    );
                });
        }
    }
}
