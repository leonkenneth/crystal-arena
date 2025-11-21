namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Events;
    using CrystalArena.Triggers;

    public class Oppression : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Oppression")
                .ManaCost("{1}{B}{B}")
                .Type("Monster")
                .Text("Whenever a player casts a spell, that player discards a card.")
                .FlavorText("Do not presume to speak for yourself.")
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever a player casts a spell, that player discards a card.";
                    p.Trigger(new OnCastedSpell());
                    p.Effect = () =>
                        new DiscardCards(
                            1,
                            P(e => e.TriggerMessage<SpellPutOnStackEvent>().Card.Controller)
                        );
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
