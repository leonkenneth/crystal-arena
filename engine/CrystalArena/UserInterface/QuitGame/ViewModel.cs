namespace CrystalArena.UserInterface.QuitGame
{
    using System.Windows;
    using Infrastructure;

    public class ViewModel : ViewModelBase
    {
        public bool CanRematch
        {
            get { return !Match.IsTournament; }
        }

        public override object ToJson()
        {
            return new { Type = "QuitGame" };
        }

        public void QuitToMainMenu()
        {
            Match.Stop();
        }

        public void QuitToOperatingSystem()
        {
            Match.Stop();
        }

        public void Cancel()
        {
            this.Close();
        }

        public void Rematch()
        {
            Ui.Match.Rematch();
        }

        public void Save()
        {
            SaveGame();
            this.Close();
        }

        public interface IFactory
        {
            ViewModel Create();
        }
    }
}
