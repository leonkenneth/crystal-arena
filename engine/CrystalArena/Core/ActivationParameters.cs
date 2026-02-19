namespace CrystalArena
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Newtonsoft.Json.Linq;

    [Copyable]
    public class ActivationParameters
    {
        public bool PayManaCost = true;
        public int Repeat = 1;
        public bool SkipStack;
        public Targets Targets = new Targets();
        public int? X;

        public List<Card> ConvokeTargets = new List<Card>();
        public List<Card> DelveTargets = new List<Card>();

        public ActivationParameters() { }

        public void WriteJson(JObject json, SerializationContext ctx)
        {
            json["PayManaCost"] = PayManaCost;
            json["Repeat"] = Repeat;
            json["SkipStack"] = SkipStack;
            json["X"] = X;

            var targetsJson = new JObject();
            Targets.WriteJson(targetsJson, ctx);
            json["Targets"] = targetsJson;

            json["convokeTargets"] = new JArray(ConvokeTargets.Select(x => x.Id));
            json["delveTargets"] = new JArray(DelveTargets.Select(x => x.Id));
        }

        public static ActivationParameters ReadJson(JObject json, SerializationContext ctx)
        {
            var parameters = new ActivationParameters();
            parameters.PayManaCost = json["PayManaCost"]!.Value<bool>();
            parameters.Repeat = json["Repeat"]!.Value<int>();
            parameters.SkipStack = json["SkipStack"]!.Value<bool>();

            var xToken = json["X"];
            parameters.X =
                xToken != null && xToken.Type != JTokenType.Null ? xToken.Value<int>() : null;

            var targetsJson = (JObject)json["Targets"]!;
            parameters.Targets = Targets.ReadJson(targetsJson, ctx);

            var convokeIds = json["convokeTargets"]!.ToObject<List<int>>()!;
            var delveIds = json["delveTargets"]!.ToObject<List<int>>()!;
            parameters.ConvokeTargets.AddRange(
                convokeIds.Select(id => (Card)ctx.Recorder.GetObject(id))
            );
            parameters.DelveTargets.AddRange(
                delveIds.Select(id => (Card)ctx.Recorder.GetObject(id))
            );

            return parameters;
        }
    }
}
