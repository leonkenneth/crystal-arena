namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Triggers;

    public class WardenOfTheEye : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Warden of the Eye")
                .ManaCost("{2}{U}{R}{W}")
                .Type("Forward — Djinn Wizard")
                .Text(
                    "When Warden of the Eye enters the battlefield, return target nonforward, nonland card from your breakZone to your hand."
                )
                .FlavorText(
                    "The wardens guard the sacred documents of Tarkir's history, though they are forbidden to read the words."
                )
                .Power(3)
                .Toughness(3)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Warden of the Eye enters the battlefield, return target nonforward, nonland card from your breakZone to your hand.";

                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () => new ReturnToHand();

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => !c.Is().Forward && !c.Is().Backup).In.YourBreakZone()
                    );

                    p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
                    p.TimingRule(
                        new WhenYourBreakZoneCountIs(
                            c => !c.Is().Forward && !c.Is().Backup,
                            minCount: 1
                        )
                    );
                });
        }
    }
}
