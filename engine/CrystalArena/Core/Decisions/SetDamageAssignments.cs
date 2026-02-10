using CrystalArena.AI;

namespace CrystalArena.Decisions
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Infrastructure;
    using CrystalArena.UserInterface;

    public class SetDamageAssignments : Decision
    {
        private readonly IEnumerable<Attacker> _attackers;
        private readonly Blocker? _blocker;

        private SetDamageAssignments() { }

        public SetDamageAssignments(
            Player defendingPlayer,
            IEnumerable<Attacker> attackers,
            Blocker? blocker
        )
            : base(
                defendingPlayer,
                () => new UiHandler(),
                () => new MachineHandler(),
                () => new MachineHandler(),
                () => new PlaybackHandler()
            )
        {
            _attackers = attackers;
            _blocker = blocker;
        }

        private abstract class Handler : DecisionHandler<SetDamageAssignments, DamageAssignment>
        {
            protected override bool ShouldExecuteQuery
            {
                get { return D._attackers.Count() > 1 && D._blocker != null; }
            }

            public override void ProcessResults()
            {
                Combat.SetDamageAssignment(Result);
            }

            protected override void SetResultNoQuery()
            {
                Result = new DamageAssignment();

                if (D._attackers.None() || D._blocker == null)
                    return;

                var attacker = D._attackers.First();
                var blocker = D._blocker;
                var damage = QuickCombat.GetAmountOfDamageForward1WillDealToForward2(
                    forward1: blocker.Card,
                    forward2: attacker.Card
                );
                Result.Assign(attacker, damage);
            }
        }

        private class MachineHandler : Handler
        {
            public MachineHandler()
            {
                Result = new DamageAssignment();
            }

            protected override void ExecuteQuery()
            {
                if (D._blocker.HasDeathTouch)
                {
                    Result = DeathTouchScenario();
                    return;
                }

                Result = DefaultScenario();
            }

            private DamageAssignment DeathTouchScenario()
            {
                var damageAssignment = new DamageAssignment();

                var attackersOrderedByScore = D
                    ._attackers.OrderByDescending(attacker => attacker.Score)
                    .ToList();

                var damagesLeftToDeal = D._blocker!.Card.CalculateCombatDamageAmount(
                    toPlayer: false
                );
                foreach (var attacker in attackersOrderedByScore)
                {
                    if (damagesLeftToDeal < 1000)
                        break;
                    damageAssignment.Assign(attacker, 1000);
                    damagesLeftToDeal -= 1000;
                }

                return damageAssignment;
            }

            private DamageAssignment DefaultScenario()
            {
                var damageAssignment = new DamageAssignment();

                var attackers = GetAttackersThatCanBeDealtLeathalDamageProducingTheGreatestScore();
                IncludeOtherAttackersAfter(attackers);

                var damagesLeftToDeal = D._blocker!.Card.CalculateCombatDamageAmount(
                    toPlayer: false
                );
                foreach (var attacker in attackers)
                {
                    if (damagesLeftToDeal < 1000)
                        break;
                    damageAssignment.Assign(attacker, 1000);
                    damagesLeftToDeal -= 1000;
                }

                return damageAssignment;
            }

            private List<Attacker> GetAttackersThatCanBeDealtLeathalDamageProducingTheGreatestScore()
            {
                var blocker = D._blocker!;
                var maxBlockerCombatDamage = blocker.Card.CalculateCombatDamageAmount(
                    toPlayer: false
                );
                var candidates = D
                    ._attackers.Where(attacker => attacker.LifepointsLeft > 0)
                    .Where(attacker => attacker.LifepointsLeft <= maxBlockerCombatDamage)
                    .Select(attacker => new KnapsackItem<Attacker>(
                        item: attacker,
                        weight: attacker.LifepointsLeft,
                        value: attacker.Score
                    ))
                    .ToList();

                var result = Knapsack.Solve(candidates, maxBlockerCombatDamage);
                return result.OrderByDescending(x => x.Value).Select(x => x.Item).ToList();
            }

            private void IncludeOtherAttackersAfter(List<Attacker> attackers)
            {
                attackers.AddRange(
                    D._attackers.Where(attacker => !attackers.Contains(attacker))
                        .OrderByDescending(attacker => attacker.Score)
                );
            }
        }

        private class PlaybackHandler : Handler
        {
            protected override bool ShouldExecuteQuery
            {
                get { return true; }
            }

            public override void SaveDecisionResults() { }

            protected override void ExecuteQuery()
            {
                Result = Game.Recorder.LoadDecisionResult<DamageAssignment>();
            }
        }

        private class UiHandler : Handler
        {
            protected override void ExecuteQuery()
            {
                var result = new DamageAssignment();

                var dialog = Ui.Dialogs.DamageAssignment.Create(D._blocker!, D._attackers, result);
                Ui.Shell.ShowModalDialog(dialog);

                Result = result;
            }
        }
    }
}
