using System;
using CrystalArena.Infrastructure;
using Lucene.Net.QueryParsers.Flexible.Core;
using CrystalArena.UserInterface.MessageBox;

namespace CrystalArena.UserInterface.Shell;

public class CallbackableDialog : RemoteCallbackable
{
    public object ViewModel { get; set; }

    public void WaitCallback()
    {
        var threadBlocker = new ThreadBlocker();
        var viewModel = ViewModel as ViewModelBase;
        ((IClosable)ViewModel).Closed += delegate
        {
            threadBlocker.Completed();
        };
        RegisterCallback(
            (message) =>
            {
                viewModel.ReceiveJSONMessage(message);
            }
        );
        threadBlocker.BlockUntilCompleted();
    }

    public override object ToJson()
    {
        return new { CallbackId };
    }
}
