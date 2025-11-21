using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_027C_JihlNabaat : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-027C")
            .Named("Jihl Nabaat")
            .Cost(2, "I")
            .Category("DFF · XIII")
            .Job("PSICOM")
            .Backup()
            .Text(
                "{I}{I}{T}, put 1 Job PSICOM into the Break Zone: Choose 1 dull Forward. Break it."
            )
            .ActivatedAbility(p =>
            {
                p.Text =
                    "{I}{I}{T}, put 1 Job PSICOM into the Break Zone: Choose 1 dull Forward. Break it.";
                p.Cost = new AggregateCost(
                    new PayMana("{I}{I}".Parse()),
                    new Tap(),
                    new SacrificeTarget()
                );
                p.TargetSelector.AddCost(trg =>
                    trg.Is.Card(x => x.HasJob("PSICOM"), ControlledBy.SpellOwner).On.Battlefield()
                );

                p.Effect = () => new DestroyTargetPermanents();
                p.TargetSelector.AddEffect(trg =>
                    trg.Is.Card(x => x.Is().Forward && x.IsTapped).On.Battlefield()
                );
            });
    }
}
