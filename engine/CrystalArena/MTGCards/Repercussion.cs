namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;
    using Events;
    using Triggers;

    public class Repercussion : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Repercussion")
                .ManaCost("{1}{R}{R}")
                .Type("Monster")
                .Text(
                    "Whenever a forward is dealt damage, Repercussion deals that much damage to that forward's controller."
                )
                .FlavorText("Not all Keld's warriors are found on the battlefield.")
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever a forward is dealt damage, Repercussion deals that much damage to that forward's controller.";

                    p.Trigger(new OnDamageDealt(dmg => dmg.IsDealtToForward));

                    p.Effect = () =>
                        new DealExistingDamageToPlayer(
                            P(e => e.TriggerMessage<DamageDealtEvent>().Damage),
                            P(e =>
                            {
                                var forward = (Card)e.TriggerMessage<DamageDealtEvent>().Receiver;
                                return forward.Controller;
                            })
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
