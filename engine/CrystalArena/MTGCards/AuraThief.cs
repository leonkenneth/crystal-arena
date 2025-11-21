namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class AuraThief : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Aura Thief")
                .ManaCost("{3}{U}")
                .Type("Forward Illusion")
                .Text(
                    "{Flying}{EOL}When Aura Thief dies, you gain control of all monsters. (You don't get to move Auras.)"
                )
                .FlavorText("Illusion steals reality from the unwise.")
                .Power(2)
                .Toughness(2)
                .TriggeredAbility(p =>
                {
                    p.Text = "When Aura Thief dies, you gain control of all monsters.";
                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
                    p.Effect = () => new GainControlOfAllPermanents((c, e) => c.Is().Monster);
                });
        }
    }
}
