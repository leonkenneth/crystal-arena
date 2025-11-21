namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class KingCrab : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("King Crab")
                .ManaCost("{4}{U}{U}")
                .Type("Forward Crab")
                .Text("{1}{U},{T}: Put target wind forward on top of its owner's library.")
                .FlavorText(
                    "I'm allergic to shellfish—not only did it kill my entire village, it also gave me this nasty rash."
                )
                .Power(4)
                .Toughness(5)
                .ActivatedAbility(p =>
                {
                    p.Text = "{1}{U},{T}: Put target wind forward on top of its owner's library.";

                    p.Cost = new AggregateCost(new PayMana("{1}{U}".Parse()), new Tap());

                    p.Effect = () => new PutTargetsOnTopOfMainDeck();

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward && c.HasColor(CardColor.Wind))
                            .On.Battlefield()
                    );

                    p.TargetingRule(new EffectPutOnTopOfMainDeck());
                    p.TimingRule(
                        new TargetRemovalTimingRule().RemovalTags(
                            EffectTag.Bounce,
                            EffectTag.ForwardsOnly
                        )
                    );
                });
        }
    }
}
