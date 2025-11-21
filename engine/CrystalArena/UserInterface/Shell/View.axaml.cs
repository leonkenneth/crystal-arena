using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CrystalArena.CardsMainDeck;

namespace CrystalArena.UserInterface.Shell;

public partial class View : Window
{
    public View()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
