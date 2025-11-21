namespace CrystalArena
{
    using System.Linq;
    using CrystalArena.AI;
    using CrystalArena.Infrastructure;
    using Events;

    public class Blocker : GameObject, IHashable
    {
        private readonly Trackable<int> _damageAssignmentOrder = new Trackable<int>();

        private Blocker() { }

        public Blocker(Card card, Game game)
        {
            Card = card;
            Game = game;

            _damageAssignmentOrder.Initialize(ChangeTracker);
        }

        public Card Card { get; private set; }
        public Player Controller
        {
            get { return Card.Controller; }
        }

        public bool HasDeathTouch
        {
            get { return Card.Has().Deathtouch; }
        }

        public int DamageAssignmentOrder
        {
            get { return _damageAssignmentOrder.Value; }
            set { _damageAssignmentOrder.Value = value; }
        }

        public int LifepointsLeft
        {
            get { return Card.Life; }
        }
        public int Toughness
        {
            get { return Card.Toughness.Value; }
        }
        public bool HasFirstStrike => Card.HasFirstStrike;

        public int CalculateHash(HashCalculator calc)
        {
            return HashCalculator.Combine(calc.Calculate(Card), DamageAssignmentOrder);
        }

        public void RemoveFromCombat()
        {
            Publish(new RemovedFromCombatEvent(Card));
        }

        public bool WillBeDealtLeathalCombatDamage()
        {
            return QuickCombat.CanBlockerBeDealtLeathalCombatDamage(
                new Party(Combat.Attackers),
                Card
            );
        }

        public bool CanKillAnyAttacker()
        {
            return Combat.Attackers.Any(x =>
                QuickCombat.CanAttackerBeDealtLeathalDamage(x, this.Card)
            );
        }
    }
}
