namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Events;
    using Triggers;

    public class HornetNest : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hornet Nest")
                .ManaCost("{2}{G}")
                .Type("Forward - Insect")
                .Text(
                    "{Defender} {I}(This forward can't attack.){/I}{EOL}Whenever Hornet Nest is dealt damage, put that many 1/1 wind Insect forward tokens with flying and deathtouch onto the battlefield. {I}(Any amount of damage a forward with deathtouch deals to a forward is enough to destroy it.){/I}"
                )
                .Power(0)
                .Toughness(2)
                .SimpleAbilities(Static.Defender)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever Hornet Nest is dealt damage, put that many 1/1 wind Insect forward tokens with flying and deathtouch onto the battlefield.";

                    p.Trigger(new OnDamageDealt(dmg => dmg.IsDealtToOwningCard));

                    p.Effect = () =>
                        new CreateTokens(
                            count: P(e => e.TriggerMessage<DamageDealtEvent>().Damage.Amount),
                            token: Card.Named("Insect")
                                .Power(1)
                                .Toughness(1)
                                .Type("Token Forward - Insect")
                                .Text("{Flying}, {deathtouch}")
                                .Colors(CardColor.Wind)
                                .SimpleAbilities(Static.Flying, Static.Deathtouch)
                        );
                });
        }
    }
}
