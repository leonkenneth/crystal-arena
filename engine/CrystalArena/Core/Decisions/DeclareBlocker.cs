namespace CrystalArena.Decisions
{
    using System.Collections.Generic;
    using System.Linq;
    using AI;
    using Events;
    using UserInterface;
    using UserInterface.Messages;
    using UserInterface.SelectTarget;

    public class DeclareBlocker : Decision
    {
        private DeclareBlocker() { }

        public DeclareBlocker(Player controller)
            : base(
                controller,
                () => new UiHandler(),
                () => new MachineHandler(),
                () => new ScenarioHandler(),
                () => new PlaybackHandler()
            ) { }

        private abstract class Handler : DecisionHandler<DeclareBlocker, ChosenBlocker>
        {
            protected override bool ShouldExecuteQuery
            {
                get
                {
                    return Game.Combat.CanAttackersBeBlockedByAny(
                        D.Controller.Battlefield.Forwards
                    );
                }
            }

            public override void ProcessResults()
            {
                var blocker = Result.Blocker;

                if (blocker != null)
                {
                    var combatCost = blocker.CombatCost;

                    if (combatCost > 0)
                    {
                        D.Controller.Consume(combatCost.Colorless(), ManaUsage.Any);
                    }

                    Combat.SetBlocker(blocker);
                }

                Publish(new BlockerDeclaredEvent(Combat.Blocker));
            }

            protected override void SetResultNoQuery()
            {
                Result = new ChosenBlocker();
            }
        }

        private class MachineHandler : Handler, IMachineExecutionPlan, ISearchNode
        {
            private readonly MachinePlanExecutor _executor;
            private List<ChosenBlocker> _declarations;

            public MachineHandler()
            {
                Result = new ChosenBlocker();
                _executor = new MachinePlanExecutor(this);
            }

            public override bool HasCompleted
            {
                get { return _executor.HasCompleted; }
            }

            void IMachineExecutionPlan.ExecuteQuery()
            {
                ExecuteQuery();
            }

            Game ISearchNode.Game
            {
                get { return Game; }
            }

            public Player Controller
            {
                get { return D.Controller; }
            }

            public int ResultCount
            {
                get { return _declarations.Count; }
            }

            public void SetResult(int index)
            {
                Result = _declarations[index];
            }

            public void GenerateChoices()
            {
                _declarations = GetBlockersDeclarations();
            }

            public override void Execute()
            {
                _executor.Execute();
            }

            protected override void Initialize()
            {
                _executor.Initialize(ChangeTracker);
            }

            private void GetBlockersDeclarations(
                List<Card> attackers,
                List<ChosenBlocker> strategies
            )
            {
                // 1. Strategy, no blockers
                strategies.Add(ChosenBlocker.None);

                // 2. Strategy, try assign some blockers via shallow strategy
                var allBlockerCandidates = RemoveBlockersIfWeCannotAffordToPayCombatCost(
                    D.Controller.Battlefield.ForwardsThatCanBlock.ToList()
                );

                var strategy2 = BlockStrategy.ChooseBlocker(
                    new BlockStrategyParameters
                    {
                        Attackers = attackers,
                        BlockerCandidates = allBlockerCandidates,
                        DefendersLife = D.Controller.Life,
                    }
                );

                if (strategy2.Blocker != null)
                {
                    strategies.Add(strategy2);
                }
            }

            private List<Card> RemoveBlockersIfWeCannotAffordToPayCombatCost(
                List<Card> blockerCandidates
            )
            {
                var blockersWithCombatCost = blockerCandidates
                    .Where(x => x.CombatCost > 0)
                    .OrderBy(x => -x.Power)
                    .ToList();

                if (blockersWithCombatCost.Count == 0)
                    return blockerCandidates;

                var availableMana = Controller.GetAvailableManaCount();

                foreach (var card in blockersWithCombatCost)
                {
                    if (card.CombatCost <= availableMana)
                    {
                        availableMana -= card.CombatCost;
                    }
                    else
                    {
                        blockerCandidates.Remove(card);
                    }
                }

                return blockerCandidates;
            }

            private List<ChosenBlocker> GetBlockersDeclarations()
            {
                var strategies = new List<ChosenBlocker>();
                var attackers = Combat.Attackers.Select(x => x.Card).ToList();

                GetBlockersDeclarations(attackers, strategies);

                return strategies;
            }

            protected override void ExecuteQuery()
            {
                Ai.SetBestResult(this);
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
                Result = Game.Recorder.LoadDecisionResult<ChosenBlocker>();
            }
        }

        private class ScenarioHandler : Handler
        {
            protected override bool ShouldExecuteQuery
            {
                get { return true; }
            }

            protected override void ExecuteQuery()
            {
                Result = GetNextScenarioResult() ?? ChosenBlocker.None;
            }
        }

        private class UiHandler : Handler
        {
            private bool IsValidBlockerDeclaration(
                ChosenBlocker chosen,
                List<Card> lureAttackers,
                int availableMana
            )
            {
                return true; // TODO: Implement "must block"
            }

            protected override void ExecuteQuery()
            {
                var result = new ChosenBlocker();
                var availableMana = D.Controller.GetAvailableManaCount();

                var lureAttackers = Combat
                    .Attackers.Select(x => x.Card)
                    .Where(x => x.Has().Lure)
                    .ToList();

                while (true)
                {
                    var blockerSpec = new IsValidTargetBuilder()
                        .Is.Card(c =>
                            c.CanBlock()
                            && c.Controller == D.Controller
                            && c.CombatCost <= availableMana
                        )
                        .On.Battlefield();

                    var blockerTarget = new TargetValidatorParameters(
                        isValidTarget: blockerSpec.IsValidTarget,
                        isValidZone: blockerSpec.IsValidZone
                    )
                    {
                        MinCount = IsValidBlockerDeclaration(result, lureAttackers, availableMana)
                            ? 0
                            : 1,
                        MaxCount = 1,
                        Message = "Select a blocker.",
                        MustBeTargetable = false,
                    };

                    var blockerValidator = new TargetValidator(blockerTarget);
                    blockerValidator.Initialize(Game, D.Controller);

                    var selectBlocker = Ui.Dialogs.SelectTarget.Create(
                        new SelectTargetParameters
                        {
                            Validator = blockerValidator,
                            CanCancel = false,
                            Instructions = IsValidBlockerDeclaration(
                                result,
                                lureAttackers,
                                availableMana
                            )
                                ? null
                                : "(Additional blockers required.)",
                        }
                    );

                    Ui.Shell.ShowModalDialog(
                        selectBlocker,
                        DialogType.Small,
                        InteractionState.SelectTarget
                    );

                    if (selectBlocker.Selection.Count == 0)
                    {
                        break;
                    }

                    var blocker = (Card)selectBlocker.Selection[0];

                    if (result.Blocker == blocker)
                    {
                        availableMana += blocker.CombatCost;
                        result.Blocker = null;

                        Ui.Publisher.Publish(new BlockerUnselected { Blocker = blocker });

                        continue;
                    }

                    Ui.Publisher.Publish(new BlockerSelected { Blocker = blocker });

                    result.Blocker = blocker;
                    availableMana -= blocker.CombatCost;
                }

                Result = result;
            }
        }
    }
}
