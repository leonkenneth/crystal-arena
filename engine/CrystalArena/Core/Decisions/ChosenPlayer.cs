namespace CrystalArena.Decisions
{
    using System;
    using System.Runtime.Serialization;
    using CrystalArena.Infrastructure;

    [Copyable, Serializable]
    public class ChosenPlayer : DecisionResult, ISerializable
    {
        private ChosenPlayer() { }

        public ChosenPlayer(Player player)
        {
            Player = player;
        }

        protected ChosenPlayer(SerializationInfo info, StreamingContext context)
        {
            var ctx = (SerializationContext)context.Context;
            Player = (Player)ctx.Recorder.GetObject(info.GetInt32("player"));
        }

        public Player Player { get; private set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("player", Player.Id);
        }

        public override string TypeName => nameof(ChosenPlayer);

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            info.AddValue("player", Player.Id);
        }

        internal static ChosenPlayer FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var ctx = info.Context;
            var playerId = info.GetInt32("player");
            var player = (Player)ctx.Recorder.GetObject(playerId);
            return new ChosenPlayer(player);
        }
    }
}
