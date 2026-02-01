using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class PlayerSearchesMainDeck : ITriggerMessage
    {
        public readonly Player Player;

        public PlayerSearchesMainDeck(Player player)
        {
            Player = player;
        }
    }
}
