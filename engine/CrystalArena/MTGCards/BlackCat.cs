namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class BlackCat : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Dark Cat")
                .ManaCost("{1}{B}")
                .Type("Forward — Zombie Cat")
                .Text("When Dark Cat dies, target opponent discards a card at random.")
                .FlavorText("Its last life is spent tormenting your dreams.")
                .Power(1)
                .Toughness(1)
                .TriggeredAbility(p =>
                {
                    p.Text = "When Dark Cat dies, target opponent discards a card at random.";
                    p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.BreakZone));
                    p.Effect = () => new OpponentDiscardsCards(randomCount: 1);
                });
        }
    }
}
