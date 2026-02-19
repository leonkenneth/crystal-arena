namespace CrystalArena.Decisions
{
    using CrystalArena.Infrastructure;
    using Newtonsoft.Json.Linq;

    [Copyable]
    public class ChosenPlayer : DecisionResult
    {
        private ChosenPlayer() { }

        public ChosenPlayer(Player player)
        {
            Player = player;
        }

        public Player Player { get; private set; }

        public override string TypeName => nameof(ChosenPlayer);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            json["player"] = Player.Id;
        }

        internal static new ChosenPlayer ReadJson(JObject json, SerializationContext ctx)
        {
            var playerId = json["player"]!.Value<int>();
            var player = (Player)ctx.Recorder.GetObject(playerId);
            return new ChosenPlayer(player);
        }
    }
}
