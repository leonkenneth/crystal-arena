using Avalonia.Threading;

namespace CrystalArena.Infrastructure
{
    using System.Diagnostics;
    using System.Threading;
    using Caliburn.Micro;

    public class ThreadBlocker
    {
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);

        // Method to block the thread until Completed() is called
        public void BlockUntilCompleted(System.Action action = null)
        {
            action = action ?? delegate { };

            // Execute the action if provided
            action();

            // Block the current thread until the reset event is set
            _resetEvent.WaitOne();
        }

        // Method to unblock the thread by signaling the reset event
        public void Completed()
        {
            _resetEvent.Set();
        }
    }

    public class OldThreadBlocker
    {
        private AutoResetEvent _autoResetEvent;
        private DispatcherFrame _frame;

        public void BlockUntilCompleted(System.Action action = null)
        {
            action = action ?? delegate { };

            if (BlockOnUiThread(action))
                return;

            BlockOnWorkerThread(action);
        }

        public void Completed()
        {
            if (_frame != null)
            {
                _frame.Continue = false;
            }

            if (_autoResetEvent != null)
                _autoResetEvent.Set();
        }

        private bool BlockOnUiThread(System.Action action)
        {
            var dispather = Dispatcher.UIThread;

            if (!dispather.CheckAccess())
                return false;

            Debug.Assert(
                _frame == null,
                "Do not reuse existing thread blocker, create a new one instead."
            );
            _frame = new DispatcherFrame(true);

            action();

            // this will block until completed is called
            dispather.PushFrame(_frame);
            _frame = null;

            return true;
        }

        private void BlockOnWorkerThread(System.Action action)
        {
            Debug.Assert(
                _autoResetEvent == null,
                "Do not reuse existing thread blocker, create a new one instead."
            );
            _autoResetEvent = new AutoResetEvent(false);

            action.OnUIThread();

            _autoResetEvent.WaitOne();
            _autoResetEvent = null;
        }
    }
}
