namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Infrastructure;
    using Triggers;

    public class SiegeDragon : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Siege Dragon")
                .ManaCost("{5}{R}{R}")
                .Type("Forward — Dragon")
                .Text(
                    "{Flying}{EOL}When Siege Dragon enters the battlefield, destroy all Walls your opponents control.{EOL}Whenever Siege Dragon attacks, if defending player controls no Walls, it deals 2 damage to each forward without flying that player controls."
                )
                .Power(5)
                .Toughness(5)
                .SimpleAbilities(Static.Flying)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Siege Dragon enters the battlefield, destroy all Walls your opponents control.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new DestroyAllPermanents(
                            (c, ctx) => c.Is("wall") && c.Controller == ctx.Opponent
                        );
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever Siege Dragon attacks, if defending player controls no Walls, it deals 2 damage to each forward without flying that player controls.";

                    p.Trigger(
                        new WhenThisAttacks
                        {
                            Condition = ctx =>
                                ctx.Players.Passive.Battlefield.Forwards.None(c => c.Is("wall")),
                        }
                    );

                    p.Effect = () =>
                        new DealDamageToForwardsAndPlayers(
                            amountForward: 2,
                            filterForward: (e, card) =>
                                !card.Has().Flying && !card.Controller.IsActive
                        );
                });
        }
    }
}
