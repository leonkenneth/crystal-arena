namespace CrystalArena.Decisions
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ChosenModalEffectIndex : DecisionResult
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

        public override string TypeName => nameof(ChosenModalEffectIndex);

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            info.AddValue("indices", _indices);
        }

        internal static ChosenModalEffectIndex FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var indices = (List<int>)info.GetValue("indices", typeof(List<int>));
            return new ChosenModalEffectIndex(indices.ToArray());
        }
    }
}
