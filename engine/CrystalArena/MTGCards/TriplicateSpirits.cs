namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;

    public class TriplicateSpirits : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Triplicate Spirits")
                .ManaCost("{4}{W}{W}")
                .Type("Sorcery")
                .Text(
                    "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Put three 1/1 light Spirit forward tokens with flying onto the battlefield.{I}(They can't be blocked except by forwards with flying or reach.){/I}"
                )
                .FlavorText("Nature is itself wild—in all its forms.")
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CreateTokens(
                            count: 3,
                            token: Card.Named("Spirit")
                                .Power(1)
                                .Toughness(1)
                                .Type("Token Forward - Spirit")
                                .Text("{Flying}")
                                .SimpleAbilities(Static.Flying)
                                .Colors(CardColor.Light)
                        );
                });
        }
    }
}
