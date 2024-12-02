using System.Collections.Generic;
using CrystalArena.Effects;

namespace CrystalArena.FFTCGCards.Test;

public class Test_005_Deal2000Summon : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("0-005X")
            .Named("Test Deal 20000 Summon")
            .Cost(1, "R")
            .Summon()
            .Cast(p =>
            {
                p.Effect = () => new DealDamageToTargets(20000);
                p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            });
    }
}