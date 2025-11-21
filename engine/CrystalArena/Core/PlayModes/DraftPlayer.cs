namespace CrystalArena
{
    using System.Collections.Generic;

    public class DraftPlayer
    {
        public IDraftingStrategy Strategy;
        public List<CardInfo> MainDeck = new List<CardInfo>();
    }
}
