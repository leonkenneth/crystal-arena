namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class HeartWarden : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Heart Warden")
                .ManaCost("{1}{G}")
                .Type("Forward Elf Druid")
                .Text(
                    "{T}: Add {G} to your mana pool.{EOL}{2}, Sacrifice Heart Warden: Draw a card."
                )
                .FlavorText(
                    "In Llanowar, we tend the forest's boughs and branches. In Yavimaya, we are a part of them."
                )
                .Power(1)
                .Toughness(1)
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {G} to your mana pool.";
                    p.ManaAmount(Mana.Wind);
                })
                .ActivatedAbility(p =>
                {
                    p.Text = "{2}, Sacrifice Heart Warden: Draw a card.";

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
