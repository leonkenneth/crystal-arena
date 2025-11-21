namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Undermine : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Undermine")
                .ManaCost("{U}{U}{B}")
                .Type("Summon")
                .Text("Counter target spell. Its controller loses 3 life.")
                .FlavorText("'Which would you like first, the insult or the injury?'")
                .Cast(p =>
                {
                    p.Effect = () => new CounterTargetSpell(ep => ep.ControllerLifeloss = 3);
                    p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());
                    p.TimingRule(new WhenTopSpellIsCounterable());
                    p.TargetingRule(new EffectCounterspell());
                });
        }
    }
}
