using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShare.Data;
using CarShare.Models;
using Microsoft.AspNetCore.Identity;
using CarShare.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace CarShare.Controllers
{
    [Authorize]
    public class RentsController : Controller
    {
        private readonly CarShareContext _context;
        private readonly UserManager<CarShareUser> _userManager;

        public RentsController(CarShareContext context, UserManager<CarShareUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rent.ToListAsync());
        }
        public async Task<IActionResult> ClientRentHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View();
            }
            else
            {
                var client = await _context.Users.Include(x => x.Client).FirstOrDefaultAsync(x => x.Id == user.Id);
                var rentList = await _context.Rent.Where(x => x.Client.Id == client.Client.Id).Include(x => x.Car).ToListAsync();
                return View(rentList);
            }   
        }

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent.Include(x => x.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }


        public IActionResult Summary(Rent rent)
        {
            rent.Car = _context.Car.Find(rent.Car.Id);
            var difference = rent.RentEndDate.Subtract(rent.RentStartDate);
            rent.RentPrice = rent.Car.BasicRentPrice * difference.Days;
            return View(rent);
        }
        public async Task<IActionResult> CreateForm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent =  new Rent();
            rent.RentStartDate = DateTime.Now;
            rent.RentEndDate = DateTime.Now;
            rent.Car =  _context.Car.Find(id);
            if (rent.Car == null)
            {
                return NotFound();
            }

            return View(rent);
        }



        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Create(Rent rent)
        {
            ModelState.Remove("Client.Id");
            ModelState.Remove("Client.LastName");
            ModelState.Remove("Client.FirstName");
            ModelState.Remove("Client");
            var user = await _userManager.GetUserAsync(User);
            var client = await _context.Users.Include(x => x.Client).Include(x => x.Client.ClientAdress).FirstOrDefaultAsync(x => x.Id == user.Id);
            rent.Car = await _context.Car.FindAsync(rent.Car.Id);
            rent.Client = client.Client;
            var car = rent.Car;
            //rent.Car.Id = 0;
            if (ModelState.IsValid)
            {
                rent.Car.IsAvailable = false;
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Summary" ,rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RentPrice,RentStartDate,RentEndDate")] Rent rent)
        {
            if (id != rent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rent.FindAsync(id);
            if (rent != null)
            {
                _context.Rent.Remove(rent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rent.Any(e => e.Id == id);
        }
    }
}
