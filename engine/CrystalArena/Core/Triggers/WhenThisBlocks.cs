namespace CrystalArena.Triggers
{
    using System;
    using Events;
    using Infrastructure;

    public class WhenThisBlocks : Trigger, IReceive<BlockerJoinedCombatEvent>
    {
        public WhenThisBlocks() { }

        public void Receive(BlockerJoinedCombatEvent e)
        {
            if (e.Blocker.Card == Ability.OwningCard)
            {
                Set(e);
            }
        }
    }
}
