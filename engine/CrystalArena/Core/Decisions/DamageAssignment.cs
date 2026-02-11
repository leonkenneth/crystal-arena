namespace CrystalArena.Decisions
{
    using System.Collections.Generic;
    using CrystalArena.Infrastructure;
    using Newtonsoft.Json.Linq;

    [Copyable]
    public class DamageAssignment : DecisionResult
    {
        private readonly Dictionary<Attacker, int> _attackerDamageAssigments =
            new Dictionary<Attacker, int>();
        public Dictionary<Attacker, int> AttackerDamageAssignments => _attackerDamageAssigments;

        public DamageAssignment() { }

        public void Assign(Attacker attacker, int damage)
        {
            _attackerDamageAssigments[attacker] = damage;
        }

        public override string TypeName => nameof(DamageAssignment);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            var array = new JArray();
            foreach (var kvp in _attackerDamageAssigments)
            {
                var item = new JObject();
                item["cardId"] = kvp.Key.Card.Id;
                item["damage"] = kvp.Value;
                array.Add(item);
            }
            json["forwardDamageAssignments"] = array;
        }

        internal static new DamageAssignment ReadJson(JObject json, SerializationContext ctx)
        {
            var result = new DamageAssignment();
            var assignments = (JArray)json["forwardDamageAssignments"]!;

            foreach (var item in assignments)
            {
                var cardId = item["cardId"]!.Value<int>();
                var damage = item["damage"]!.Value<int>();
                var attacker = ctx.Game.Combat.FindAttacker((Card)ctx.Recorder.GetObject(cardId));
                result._attackerDamageAssigments.Add(attacker, damage);
            }
            return result;
        }
    }
}
