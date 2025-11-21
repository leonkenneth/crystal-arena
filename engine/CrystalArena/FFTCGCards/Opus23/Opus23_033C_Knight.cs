using System.Collections.Generic;
using System.Linq;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_033C_Knight : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card("23-033C");
        yield return Card("15-038C");
    }

    private CardTemplate Card(string code)
    {
        return base
            .Card.Code(code)
            .Named("Knight")
            .Cost(2, "I")
            .Backup(multiplayable: true)
            .Job("Standard Unit")
            .Text(
                "When Knight enters the field, gain {Z}.\n{T}, put Knight into the Break Zone: Choose 1 Forward of cost 3 or less. Dull it. You can only use this ability during your turn."
            )
            .TriggeredAbility(p =>
            {
                p.Text = "When Samurai enters the field, gain {Z}.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                p.Effect = () => new AddManaToPool("{Z}".Parse());
            })
            .ActivatedAbility(p =>
            {
                p.Text =
                    "{T}, put Knight into the Break Zone: Choose 1 Forward of cost 3 or less. Dull it. You can only use this ability during your turn.";
                p.Cost = new AggregateCost(new Tap(), new SacrificeThis());
                p.Effect = () => new TapTargets();
                p.TargetSelector.AddEffect(trg =>
                    trg.Is.Card(c => c.Is().Forward && c.ConvertedCost <= 3).On.Battlefield()
                );
                p.ActivateOnlyDuringYourTurn = true;
            });
    }
}
