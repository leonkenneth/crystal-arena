using System.Collections.Generic;
using CrystalArena.Effects;

namespace CrystalArena.FFTCGCards.Test;

public class Test_004_RemoveFromGameSummon : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("0-004X")
            .Named("Test RemoveFromPlay Invocation")
            .ManaCost("{R}")
            .Summon()
            .Cast(p =>
            {
                p.Effect = () => new RemoveFromPlayTargets();
                p.TargetSelector.AddEffect(trg => trg.Is.Card().In.BreakZone());
            });
    }
}
