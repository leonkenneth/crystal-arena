namespace CrystalArena
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Newtonsoft.Json.Linq;

    [Copyable]
    public class Targets : IEnumerable<ITarget>, IHashable
    {
        private readonly List<ITarget> _costTargets = new List<ITarget>();
        private readonly List<ITarget> _effectTargets = new List<ITarget>();
        public List<int> Distribution;

        public Targets() { }

        public Targets(ITarget effect)
        {
            _effectTargets.Add(effect);
        }

        public Targets(ITarget cost, ITarget effect)
        {
            _costTargets.Add(cost);
            _effectTargets.Add(effect);
        }

        public int Count
        {
            get { return _effectTargets.Count + _costTargets.Count; }
        }
        public List<ITarget> Effect
        {
            get { return _effectTargets; }
        }
        public List<ITarget> Cost
        {
            get { return _costTargets; }
        }

        public IEnumerator<ITarget> GetEnumerator()
        {
            foreach (var costTarget in _costTargets)
            {
                yield return costTarget;
            }

            foreach (var effectTarget in _effectTargets)
            {
                yield return effectTarget;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int CalculateHash(HashCalculator calc)
        {
            return HashCalculator.Combine(
                HashCalculator.Combine(_effectTargets.Select(calc.Calculate)),
                HashCalculator.Combine(_costTargets.Select(calc.Calculate))
            );
        }

        public void WriteJson(JObject json, SerializationContext ctx)
        {
            json["costTargets"] = new JArray(_costTargets.Select(x => x.Id));
            json["effectTargets"] = new JArray(_effectTargets.Select(x => x.Id));
            json["distribution"] = Distribution != null ? new JArray(Distribution) : null;
        }

        public static Targets ReadJson(JObject json, SerializationContext ctx)
        {
            var targets = new Targets();
            var costTargetIds = json["costTargets"]!.ToObject<List<int>>()!;
            var effectTargetIds = json["effectTargets"]!.ToObject<List<int>>()!;

            var distributionToken = json["distribution"];
            targets.Distribution =
                distributionToken != null && distributionToken.Type != JTokenType.Null
                    ? distributionToken.ToObject<List<int>>()
                    : null;

            foreach (var id in costTargetIds)
                targets._costTargets.Add((ITarget)ctx.Recorder.GetObject(id));
            foreach (var id in effectTargetIds)
                targets._effectTargets.Add((ITarget)ctx.Recorder.GetObject(id));

            return targets;
        }

        public Targets AddCost(ITarget target)
        {
            _costTargets.Add(target);
            return this;
        }

        public Targets AddEffect(ITarget target)
        {
            _effectTargets.Add(target);
            return this;
        }
    }
}
