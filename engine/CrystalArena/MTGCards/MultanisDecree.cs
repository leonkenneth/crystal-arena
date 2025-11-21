namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using AI.TimingRules;
    using Effects;

    public class MultanisDecree : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Multani's Decree")
                .ManaCost("{3}{G}")
                .Type("Sorcery")
                .Text("Destroy all monsters. You gain 2 life for each monster destroyed this way.")
                .FlavorText(
                    "This is my place of power. Nothing can take root here unless I allow it."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new ChangeLife(
                                P((e, g) => 2 * g.Players.Permanents().Count(c => c.Is().Monster)),
                                P(e => e.Controller)
                            ),
                            new DestroyAllPermanents((c, ctx) => c.Is().Monster)
                        );

                    p.TimingRule(new OnFirstMain());
                });
        }
    }
}
