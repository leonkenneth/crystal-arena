namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Events;
    using CrystalArena.Triggers;

    public class NoMercy : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("No Mercy")
                .ManaCost("{2}{B}{B}")
                .Type("Monster")
                .Text("Whenever a forward deals damage to you, destroy it.")
                .FlavorText("We had years to prepare, while they had mere minutes.")
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever a forward deals damage to you, destroy it.";

                    p.Trigger(
                        new OnDamageDealt(dmg => dmg.IsDealtToYou && dmg.Source.Is().Forward)
                    );

                    p.Effect = () =>
                        new DestroyPermanent(
                            P(e => e.TriggerMessage<DamageDealtEvent>().Damage.Source.Cards)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
