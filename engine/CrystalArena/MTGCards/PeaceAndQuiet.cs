namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class PeaceAndQuiet : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Peace and Quiet")
                .ManaCost("{1}{W}")
                .Type("Summon")
                .Text("Destroy two target monsters.")
                .FlavorText(
                    "In time our realm will shine again. But it will gleam only when we scour away the taint of doubt."
                )
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Monster().On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = 2;
                            trg.MaxCount = 2;
                        }
                    );

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.Destroy));
                });
        }
    }
}
