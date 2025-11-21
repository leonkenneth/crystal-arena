namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Costs;

    public class WizardMentor : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Wizard Mentor")
                .ManaCost("{2}{U}")
                .Type("Forward Human Wizard")
                .Text(
                    "{T}: Return Wizard Mentor and target forward you control to their owner's hand."
                )
                .FlavorText(
                    "Although some of the students quickly grasped the concept, the others could summon only blackboards."
                )
                .Power(2)
                .Toughness(2)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{T}: Return Wizard Mentor and target forward you control to their owner's hand.";
                    p.Cost = new Tap();
                    p.Effect = () => new Effects.ReturnToHand(returnOwningCard: true);
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield()
                    );
                    p.TargetingRule(new EffectBounceAlongWithOwningCard());
                });
        }
    }
}
