namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class PhyrexianDebaser : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Phyrexian Debaser")
                .ManaCost("{3}{B}")
                .Type("Forward Carrier")
                .Text(
                    "{Flying}{EOL}{T}, Sacrifice Phyrexian Debaser: Target forward gets -2/-2 until end of turn."
                )
                .FlavorText(
                    "The second stage of the illness: high fever and severe infectiousness."
                )
                .Power(2)
                .Toughness(2)
                .SimpleAbilities(Static.Flying)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{T}, Sacrifice Phyrexian Debaser: Target forward gets -2/-2 until end of turn.";
                    p.Cost = new AggregateCost(new Tap(), new Sacrifice());

                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddPowerAndToughness(-2, -2) { UntilEot = true }
                        )
                        {
                            ToughnessReduction = 2,
                        };

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TargetingRule(new EffectReduceToughness(2));

                    p.TimingRule(
                        new Any(
                            new WhenOwningCardWillBeDestroyed(),
                            new TargetRemovalTimingRule(
                                removalTag: EffectTag.ReduceToughness,
                                combatOnly: true
                            )
                        )
                    );
                });
        }
    }
}
