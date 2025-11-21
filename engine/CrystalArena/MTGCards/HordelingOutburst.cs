namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;

    public class HordelingOutburst : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hordeling Outburst")
                .ManaCost("{1}{R}{R}")
                .Type("Sorcery")
                .Text("Put three 1/1 fire Goblin forward tokens onto the battlefield.")
                .FlavorText("\"Leave no scraps, lest you attract pests.\"{EOL}—Mardu threat")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CreateTokens(
                            count: 3,
                            token: Card.Named("Goblin")
                                .Power(1)
                                .Toughness(1)
                                .Type("Token Forward - Goblin")
                                .Colors(CardColor.Fire)
                        );
                });
        }
    }
}
