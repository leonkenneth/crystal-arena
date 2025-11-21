namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Modifiers;

    public class BattleBrawler : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Battle Brawler")
                .ManaCost("{1}{B}")
                .Type("Forward — Orc Warrior")
                .Text(
                    "As long as you control a fire or light permanent, Battle Brawler gets +1/+0 and has first strike."
                )
                .FlavorText(
                    "Every time he returns from battle unscathed, he feels a tinge of disappointment."
                )
                .Power(2)
                .Toughness(2)
                .StaticAbility(p =>
                {
                    p.Modifier(() => new AddSimpleAbility(Static.FirstStrike));
                    p.Modifier(() => new AddPowerAndToughness(1, 0));
                    p.Condition = cond =>
                        cond.OwnerControlsPermanent(c =>
                            c.HasColor(CardColor.Fire) || c.HasColor(CardColor.Light)
                        );
                });
        }
    }
}
