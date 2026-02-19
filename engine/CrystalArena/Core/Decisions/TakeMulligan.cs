using MsBox.Avalonia.Enums;

namespace CrystalArena.Decisions
{
    using System.Linq;
    using System.Windows;
    using CrystalArena.UserInterface;

    public class TakeMulligan : Decision
    {
        private TakeMulligan() { }

        public TakeMulligan(Player controller)
            : base(
                controller,
                () => new UiHandler(),
                () => new MachineHandler(),
                () => new MachineHandler(),
                () => new PlaybackHandler()
            ) { }

        private abstract class Handler : DecisionHandler<TakeMulligan, BooleanResult>
        {
            protected override bool ShouldExecuteQuery
            {
                get { return D.Controller.CanMulligan; }
            }

            public override void ProcessResults()
            {
                if (Result.IsTrue)
                {
                    D.Controller.TakeMulligan();
                }
            }

            protected override void SetResultNoQuery()
            {
                Result = false;
            }
        }

        private class MachineHandler : Handler
        {
            public MachineHandler()
            {
                Result = false;
            }

            protected override void ExecuteQuery()
            {
                var backupCount = D.Controller.Hand.Backups.Count();
                Result = backupCount < 2 && D.Controller.Hand.Count > 4;
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
                Result = Game.Recorder.LoadDecisionResult<BooleanResult>();
            }
        }

        private class UiHandler : Handler
        {
            protected override void ExecuteQuery()
            {
                var result = Ui.Shell.ShowMessageBox(
                    title: "Mulligan",
                    message: "Do you want to improve your hand by taking a mulligan?",
                    buttons: ButtonEnum.YesNo
                );

                Result = result == ButtonResult.Yes;
            }
        }
    }
}
