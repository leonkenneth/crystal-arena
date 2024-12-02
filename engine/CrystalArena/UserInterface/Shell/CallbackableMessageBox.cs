using System;
using CrystalArena.Infrastructure;
using Lucene.Net.QueryParsers.Flexible.Core;
using MsBox.Avalonia.Enums;

namespace CrystalArena.UserInterface.Shell;

public class CallbackableMessageBox : RemoteCallbackable
{
    public string Message { get; set; }
    public string Buttons { get; set; }
    public string Title { get; set; }

    
    public ButtonResult WaitCallback()
    {
        var threadBlocker = new ThreadBlocker();
        ButtonResult buttonResultEnum = ButtonResult.None;
        RegisterCallback((string buttonResult) =>
        {
            if (!ButtonResult.TryParse(buttonResult, true, out buttonResultEnum))
            {
                throw new Exception("Nope nope");
            }
            threadBlocker.Completed();
        });
        threadBlocker.BlockUntilCompleted();
        return buttonResultEnum;
    }

    public override object ToJson()
    {
        return new
        {
            Message,
            Buttons,
            Title,
            CallbackId
        };
    }
}