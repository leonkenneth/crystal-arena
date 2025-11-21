namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.RepetitionRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Infrastructure;
    using CrystalArena.Triggers;

    public class Pestilence : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Pestilence")
                .ManaCost("{2}{B}{B}")
                .Type("Monster")
                .Text(
                    "At the beginning of the end step, if no forwards are on the battlefield, sacrifice Pestilence.{EOL}{B}: Pestilence deals 1 damage to each forward and each player."
                )
                .Cast(p =>
                {
                    p.TimingRule(new OnFirstMain());
                    p.TimingRule(new WhenYouDontControlSamePermanent());
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of the end step, if no forwards are on the battlefield, sacrifice Pestilence.";
                    p.Trigger(
                        new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true)
                        {
                            Condition = ctx => ctx.Players.Permanents().None(x => x.Is().Forward),
                        }
                    );
                    p.Effect = () => new SacrificeOwner();

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text = "{B}: Pestilence deals 1 damage to each forward and each player.";
                    p.Cost = new PayMana(Mana.Dark, supportsRepetitions: true);
                    p.Effect = () =>
                        new DealDamageToForwardsAndPlayers(amountForward: 1, amountPlayer: 1);

                    p.TimingRule(new MassRemovalTimingRule(removalTag: EffectTag.DealDamage));
                    p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
                    p.RepetitionRule(new RepeatForOptimalMassDamage());
                });
        }
    }
}
