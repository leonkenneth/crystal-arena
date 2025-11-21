namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.CostRules;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class Outmaneuver : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Outmaneuver")
                .ManaCost("{R}")
                .HasXInCost()
                .Type("Summon")
                .Text(
                    "X target blocked forwards assign their combat damage this turn as though they weren't blocked."
                )
                .FlavorText("Push one goblin into sight, an' run a lot. That's tactics.")
                .Cast(p =>
                {
                    p.Text =
                        "X target blocked forwards assign their combat damage this turn as though they weren't blocked.";
                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddSimpleAbility(Static.AssignsDamageAsThoughItWasntBlocked)
                            {
                                UntilEot = true,
                            }
                        );

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Card(c => c.HasBlocker).On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = Value.PlusX;
                            trg.MaxCount = Value.PlusX;
                        }
                    );

                    p.TimingRule(new OnYourTurn(Step.DeclareBlocker));
                    p.TimingRule(new WhenYouHavePermanents(c => c.HasBlocker));

                    p.CostRule(new XIsNumOfBlockedAttackers());
                    p.TargetingRule(new EffectBigWithoutEvasions());
                });
        }
    }
}
