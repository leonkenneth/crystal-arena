namespace CrystalArena.Infrastructure
{
    using System.Threading;

    public class ThreadBlocker
    {
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);

        // Method to block the thread until Completed() is called
        public void BlockUntilCompleted(System.Action? action = null)
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
}
