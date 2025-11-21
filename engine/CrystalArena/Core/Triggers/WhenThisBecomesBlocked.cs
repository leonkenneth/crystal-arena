namespace CrystalArena.Triggers
{
    using System;
    using Events;
    using Infrastructure;

    public class WhenThisBecomesBlocked
        : Trigger,
            IReceive<BlockerJoinedCombatEvent>,
            IReceive<StepStartedEvent>
    {
        private readonly Trackable<int> _count = new Trackable<int>();
        private readonly bool _triggerForEveryBlocker;

        private WhenThisBecomesBlocked() { }

        public WhenThisBecomesBlocked(bool triggerForEveryBlocker)
        {
            _triggerForEveryBlocker = triggerForEveryBlocker;
        }

        public void Receive(BlockerJoinedCombatEvent message)
        {
            if (!message.Party.Contains(Ability.OwningCard))
                return;

            _count.Value += 1;

            if (_triggerForEveryBlocker || _count.Value == 1)
            {
                Set(message);
            }
        }

        public void Receive(StepStartedEvent message)
        {
            if (message.Step == Step.EndOfCombat)
            {
                _count.Value = 0;
            }
        }

        protected override void Initialize()
        {
            _count.Initialize(ChangeTracker);
        }

        public class Parameters { }
    }
}
