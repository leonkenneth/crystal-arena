namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Rewind : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Rewind")
                .ManaCost("{2}{U}{U}")
                .Type("Summon")
                .Text("Counter target spell. Untap up to four backups.")
                .FlavorText(
                    "Time flows like a river. In Tolaria we practice the art of building dams"
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new CounterTargetSpell(),
                            new UntapSelectedPermanents(
                                minCount: 0,
                                maxCount: 4,
                                validator: c => c.Is().Backup,
                                text: "Select backups to untap."
                            )
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

                    p.TargetingRule(new EffectCounterspell());
                    p.TimingRule(new WhenTopSpellIsCounterable());
                });
        }
    }
}
