namespace CrystalArena.Triggers
{
    using Events;
    using Infrastructure;

    public class WhenPlayerSearchesMainDeck : Trigger, IReceive<PlayerSearchesMainDeck>
    {
        private readonly TriggerPredicate<Player> _cond;

        private WhenPlayerSearchesMainDeck() { }

        public WhenPlayerSearchesMainDeck(TriggerPredicate<Player> cond = null)
        {
            _cond =
                cond
                ?? delegate
                {
                    return true;
                };
        }

        public void Receive(PlayerSearchesMainDeck message)
        {
            if (_cond(message.Player, Ctx))
            {
                Set(message);
            }
        }
    }
}
