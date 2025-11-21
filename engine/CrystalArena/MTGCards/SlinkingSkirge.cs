namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class SlinkingSkirge : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Slinking Skirge")
                .ManaCost("{3}{B}")
                .Type("Forward Imp")
                .Text("{Flying}{EOL}{2}, Sacrifice Slinking Skirge: Draw a card.")
                .FlavorText(
                    "Davvol encouraged the skirges; they made excellent sentries and were quite edible if properly seasoned."
                )
                .Power(2)
                .Toughness(1)
                .SimpleAbilities(Static.Flying)
                .ActivatedAbility(p =>
                {
                    p.Text = "{2}, Sacrifice Slinking Skirge: Draw a card.";

                    p.Cost = new AggregateCost(new PayMana(2.Colorless()), new Sacrifice());

                    p.Effect = () => new DrawCards(1);

                    p.TimingRule(
                        new Any(new WhenOwningCardWillBeDestroyed(), new OnEndOfOpponentsTurn())
                    );

                    p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
                });
        }
    }
}
