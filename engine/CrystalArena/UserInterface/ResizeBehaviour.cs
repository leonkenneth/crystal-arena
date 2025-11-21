namespace CrystalArena.UserInterface
{
    using System;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using Avalonia.Xaml.Interactivity;
    using Infrastructure;

    public class ResizeBehaviour : Behavior<Window>
    {
        private const Int32 SizeMaxhide = 0x0004;
        private const Int32 SizeMaximized = 0x0002;
        private const Int32 SizeMaxshow = 0x0003;
        private const Int32 SizeMinimized = 0x0001;
        private const Int32 SizeRestored = 0x0000;
        private const Int32 WmExitSizeMove = 0x0232;
        private const Int32 WmSize = 0x0005;
        private const Int32 WmSizing = 0x0214;

        private Window Window
        {
            get { return AssociatedObject; }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            //Window.Loaded += (s, e) => WireUpWndProc();
            //Window.KeyUp += ToggleFullScreen;

            Window.SizeChanged += delegate
            {
                OnResized();
            };

            var fullscreen = Settings.Readonly.FullScreen;

#if DEBUG
            fullscreen = false;
#endif

            if (fullscreen)
            {
                Window.Loaded += delegate
                {
                    Window.WindowState = WindowState.Maximized;
                };
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        private void OnMaximized()
        {
            Window.Hide();
            Window.Show();
        }

        private void OnResized()
        {
            var verticalBorderWidth = WindowDimensions.VerticalBorderWidth;
            var horizontalBorderWidth = WindowDimensions.HorizontalBorderWidth;
        }

        private void OnRestored() { }
    }
}
