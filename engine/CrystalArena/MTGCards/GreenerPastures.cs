namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class GreenerPastures : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Greener Pastures")
                .ManaCost("{2}{G}")
                .Type("Monster")
                .Text(
                    "At the beginning of each player's upkeep, if that player controls more backups than each other player, the player puts a 1/1 wind Saproling forward token onto the battlefield."
                )
                .Cast(p =>
                {
                    p.TimingRule(new OnSecondMain());
                    p.TimingRule(new WhenYouHaveMorePermanents(c => c.Is().Backup));
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of each player's upkeep, if that player controls more backups than each other player, the player puts a 1/1 wind Saproling forward token onto the battlefield.";

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

                    p.Effect = () =>
                        new CreateTokens(
                            count: 1,
                            tokenController: P((e, g) => g.Players.Active),
                            token: Card.Named("Saproling")
                                .FlavorText(
                                    "The nauseating wriggling of a saproling is exceeded only by the nauseating wriggling of its prey."
                                )
                                .Power(1)
                                .Toughness(1)
                                .Type("Token Forward - Saproling")
                                .Colors(CardColor.Wind)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
