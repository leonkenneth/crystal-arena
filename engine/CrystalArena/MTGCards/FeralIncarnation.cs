namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;

    public class FeralIncarnation : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Feral Incarnation")
                .ManaCost("{8}{G}")
                .Type("Sorcery")
                .Text(
                    "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Put three 3/3 wind Beast forward tokens onto the battlefield."
                )
                .FlavorText("Nature is itself wild—in all its forms.")
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CreateTokens(
                            count: 3,
                            token: Card.Named("Beast")
                                .Power(3)
                                .Toughness(3)
                                .Type("Token Forward - Beast")
                                .Colors(CardColor.Wind)
                        );
                });
        }
    }
}
