using System.Collections.Generic;
using System.Linq;
using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class BlockerJoinedCombatEvent : ITriggerMessage
    {
        public readonly Party Party;
        public readonly Blocker Blocker;

        public BlockerJoinedCombatEvent(Blocker blocker, IEnumerable<Attacker> attackers)
        {
            Blocker = blocker;
            Party = new Party(attackers);
        }
    }
}
