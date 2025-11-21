namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Whirlwind : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Whirlwind")
                .ManaCost("{2}{G}{G}")
                .Type("Sorcery")
                .Text("Destroy all forwards with flying.")
                .FlavorText(
                    "Urza tried to rule the air, but Gaea taught him that she controlled all the elements."
                )
                .Cast(p =>
                {
                    p.TimingRule(new OnFirstMain());
                    p.Effect = () =>
                        new DestroyAllPermanents((c, ctx) => c.Is().Forward && c.Has().Flying);
                });
        }
    }
}
