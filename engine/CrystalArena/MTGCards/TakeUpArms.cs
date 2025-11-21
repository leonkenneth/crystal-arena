namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;

    public class TakeUpArms : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Take Up Arms")
                .ManaCost("{4}{W}")
                .Type("Summon")
                .Text("Put three 1/1 light Warrior forward tokens onto the battlefield.")
                .FlavorText("\"Many scales make the skin of a dragon.\"{EOL}—Abzan wisdom")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CreateTokens(
                            count: 3,
                            token: Card.Named("Warrior")
                                .Power(1)
                                .Toughness(1)
                                .Type("Token Forward - Warrior")
                                .Colors(CardColor.Light)
                        );

                    p.TimingRule(
                        new Any(new AfterOpponentDeclaresAttackers(), new OnEndOfOpponentsTurn())
                    );
                });
        }
    }
}
