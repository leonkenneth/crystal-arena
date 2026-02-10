namespace CrystalArena.Decisions
{
    using System;

    [Serializable]
    public class Ordering : IDecisionResult
    {
        public Ordering(params int[] indices)
        {
            Indices = indices;
        }

        public int[] Indices { get; private set; }
    }
}
