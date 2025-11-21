namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class TimeSpiral : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Time Spiral")
                .ManaCost("{4}{U}{U}")
                .Type("Sorcery")
                .Text(
                    "RemoveFromPlay Time Spiral. Each player shuffles his or her breakZone and hand into his or her library, then draws seven cards. You untap up to six backups."
                )
                .Cast(p =>
                {
                    p.AfterResolve = (c, _) => c.RemoveFromPlay(null);
                    p.Effect = () =>
                        new CompoundEffect(
                            new EachPlayerShufflesHandAndBreakZoneIntoMainDeckAndDrawsCards(7),
                            new UntapSelectedPermanents(0, 6, c => c.Is().Backup)
                        );

                    p.TimingRule(new OnFirstMain());

                    p.TimingRule(new WhenYourHandCountIs(maxCount: 3));
                    p.TimingRule(new WhenOpponentsHandCountIs(minCount: 2));
                });
        }
    }
}
