using System;
using CrystalArena.Effects;
using MsBox.Avalonia.Enums;

namespace CrystalArena.Decisions
{
    using System.Windows;
    using AI;
    using UserInterface;

    public class ChooseToUseExBurst : Decision
    {
        private readonly IEffectSource _exBurstEffectSource;

        private ChooseToUseExBurst() { }

        public ChooseToUseExBurst(Player controller, IEffectSource exBurstEffectSource)
            : base(
                controller,
                () => new UiHandler(),
                () => new MachineHandler(),
                () => new ScenarioHandler(),
                () => new PlaybackHandler()
            )
        {
            _exBurstEffectSource = exBurstEffectSource;
        }

        private abstract class Handler : DecisionHandler<ChooseToUseExBurst, BooleanResult>
        {
            public override void ProcessResults()
            {
                if (Result.IsTrue)
                {
                    var ability = (D._exBurstEffectSource as TriggeredAbility);
                    var castRule = D._exBurstEffectSource as CastRule;
                    if (ability != null)
                    {
                        ResolveAbilityEffect(ability);
                        return;
                    }

                    if (castRule != null)
                    {
                        ResolveCastRuleEffect(castRule);
                        return;
                    }
                    throw new ArgumentException("Unsupported ExBurst effect type");
                }
            }

            protected override void SetResultNoQuery()
            {
                Result = false;
            }

            private void ResolveAbilityEffect(TriggeredAbility ability)
            {
                ability.Execute(null);
            }

            private void ResolveCastRuleEffect(CastRule castRule)
            {
                Enqueue(
                    new CastCard(
                        D.Controller,
                        p =>
                        {
                            p.Card = castRule.OwningCard;
                            p.PayManaCost = false;
                            p.SkipStack = true;
                        }
                    )
                );
            }
        }

        private class MachineHandler : Handler, ISearchNode, IMachineExecutionPlan
        {
            private readonly MachinePlanExecutor _executor;

            public MachineHandler()
            {
                _executor = new MachinePlanExecutor(this);
                Result = false;
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
                get { return 2; }
            }

            public void SetResult(int index)
            {
                Result = index != 0;
            }

            public void GenerateChoices() { }

            public override void Execute()
            {
                _executor.Execute();
            }

            protected override void Initialize()
            {
                _executor.Initialize(ChangeTracker);
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
                Result = (BooleanResult)Game.Recorder.LoadDecisionResult();
            }
        }

        private class ScenarioHandler : Handler
        {
            protected override void ExecuteQuery()
            {
                Result = GetNextScenarioResult() ?? false;
            }
        }

        private class UiHandler : Handler
        {
            protected override void ExecuteQuery()
            {
                var result = Ui.Shell.ShowMessageBox(
                    message: "Use ExBurst?",
                    buttons: ButtonEnum.YesNo,
                    type: DialogType.Small
                );

                Result = result == ButtonResult.Yes;
            }
        }
    }
}
