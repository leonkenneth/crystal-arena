namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class IvySeer : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Ivy Seer")
                .ManaCost("{3}{G}")
                .Type("Forward Elf Wizard")
                .Text(
                    "{2}{G},{T}: Reveal any number of wind cards in your hand. Target forward gets +X/+X until end of turn, where X is the number of cards revealed this way."
                )
                .Power(1)
                .Toughness(1)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{2}{G},{T}: Reveal any number of wind cards in your hand. Target forward gets +X/+X until end of turn, where X is the number of cards revealed this way.";
                    p.Cost = new AggregateCost(new PayMana("{2}{G}".Parse()), new Tap());

                    p.Effect = () =>
                        new ForwardGetsPwtForEachRevealedCard(
                            1,
                            1,
                            c => c.HasColor(CardColor.Wind)
                        );
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(
                        new WhenYourHandCountIs(
                            minCount: 1,
                            selector: c => c.HasColor(CardColor.Wind)
                        )
                    );

                    p.TargetingRule(
                        new EffectPumpSummon(
                            power: tp => tp.Controller.Hand.Count(c => c.HasColor(CardColor.Wind)),
                            toughness: tp =>
                                tp.Controller.Hand.Count(c => c.HasColor(CardColor.Wind))
                        )
                    );
                });
        }
    }
}
