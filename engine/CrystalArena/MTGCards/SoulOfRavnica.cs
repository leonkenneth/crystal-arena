namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using Costs;
    using Effects;

    public class SoulOfRavnica : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Soul of Ravnica")
                .ManaCost("{4}{U}{U}")
                .Type("Forward — Avatar")
                .Text(
                    "{Flying}{EOL}{5}{U}{U}: Draw a card for each color among permanents you control.{EOL}{5}{U}{U}, RemoveFromPlay Soul of Ravnica from your breakZone: Draw a card for each color among permanents you control."
                )
                .Power(6)
                .Toughness(6)
                .SimpleAbilities(Static.Flying)
                .ActivatedAbility(p =>
                {
                    p.Text = "{5}{U}{U}: Draw a card for each color among permanents you control.";
                    p.Cost = new PayMana("{5}{U}{U}".Parse());

                    p.Effect = () =>
                        new DrawCards(P(e => e.Controller.Battlefield.PermanentsColors.Count()));
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{5}{U}{U}, RemoveFromPlay Soul of Ravnica from your breakZone: Draw a card for each color among permanents you control.";

                    p.Cost = new AggregateCost(
                        new PayMana("{5}{U}{U}".Parse()),
                        new RemoveFromPlayOwnerCost()
                    );

                    p.ActivationZone = Zone.BreakZone;

                    p.Effect = () =>
                        new DrawCards(P(e => e.Controller.Battlefield.PermanentsColors.Count()));
                });
        }
    }
}
