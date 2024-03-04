namespace CarShare.Services
{
    public interface IBackgroundTaskRunner
    {
        private static async void ExecuteTask(object state)
        {
        }

        private static async Task MyAsyncMethod()
        {
        }

        public void Stop()
        {
        }
    }
}
