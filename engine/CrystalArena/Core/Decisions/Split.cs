namespace CrystalArena.Decisions
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class Split : DecisionResult
    {
        private readonly List<List<Card>> _groups;

        public Split()
        {
            _groups = new List<List<Card>>();
        }

        public Split(IEnumerable<IEnumerable<Card>> groups)
        {
            _groups = groups.Select(x => x.ToList()).ToList();
        }

        public List<List<Card>> Groups
        {
            get { return _groups; }
        }

        public override string TypeName => nameof(Split);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            var groupsArray = new JArray(
                _groups.Select(group => new JArray(group.Select(card => card.Id)))
            );
            json["groups"] = groupsArray;
        }

        internal static new Split ReadJson(JObject json, SerializationContext ctx)
        {
            var groupsArray = (JArray)json["groups"]!;
            var groups = groupsArray
                .Select(group =>
                    group.Select(id => (Card)ctx.Recorder.GetObject(id.Value<int>())).ToList()
                )
                .ToList();
            return new Split(groups);
        }
    }
}
