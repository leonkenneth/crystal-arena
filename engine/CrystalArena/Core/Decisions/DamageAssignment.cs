namespace CrystalArena.Decisions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using CrystalArena.Infrastructure;

    [Copyable, Serializable]
    public class DamageAssignment : ISerializable
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

        [Serializable]
        public class ForwardDamageAssignment
        {
            public int CardId;
            public int Damage;
        }
    }
}
