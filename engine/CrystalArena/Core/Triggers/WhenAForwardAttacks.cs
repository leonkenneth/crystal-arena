namespace CrystalArena.Triggers
{
    using System;
    using Events;
    using Infrastructure;

    public class WhenAForwardAttacks : Trigger, IReceive<AttackerJoinedCombatEvent>
    {
        private readonly Func<Parameters, bool> _predicate;

        private WhenAForwardAttacks() { }

        public WhenAForwardAttacks(Func<Parameters, bool> predicate = null)
        {
            _predicate =
                predicate
                ?? delegate
                {
                    return true;
                };
        }

        public void Receive(AttackerJoinedCombatEvent e)
        {
            if (_predicate(new Parameters(e, this)))
            {
                Set(e);
            }
        }

        public class Parameters
        {
            private readonly AttackerJoinedCombatEvent _e;
            private readonly WhenAForwardAttacks _trigger;

            public Parameters(AttackerJoinedCombatEvent e, WhenAForwardAttacks trigger)
            {
                _e = e;
                _trigger = trigger;
            }

            public bool You
            {
                get { return _e.Attacker.Controller != _trigger.OwningCard.Controller; }
            }
            public bool Opponent
            {
                get { return !You; }
            }

            public bool AttackerHas(Func<Card, bool> predicate)
            {
                return predicate(_e.Attacker.Card);
            }
        }
    }
}
