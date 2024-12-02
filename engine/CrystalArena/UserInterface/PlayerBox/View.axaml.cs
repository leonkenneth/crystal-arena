namespace CrystalArena.UserInterface.PlayerBox
{
  using Avalonia.Controls;
using Avalonia.Markup.Xaml;

  /// <summary>
  ///     Interaction logic for View.xaml
  /// </summary>
  public partial class View : UserControl
  {
    public View(Player player)
    {
      AvaloniaXamlLoader.Load(this);
      DataContext = new ViewModel(player);
    }

    public Player Player
    {
      get;
      set;
    }
  }
}