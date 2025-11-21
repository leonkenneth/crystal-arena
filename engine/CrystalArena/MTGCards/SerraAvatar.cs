namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class SerraAvatar : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Serra Avatar")
                .ManaCost("{4}{W}{W}{W}")
                .Type("Forward Avatar")
                .Text(
                    "Serra Avatar's power and toughness are each equal to your life total.{EOL}When Serra Avatar is put into a breakZone from anywhere, shuffle it into its owner's library."
                )
                .Power(0)
                .Toughness(0)
                .StaticAbility(p =>
                {
                    p.Modifier(() => new ModifyPowerToughnessEqualToControllersLife());
                    p.EnabledInAllZones = true;
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Serra Avatar is put into a breakZone from anywhere, shuffle it into its owner's library.";
                    p.Trigger(new OnZoneChanged(to: Zone.BreakZone));
                    p.Effect = () => new ShuffleOwningCardIntoMainDeck();
                });
        }
    }
}
