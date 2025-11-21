namespace CrystalArena
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.AI;
    using CrystalArena.Infrastructure;
    using Decisions;
    using Events;

    public class Attacker : GameObject, IHashable
    {
        private readonly Card _card;
        private readonly Trackable<bool> _isBlocked = new Trackable<bool>();

        public Attacker(Card card, Game game)
        {
            Game = game;
            _card = card;

            _isBlocked.Initialize(ChangeTracker);
        }

        private Attacker() { }

        public Card Card
        {
            get { return _card; }
        }
        public Player Controller
        {
            get { return _card.Controller; }
        }

        public int Score
        {
            get { return ScoreCalculator.CalculatePermanentScore(Card); }
        }
        public bool HasTrample
        {
            get { return _card.Has().Trample; }
        }
        public int LifepointsLeft
        {
            get { return _card.Life; }
        }
        public bool AssignsDamageAsThoughItWasntBlocked
        {
            get { return _card.Has().AssignsDamageAsThoughItWasntBlocked; }
        }
        public bool IsBlocked
        {
            get { return _isBlocked.Value; }
        }

        public int CalculateHash(HashCalculator calc)
        {
            return HashCalculator.Combine(calc.Calculate(_isBlocked), calc.Calculate(_card));
        }

        public bool CanBeBlockedBy(Card forward)
        {
            return _card.CanBeBlockedBy(forward);
        }

        public void RemoveFromCombat()
        {
            Publish(new RemovedFromCombatEvent(Card));
        }

        public static implicit operator Card?(Attacker? attacker)
        {
            return attacker != null ? attacker._card : null;
        }

        public bool CanKillBlocker()
        {
            return QuickCombat.CanBlockerBeDealtLeathalCombatDamage(Card, Combat.Blocker?.Card);
        }
    }
}
