using System.Collections.Generic;
using System.Linq;

namespace CrystalArena.Events
{
    public class BlockerJoinedCombatEvent
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
