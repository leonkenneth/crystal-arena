namespace CrystalArena.Decisions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using CrystalArena.Infrastructure;

    [Copyable, Serializable]
    public class DamageAssignment : DecisionResult, ISerializable
    {
        private readonly Dictionary<Attacker, int> _attackerDamageAssigments =
            new Dictionary<Attacker, int>();
        public Dictionary<Attacker, int> AttackerDamageAssignments => _attackerDamageAssigments;

        public DamageAssignment() { }

        protected DamageAssignment(SerializationInfo info, StreamingContext context)
        {
            var ctx = (SerializationContext)context.Context;

            var forwardDamageAssignments =
                (List<ForwardDamageAssignment>)
                    info.GetValue(
                        "forwardDamageAssignments",
                        typeof(List<ForwardDamageAssignment>)
                    );

            foreach (var forwardDamageAssignment in forwardDamageAssignments)
            {
                var attacker = ctx.Game.Combat.FindAttacker(
                    (Card)ctx.Recorder.GetObject(forwardDamageAssignment.CardId)
                );
                _attackerDamageAssigments.Add(attacker, forwardDamageAssignment.Damage);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var forwardDamageAssignments = _attackerDamageAssigments
                .Select(x => new ForwardDamageAssignment()
                {
                    CardId = x.Key.Card.Id,
                    Damage = x.Value,
                })
                .ToList();

            info.AddValue("forwardDamageAssignments", forwardDamageAssignments);
        }

        public void Assign(Attacker attacker, int damage)
        {
            _attackerDamageAssigments[attacker] = damage;
        }

        public override string TypeName => nameof(DamageAssignment);

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            var forwardDamageAssignments = _attackerDamageAssigments
                .Select(x => new ForwardDamageAssignment()
                {
                    CardId = x.Key.Card.Id,
                    Damage = x.Value,
                })
                .ToList();

            info.AddValue("forwardDamageAssignments", forwardDamageAssignments);
        }

        internal static DamageAssignment FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var ctx = info.Context;
            var result = new DamageAssignment();

            var forwardDamageAssignments = (List<ForwardDamageAssignment>)
                info.GetValue("forwardDamageAssignments", typeof(List<ForwardDamageAssignment>));

            foreach (var forwardDamageAssignment in forwardDamageAssignments)
            {
                var attacker = ctx.Game.Combat.FindAttacker(
                    (Card)ctx.Recorder.GetObject(forwardDamageAssignment.CardId)
                );
                result._attackerDamageAssigments.Add(attacker, forwardDamageAssignment.Damage);
            }

            return result;
        }

        [Serializable]
        public class ForwardDamageAssignment
        {
            public int CardId;
            public int Damage;
        }
    }
}
