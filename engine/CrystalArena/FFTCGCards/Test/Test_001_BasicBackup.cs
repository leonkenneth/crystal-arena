using System.Collections.Generic;

namespace CrystalArena.FFTCGCards.Test;

public class Test_001_BasicBackup : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("0-001X")
            .Named("Test Basic Backup")
            .ManaCost("{R}")
            .Backup(multiplayable: true);
    }
}