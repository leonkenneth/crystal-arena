using System.Collections.Generic;
using CrystalArena.Decisions;

namespace CrystalArena
{
    using System;
    using System.IO;

    [Serializable]
    public class SavedGame
    {
        public DecisionLog Decisions;
        public PlayerParameters Player1;
        public PlayerParameters Player2;
        public int RandomSeed;
        public int StateCount;
    }
}
