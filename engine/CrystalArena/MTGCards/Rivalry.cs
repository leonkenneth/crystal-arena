namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class Rivalry : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Rivalry")
                .ManaCost("{2}{R}")
                .Type("Monster")
                .Text(
                    "At the beginning of each player's upkeep, if that player controls more backups than each other player, Rivalry deals 2 damage to him or her."
                )
                .FlavorText(
                    "The goblins revered it; the viashino defended it. Only Urza understood it."
                )
                .Cast(p =>
                {
                    p.TimingRule(new OnSecondMain());
                    p.TimingRule(new WhenYouHaveMorePermanents(c => c.Is().Backup));
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of each player's upkeep, if that player controls more backups than each other player, Rivalry deals 2 damage to him or her.";

                    p.Trigger(
                        new OnStepStart(Step.Upkeep, activeTurn: true, passiveTurn: true)
                        {
                            Condition = ctx =>
                            {
                                var activeCount = ctx.Players.Active.Battlefield.Backups.Count();
                                var passiveCount = ctx.Players.Passive.Battlefield.Backups.Count();
                                return activeCount > passiveCount;
                            },
                        }
                    );

                    p.Effect = () => new DealDamageToPlayer(2, P((e, g) => g.Players.Active));
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
