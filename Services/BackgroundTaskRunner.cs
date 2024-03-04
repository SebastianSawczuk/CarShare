using CarShare.Data;
using CarShare.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShare.Services
{
    public class BackgroundTaskRunner : IBackgroundTaskRunner
    {
        private Timer _timer;
        private readonly CarShareContext _context;

        public BackgroundTaskRunner(CarShareContext context)
        {
            _context = context;
            _timer = new Timer(ExecuteTask, null, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10));
        }

        private  async void ExecuteTask(object state)
        {
            var rents = _context.Rent.Include(x => x.Car).Where(x => x.RentEndDate < DateTime.Now).ToList();
            await MyAsyncMethod(rents).ConfigureAwait(false);
        }

        private  async Task MyAsyncMethod(List<Rent> rents)
        {
            Console.WriteLine("wszystko działa");


        }

        public void Stop()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
