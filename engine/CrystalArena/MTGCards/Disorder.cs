namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Disorder : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Disorder")
                .ManaCost("{1}{R}")
                .Type("Sorcery")
                .Text(
                    "Disorder deals 2 damage to each light forward and each player who controls a light forward."
                )
                .FlavorText("Then, just when the other guys were winnin', the sky threw up.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new DealDamageToForwardsAndPlayers(
                            amountPlayer: 2,
                            amountForward: 2,
                            filterPlayer: (e, player) =>
                                player.Battlefield.Any(card =>
                                    card.Is().Forward && card.HasColor(CardColor.Light)
                                ),
                            filterForward: (e, c) => c.HasColor(CardColor.Light)
                        );

                    p.TimingRule(
                        new WhenOpponentControllsPermanents(c =>
                            c.Is().Forward && c.HasColor(CardColor.Light)
                        )
                    );
                });
        }
    }
}
