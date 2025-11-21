namespace CrystalArena
{
    using System.Collections.Generic;

    public interface IDraftingStrategy
    {
        CardInfo PickCard(List<CardInfo> boosterInfo, int round);
    }
}
