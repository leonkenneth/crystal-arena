namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class KrenkosEnforcer : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Krenko's Enforcer")
                .ManaCost("{1}{R}{R}")
                .Type("Forward — Goblin Warrior")
                .Text(
                    "{Intimidate}{I}(This forward can't be blocked except by artifact forwards and/or forwards that share a color with it.){/I}"
                )
                .FlavorText("He just likes to break legs. Collecting the debt is a bonus.")
                .Power(2)
                .Toughness(2)
                .SimpleAbilities(Static.Intimidate);
        }
    }
}
