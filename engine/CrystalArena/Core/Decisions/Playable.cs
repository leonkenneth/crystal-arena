namespace CrystalArena.Decisions
{
    using CrystalArena.Infrastructure;

    [Copyable]
    public abstract class Playable : IPlayable
    {
        public ActivationParameters ActivationParameters = new ActivationParameters();
        public Card Card;
        public int Index;

        protected Playable() { }

        public Player Controller
        {
            get { return Card.Controller; }
        }
        public virtual bool WasPriorityPassed
        {
            get { return false; }
        }

        public virtual void Play() { }
    }
}
