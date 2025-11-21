using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace CrystalArena.UserInterface
{
    // Enables key bindings to be used in Caliburn micro based on idea found here:
    // http://www.felicepollano.com/2011/05/02/InputBindingKeyBindingWithCaliburnMicro.aspx
    public class InputBindingTrigger : Trigger<Control>
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter) { }

        private Window GetWindow(Control control)
        {
            if (control is Window)
                return control as Window;

            var parent = control.Parent as Control;
            Debug.Assert(parent != null);

            return GetWindow(parent);
        }
    }
}
