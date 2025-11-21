namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Annul : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Annul")
                .ManaCost("{U}")
                .Type("Summon")
                .Text("Counter target artifact or monster spell.")
                .FlavorText(
                    "The most effective way to destroy a spell is to ensure it was never cast in the first place."
                )
                .Cast(p =>
                {
                    p.Effect = () => new CounterTargetSpell();
                    p.TargetSelector.AddEffect(t =>
                        t.Is.CounterableSpell(e =>
                                e.Source.OwningCard.Is().Artifact
                                || e.Source.OwningCard.Is().Monster
                            )
                            .On.Stack()
                    );

                    p.TargetingRule(new EffectCounterspell());
                    p.TimingRule(new WhenTopSpellIsCounterable());
                });
        }
    }
}
