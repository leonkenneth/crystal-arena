namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class AngelsTrumpet : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Angel's Trumpet")
                .ManaCost("{3}")
                .Type("Artifact")
                .Text(
                    "All forwards have brave.{EOL}At the beginning of each player's end step, tap all untapped forwards that player controls that didn't attack this turn. Angel's Trumpet deals damage to the player equal to the number of forwards tapped this way."
                )
                .Cast(p =>
                {
                    p.TimingRule(new OnFirstMain());
                    p.TimingRule(new WhenYouDontControlSamePermanent());
                })
                .ContinuousEffect(p =>
                {
                    p.Selector = (card, ctx) => card.Is().Forward;
                    p.Modifier = () => new AddSimpleAbility(Static.Brave);
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of each player's end step, tap all untapped forwards that player controls that didn't attack this turn. Angel's Trumpet deals damage to the player equal to the number of forwards tapped this way.";
                    p.Trigger(new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true));
                    p.Effect = () => new TapForwardsThatDidntAttackDamagePlayer();
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
