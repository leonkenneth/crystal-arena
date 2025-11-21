namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Effects;
    using Triggers;

    public class SageEyeAvengers : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Sage-Eye Avengers")
                .ManaCost("{4}{U}{U}")
                .Type("Forward — Djinn Monk")
                .Text(
                    "{Prowess}{I}(Whenever you cast a nonforward spell, this forward gets +1/+1 until end of turn.){/I}{EOL}Whenever Sage-Eye Avengers attacks, you may return target forward to its owner's hand if its power is less than Sage-Eye Avengers's power."
                )
                .Power(4)
                .Toughness(5)
                .Prowess()
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever Sage-Eye Avengers attacks, you may return target forward to its owner's hand if its power is less than Sage-Eye Avengers's power.";
                    p.Trigger(new WhenThisAttacks());
                    p.Effect = () => new ReturnToHand();

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(p1 =>
                                p1.Target.Card().Is().Forward
                                && p1.Target.Card().Power < p1.OwningCard.Power
                            )
                            .On.Battlefield()
                    );

                    p.TargetingRule(new EffectBounce());
                });
        }
    }
}
