using System.Collections.Generic;

namespace CrystalArena.FFTCGCards.Test;

public class Test_002_BasicForward : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("0-002X")
            .Named("Test Basic Forward")
            .Cost(2, "R")
            .Forward(multiplayable: true)
            .SimpleAbilities(Static.Brave)
            .Power(5000);
    }
}
