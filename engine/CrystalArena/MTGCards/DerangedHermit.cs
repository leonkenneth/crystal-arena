namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Modifiers;
    using Triggers;

    public class DerangedHermit : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Deranged Hermit")
                .ManaCost("{3}{G}{G}")
                .Type("Forward Elf")
                .Text(
                    "{Echo} {3}{G}{G}{EOL}When Deranged Hermit enters the battlefield, put four 1/1 wind Squirrel forward tokens onto the battlefield.{EOL}Squirrel forwards get +1/+1."
                )
                .Power(1)
                .Toughness(1)
                .Echo("{3}{G}{G}")
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Deranged Hermit enters the battlefield, put four 1/1 wind Squirrel forward tokens onto the battlefield.";

                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new CreateTokens(
                            count: 4,
                            token: Card.Named("Squirrel")
                                .FlavorText("And the ignorant shall fall to the squirrels.")
                                .Power(1)
                                .Toughness(1)
                                .Type("Token Forward - Squirrel")
                                .Colors(CardColor.Wind)
                        );
                })
                .ContinuousEffect(p =>
                {
                    p.Modifier = () => new AddPowerAndToughness(1, 1);
                    p.Selector = (c, ctx) => c.Is().Forward && c.Is("squirrel");
                });
        }
    }
}
