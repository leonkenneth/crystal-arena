using System.Reactive;
using Avalonia.Interactivity;
using ReactiveUI;

namespace CrystalArena.UserInterface.StartScreen
{
  using Avalonia.Controls;
using Avalonia.Markup.Xaml;

  /// <summary>
  ///     Interaction logic for View.xaml
  /// </summary>
  public partial class View : UserControl
  {
    public View()
    {
      this.DataContext = new ViewModel();
      AvaloniaXamlLoader.Load(this);
    }
  }
}