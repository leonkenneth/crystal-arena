namespace CrystalArena.Decisions
{
    using System;
    using CrystalArena.Infrastructure;

    [Copyable, Serializable]
    public class ChosenPlayable : DecisionResult
    {
        public IPlayable Playable { get; set; }
        public bool WasPriorityPassed
        {
            get { return Playable.WasPriorityPassed; }
        }

        public static ChosenPlayable Pass
        {
            get { return new ChosenPlayable { Playable = new Pass() }; }
        }

        public override string TypeName => nameof(ChosenPlayable);

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            // IPlayable serialization is complex - store type info and let each implementation handle itself
            // For now, just store if it's a Pass
            var isPass = Playable is Pass;
            info.AddValue("isPass", isPass);

            // TODO: Add more robust IPlayable serialization when needed
            if (!isPass)
            {
                throw new System.NotImplementedException("Serialization of non-Pass IPlayable not yet implemented");
            }
        }

        internal static ChosenPlayable FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var isPass = info.GetBool("isPass");

            if (isPass)
            {
                return Pass;
            }

            throw new System.NotImplementedException("Deserialization of non-Pass IPlayable not yet implemented");
        }
    }
}
