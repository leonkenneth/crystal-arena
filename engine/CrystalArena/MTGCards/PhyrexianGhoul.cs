namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class PhyrexianGhoul : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Phyrexian Ghoul")
                .ManaCost("{2}{B}")
                .Type("Forward - Zombie")
                .Text("Sacrifice a forward: Phyrexian Ghoul gets +2/+2 until end of turn.")
                .FlavorText("Phyrexia wastes nothing. Its food chain is a spiraling cycle.")
                .Power(2)
                .Toughness(2)
                .ActivatedAbility(p =>
                {
                    p.Text = "Sacrifice a forward: Phyrexian Ghoul gets +2/+2 until end of turn.";
                    p.Cost = new Sacrifice();
                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new AddPowerAndToughness(2, 2) { UntilEot = true }
                        ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

                    p.TargetSelector.AddCost(trg =>
                        trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield()
                    );

                    p.TimingRule(new PumpOwningCardTimingRule(2, 2));
                    p.TargetingRule(
                        new EffectOrCostRankBy(c => c.Score)
                        {
                            TargetLimit = 1,
                            ConsiderTargetingSelf = false,
                        }
                    );
                });
        }
    }
}
