using CrystalArena.UserInterface.MessageBox;

namespace CrystalArena.UserInterface.LoadScreen
{
    using System.Threading.Tasks;
    using Infrastructure;

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
            var startScreen = ViewModels.StartScreen.Create();
            Shell.ChangeScreen(startScreen);
        }

        public interface IFactory
        {
            ViewModel Create();
        }
    }
}
