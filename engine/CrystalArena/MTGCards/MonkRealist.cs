namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Triggers;

    public class MonkRealist : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Monk Realist")
                .ManaCost("{1}{W}")
                .Type("Forward Human Monk Cleric")
                .Text("When Monk Realist enters the battlefield, destroy target monster.")
                .FlavorText("We plant the seeds of doubt to harvest the crop of wisdom.")
                .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[1])
                .Power(1)
                .Toughness(1)
                .Cast(p =>
                {
                    p.TimingRule(new OnFirstMain());
                    p.TimingRule(new WhenOpponentControllsPermanents(c => c.Is().Monster));
                })
                .TriggeredAbility(p =>
                {
                    p.Text = "When Monk Realist enters the battlefield, destroy target monster.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg => trg.Is.Monster().On.Battlefield());

                    p.TargetingRule(new EffectDestroy());
                });
        }
    }
}
