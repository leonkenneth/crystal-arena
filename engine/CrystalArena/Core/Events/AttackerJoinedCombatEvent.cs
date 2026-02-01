using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class AttackerJoinedCombatEvent : ITriggerMessage
    {
        public readonly Attacker Attacker;

        public AttackerJoinedCombatEvent(Attacker attacker)
        {
            Attacker = attacker;
        }
    }
}
