using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class PlayerDrawsCardEvent : ITriggerMessage
    {
        public readonly Player Player;

        public PlayerDrawsCardEvent(Player player)
        {
            Player = player;
        }
    }
}
