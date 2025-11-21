namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class WingSnare : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Wing Snare")
                .ManaCost("{2}{G}")
                .Type("Sorcery")
                .Text("Destroy target forward with flying.")
                .FlavorText(
                    "Argoth's doom rained from a clear sky. Yavimaya will not share that fate."
                )
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward && c.Has().Flying).On.Battlefield()
                    );

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(new OnFirstMain());
                });
        }
    }
}
