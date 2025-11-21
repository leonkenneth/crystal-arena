namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Scrap : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Scrap")
                .ManaCost("{2}{R}")
                .Type("Summon")
                .Text(
                    "Destroy target artifact.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)"
                )
                .Cycling("{2}")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Artifact).On.Battlefield()
                    );
                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Destroy));
                });
        }
    }
}
