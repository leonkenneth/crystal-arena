using System.Collections.Generic;

namespace CrystalArena.FFTCGCards.Test;

public class Test_007_LimitBreakForward : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("0-007X")
            .Named("Test LimitBreak Forward")
            .Cost(2, "R")
            .Forward(multiplayable: true)
            .SimpleAbilities(Static.Brave)
            .Power(5000)
            .LimitBreak(2);
    }
}
