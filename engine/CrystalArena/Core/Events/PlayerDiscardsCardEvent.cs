using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class PlayerDiscardsCardEvent : ITriggerMessage
    {
        public readonly Player Player;
        public readonly Card Card;

        public PlayerDiscardsCardEvent(Player player, Card card)
        {
            Player = player;
            Card = card;
        }
    }
}
