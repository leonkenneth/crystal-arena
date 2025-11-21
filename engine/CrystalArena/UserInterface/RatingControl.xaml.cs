namespace CrystalArena.UserInterface
{
    using System;
    using Avalonia.Controls;
    using Avalonia.Interactivity;
    using Avalonia.Markup.Xaml;

    /// <summary>
    ///     Interaction logic for RatingControl.xaml
    /// </summary>
    public partial class RatingControl : Grid
    {
        public RatingControl()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public int Rating
        {
            get { return 1; }
        }
    }
}
