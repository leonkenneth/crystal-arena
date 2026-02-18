namespace CrystalArena.UserInterface.MessageBox
{
    using System;
    using System.Windows;
    using Infrastructure;

    public class ViewModel : IClosable
    {
        public ViewModel(
            string message,
            string title,
            ButtonEnum buttons,
            DialogType dialogType = DialogType.Large
        )
        {
            Message = message;
            Title = title;
            DialogType = dialogType;
            Buttons = buttons;
        }

        public ButtonEnum Buttons { get; private set; }
        public DialogType DialogType { get; private set; }
        public string? Icon { get; private set; }

        public bool IsOk
        {
            get { return Buttons == ButtonEnum.Ok; }
        }

        public bool IsYesNo
        {
            get { return Buttons == ButtonEnum.YesNo || Buttons == ButtonEnum.YesNoCancel; }
        }
        public bool IsCancel
        {
            get { return Buttons == ButtonEnum.YesNoCancel; }
        }

        public string Message { get; private set; }
        public ButtonResult Result { get; private set; }
        public string Title { get; private set; }
        public event EventHandler Closed = delegate { };

        public void Close()
        {
            Closed(this, EventArgs.Empty);
        }

        public void No()
        {
            Close(ButtonResult.No);
        }

        public void Ok()
        {
            Close(ButtonResult.Ok);
        }

        public void Yes()
        {
            Close(ButtonResult.Yes);
        }

        public void Cancel()
        {
            Close(ButtonResult.Cancel);
        }

        private void Close(ButtonResult result)
        {
            Result = result;
            Close();
        }
    }
}
