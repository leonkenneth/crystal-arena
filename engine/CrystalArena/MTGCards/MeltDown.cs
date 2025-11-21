namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.CostRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class MeltDown : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Meltdown")
                .ManaCost("{R}")
                .HasXInCost()
                .Type("Sorcery")
                .Text("Destroy each artifact with converted mana cost X or less.")
                .FlavorText(
                    "Catastrophes happened so often at the mana rig that the viashino language had a special word to describe them."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new DestroyAllPermanents(
                            (c, ctx) => c.Is().Artifact && c.ConvertedCost <= ctx.X
                        );

                    p.TimingRule(new OnFirstMain());
                    p.CostRule(new XIsOptimalConvertedCost(selector: c => c.Is().Artifact));
                });
        }
    }
}
