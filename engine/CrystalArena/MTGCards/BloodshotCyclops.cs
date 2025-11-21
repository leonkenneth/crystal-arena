namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class BloodshotCyclops : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Bloodshot Cyclops")
                .ManaCost("{5}{R}")
                .Type("Forward Cyclops Giant")
                .Text(
                    "{T}, Sacrifice a forward: Bloodshot Cyclops deals damage equal to the sacrificed forward's power to target forward or player."
                )
                .FlavorText("After their first encounter, the goblins named him Chuck.")
                .Power(4)
                .Toughness(4)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{T}, Sacrifice a forward: Bloodshot Cyclops deals damage equal to the sacrificed forward's power to target forward or player.";

                    p.Cost = new AggregateCost(new TapOwner(), new Sacrifice());

                    p.Effect = () =>
                        new DealDamageToTargets(
                            amount: P(e => e.Targets.Cost[0].Card().Power.GetValueOrDefault())
                        );

                    p.TargetSelector.AddCost(
                        trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield(),
                        trg => trg.Message = "Select a forward to sacrifice."
                    );

                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

                    p.TargetingRule(new CostSacrificeEffectDealDamageEqualToPower());
                    p.TimingRule(new OnMainStepsOfYourTurn());
                });
        }
    }
}
