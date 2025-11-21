using Avalonia.Markup.Xaml;

namespace CrystalArena.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Markup;
    using Avalonia.Controls;
    using Avalonia.Media.Imaging;
    using Infrastructure;
    using Media;

    public class BackgroundExtension : MarkupExtension
    {
        private static readonly List<Bitmap> Backgrounds;

        static BackgroundExtension()
        {
            Backgrounds = MediaMainDeck
                .GetImages(filename => filename.StartsWith("background"))
                .ToList();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return MediaMainDeck.MissingImage;
            var index = RandomEx.Next(0, Backgrounds.Count);
            return Backgrounds[index];
        }
    }
}
