using System.Collections.Generic;

namespace CrystalArena.FFTCGCards.Test;

public class Test_003_BasicMonster : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("0-003X")
            .Named("Test Basic Monster")
            .ManaCost("{R}")
            .Monster(multiplayable: true);
    }
}
