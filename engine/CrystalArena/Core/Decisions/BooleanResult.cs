namespace CrystalArena.Decisions
{
    using System;

    [Serializable]
    public class BooleanResult : DecisionResult
    {
        public BooleanResult(bool value)
        {
            IsTrue = value;
        }

        public bool IsTrue { get; private set; }
        
        public override string TypeName => nameof(BooleanResult);

        public static implicit operator BooleanResult(bool value)
        {
            return new BooleanResult(value);
        }

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            info.AddValue("IsTrue", IsTrue);
        }

        internal static BooleanResult FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var value = info.GetBool("IsTrue");
            return new BooleanResult(value);
        }
    }
}
