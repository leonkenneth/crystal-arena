namespace CrystalArena.Decisions
{
    using System;

    [Serializable]
    public class Ordering : DecisionResult
    {
        public Ordering(params int[] indices)
        {
            Indices = indices;
        }

        public int[] Indices { get; private set; }

        public override string TypeName => nameof(Ordering);

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            info.AddValue("indices", Indices);
        }

        internal static Ordering FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var indices = (int[])info.GetValue("indices", typeof(int[]));
            return new Ordering(indices);
        }
    }
}
