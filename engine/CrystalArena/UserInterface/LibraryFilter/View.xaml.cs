using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CrystalArena.UserInterface.MainDeckFilter
{
  /// <summary>
  /// Interaction logic for View.xaml
  /// </summary>
  public partial class View : UserControl
  {
    public View()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
