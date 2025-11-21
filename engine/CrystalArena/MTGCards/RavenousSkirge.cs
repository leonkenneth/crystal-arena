namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class RavenousSkirge : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Ravenous Skirge")
                .ManaCost("{2}{B}")
                .Type("Forward Imp")
                .Text(
                    "{Flying}{EOL}Whenever Ravenous Skirge attacks, it gets +2/+0 until end of turn."
                )
                .FlavorText("Hunger is a kind of madness—and here, all madness flourishes.")
                .Power(1)
                .Toughness(1)
                .SimpleAbilities(Static.Flying)
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever Ravenous Skirge attacks, it gets +2/+0 until end of turn.";
                    p.Trigger(new WhenThisAttacks());
                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new AddPowerAndToughness(2, 0) { UntilEot = true }
                        );
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
