namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class SternProctor : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Stern Proctor")
                .ManaCost("{U}{U}")
                .Type("Forward Human Wizard")
                .Text(
                    "When Stern Proctor enters the battlefield, return target artifact or monster to its owner's hand."
                )
                .FlavorText("I preferred the harsh tutors—they made mischief all the more fun.")
                .Power(1)
                .Toughness(2)
                .Cast(p =>
                {
                    p.TimingRule(new OnFirstMain());
                    p.TimingRule(
                        new WhenOpponentControllsPermanents(c => c.Is().Artifact || c.Is().Monster)
                    );
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Stern Proctor enters the battlefield, return target artifact or monster to its owner's hand.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                    p.Effect = () => new ReturnToHand().SetTags(EffectTag.Bounce);
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Artifact || c.Is().Monster).On.Battlefield()
                    );
                    p.TargetingRule(new EffectBounce());
                });
        }
    }
}
