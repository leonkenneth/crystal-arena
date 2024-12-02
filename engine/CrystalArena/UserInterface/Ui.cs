namespace CrystalArena.UserInterface
{
  using Infrastructure;
  using Shell;

  public class Ui
  {
    public IShell Shell;
    public Match Match;
    public Dialogs Dialogs;
    public Configuration Configuration;
    public Publisher Publisher = new Publisher();

    public Ui(IShell shell, Dialogs dialogs)
    {
      Shell = shell;
      Shell.Ui = this;
      Dialogs = dialogs;
      Configuration = Configuration.Default;
    }

    public int GameId { get; set; }
  }
}