namespace CrystalArena.UserInterface
{
  using Infrastructure;
  using Shell;

  public class Ui
  {
    private string? _playerToken;
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

    public string GameId { get; set; }

    public string PlayerToken
    {
      get
      {
        if (_playerToken == null)
        {
          _playerToken = System.Guid.NewGuid().ToString();
        }

        return _playerToken;
      }
    }
  }
}