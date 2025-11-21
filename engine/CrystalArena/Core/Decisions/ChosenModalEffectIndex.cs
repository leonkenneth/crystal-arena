namespace CrystalArena.Decisions
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ChosenModalEffectIndex
    {
        private readonly List<int> _indices = new List<int>();

        public ChosenModalEffectIndex(params int[] indices)
        {
            _indices.AddRange(indices);
        }

        public IList<int> Indices
        {
            get { return _indices; }
        }
    }
}
