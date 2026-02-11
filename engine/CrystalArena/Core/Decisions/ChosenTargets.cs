namespace CrystalArena.Decisions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    [Serializable]
    public class ChosenTargets : DecisionResult
    {
        public ChosenTargets(Targets targets)
        {
            Targets = targets;
        }

        public Targets Targets { get; private set; }

        public bool HasTargets
        {
            get { return Targets != null && Targets.Count > 0; }
        }

        public static ChosenTargets None()
        {
            return new ChosenTargets(new Targets());
        }

        public override string TypeName => nameof(ChosenTargets);

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            // Targets class already implements ISerializable, we can serialize it as nested
            info.AddNested("targets", nested =>
            {
                var costTargetsIds = Targets.Cost.Select(x => x.Id).ToList();
                var effectTargetsIds = Targets.Effect.Select(x => x.Id).ToList();
                nested.AddValue("costTargets", costTargetsIds);
                nested.AddValue("effectTargets", effectTargetsIds);
                nested.AddValue("distribution", Targets.Distribution);
            });
        }

        internal static ChosenTargets FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var ctx = info.Context;
            var targetsInfo = info.GetNested("targets");

            var costTargetsIds = (List<int>)targetsInfo.GetValue("costTargets", typeof(List<int>));
            var effectTargetsIds = (List<int>)targetsInfo.GetValue("effectTargets", typeof(List<int>));
            var distribution = (List<int>)targetsInfo.GetValue("distribution", typeof(List<int>));

            var targets = new Targets();
            targets.Distribution = distribution;

            foreach (var id in costTargetsIds)
            {
                targets.Cost.Add((ITarget)ctx.Recorder.GetObject(id));
            }

            foreach (var id in effectTargetsIds)
            {
                targets.Effect.Add((ITarget)ctx.Recorder.GetObject(id));
            }

            return new ChosenTargets(targets);
        }
    }
}
