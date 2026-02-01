using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    using System.Collections.Generic;

    public class AttackersDeclaredEvent : ITriggerMessage
    {
        public readonly IEnumerable<Attacker> Attackers;

        public AttackersDeclaredEvent(IEnumerable<Attacker> attackers)
        {
            Attackers = attackers;
        }
    }
}
