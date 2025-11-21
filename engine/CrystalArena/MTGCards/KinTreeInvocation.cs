namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.AI.TimingRules;
    using Effects;
    using Modifiers;

    public class KinTreeInvocation : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Kin-Tree Invocation")
                .ManaCost("{B}{G}")
                .Type("Sorcery")
                .Text(
                    "Put an X/X dark and wind Spirit Warrior forward token onto the battlefield, where X is the greatest toughness among forwards you control."
                )
                .FlavorText(
                    "The passing years add new rings to the tree's trunk, bolstering the spirits that dwell within."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CreateTokens(
                            count: 1,
                            token: Card.Named("Spirit Warrior")
                                .Type("Token Forward - Spirit Warrior")
                                .Colors(CardColor.Dark, CardColor.Wind),
                            tokenParameters: (token, ctx) =>
                            {
                                token.Power(
                                    ctx.You.Battlefield.Forwards.Max(c =>
                                        c.Toughness.GetValueOrDefault()
                                    )
                                );
                                token.Toughness(
                                    ctx.You.Battlefield.Forwards.Max(c =>
                                        c.Toughness.GetValueOrDefault()
                                    )
                                );
                            }
                        );

                    p.TimingRule(
                        new WhenYouHavePermanents(c => c.Toughness.GetValueOrDefault() >= 2)
                    );
                });
        }
    }
}
