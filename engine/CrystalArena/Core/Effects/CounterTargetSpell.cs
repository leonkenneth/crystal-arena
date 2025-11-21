namespace CrystalArena.Effects
{
    using System;
    using Decisions;
    using Infrastructure;

    public class CounterTargetSpell : Effect, IProcessDecisionResults<BooleanResult>
    {
        private readonly Parameters _p = new Parameters();

        private CounterTargetSpell() { }

        [Copyable]
        public class Parameters
        {
            public DynParam<int> DoNotCounterCost;
            public int? ControllerLifeloss;
            public bool TapBackupsAndEmptyManaPool;
            public bool RemoveFromPlaySpell;
        }

        public CounterTargetSpell(Action<Parameters> setParmeters = null)
        {
            if (setParmeters != null)
            {
                setParmeters(_p);
            }

            RegisterDynamicParameters(_p.DoNotCounterCost);
        }

        public void ProcessResults(BooleanResult results)
        {
            if (results.IsTrue)
                return;

            CounterSpell();
        }

        protected override void ResolveEffect()
        {
            var targetSpellController = Target.Effect().Controller;

            if (_p.DoNotCounterCost == null)
            {
                CounterSpell();
                return;
            }

            Enqueue(
                new PayOr(
                    targetSpellController,
                    p =>
                    {
                        p.ManaAmount = _p.DoNotCounterCost.Value.Colorless();
                        p.Text = string.Format("Pay {0}?", _p.DoNotCounterCost);
                        p.ProcessDecisionResults = this;
                    }
                )
            );
        }

        private void CounterSpell()
        {
            var targetSpellController = Target.Effect().Controller;

            if (_p.ControllerLifeloss.HasValue)
            {
                targetSpellController.Life -= _p.ControllerLifeloss.Value;
            }

            if (_p.TapBackupsAndEmptyManaPool)
            {
                foreach (var backup in targetSpellController.Battlefield.Backups)
                {
                    backup.Tap();
                }

                targetSpellController.EmptyManaPool();
            }

            Stack.Counter(Target.Effect());

            if (_p.RemoveFromPlaySpell)
            {
                Target.Effect().Source.OwningCard.RemoveFromPlay(this);
            }
        }
    }
}
