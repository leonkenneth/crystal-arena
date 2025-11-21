using MsBox.Avalonia.Enums;

namespace CrystalArena.UserInterface.LoadScreen
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Media;

    public class ViewModel : ViewModelBase
    {
        public override object ToJson()
        {
            return new { Type = "LoadScreen" };
        }

        public string LoadingMessage
        {
            get { return ThinkingMessages.GetRandom(); }
        }

        public virtual long Completed { get; protected set; }

        public override void Initialize()
        {
            Task.Factory.StartNew(() => MediaMainDeck.LoadAll(ShowProgress))
                .ContinueWith(
                    tsk =>
                    {
                        if (tsk.IsFaulted)
                        {
                            var message = tsk.Exception.GetBaseException().Message;

                            Ui.Shell.ShowMessageBox(
                                message,
                                ButtonEnum.Ok,
                                DialogType.Large,
                                "Initialization error"
                            );

                            LogFile.Error(message);

                            //Application.Current.Shutdown();
                            return;
                        }

                        var startScreen = ViewModels.StartScreen.Create();
                        Shell.ChangeScreen(startScreen);
                    },
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
        }

        private void ShowProgress(long current, long total)
        {
            var completed = current * 100 / total;

            if (completed > Completed)
            {
                Completed = completed;
            }
        }

        public interface IFactory
        {
            ViewModel Create();
        }
    }
}
