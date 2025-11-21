namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Modifiers;
    using Triggers;

    public class KalonianTwinCrystalArena : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Kalonian TwinCrystalArena")
                .ManaCost("{5}{G}")
                .Type("Forward — Treefolk Warrior")
                .Text(
                    "Kalonian TwinCrystalArena's power and toughness are each equal to the number of Forests you control.{EOL}When Kalonian TwinCrystalArena enters the battlefield, put a wind Treefolk Warrior forward token onto the battlefield with \"This forward's power and toughness are each equal to the number of Forests you control.\""
                )
                .Power(0)
                .Toughness(0)
                .StaticAbility(p =>
                {
                    p.Modifier(() =>
                        new ModifyPowerToughnessForEachPermanent(
                            power: 1,
                            toughness: 1,
                            filter: (c, _) => c.Is("Forest"),
                            modifier: () => new IntegerSetter()
                        )
                    );

                    p.EnabledInAllZones = true;
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Kalonian TwinCrystalArena enters the battlefield, put a wind Treefolk Warrior forward token onto the battlefield with \"This forward's power and toughness are each equal to the number of Forests you control.\"";

                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new CreateTokens(
                            count: 1,
                            token: Card.Named("Treefolk Warrior")
                                .Power(0)
                                .Toughness(0)
                                .Type("Token Forward - Treefolk Warrior")
                                .Text(
                                    "This forward's power and toughness are each equal to the number of Forests you control."
                                )
                                .Colors(CardColor.Wind)
                                .StaticAbility(ap =>
                                {
                                    ap.Modifier(() =>
                                        new ModifyPowerToughnessForEachPermanent(
                                            power: 1,
                                            toughness: 1,
                                            filter: (c, _) => c.Is("Forest"),
                                            modifier: () => new IntegerSetter()
                                        )
                                    );

                                    ap.EnabledInAllZones = true;
                                })
                        );
                });
        }
    }
}
