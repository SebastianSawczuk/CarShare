using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShare.Data;
using CarShare.Models;
using Microsoft.AspNetCore.Authorization;
using CarShare.Services;
using Microsoft.IdentityModel.Tokens;

namespace CarShare.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarShareContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CarsController(CarShareContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Car.ToListAsync());
        }       
        


        public async Task<IActionResult> CarsToRent(string? phrase, string filter, int page = 1, int pageSize = 6)
        {
            var cars = _context.Car.ToListAsync().Result;
            if (!phrase.IsNullOrEmpty())
            {
                cars = cars.Where(x => x.Brand!.Contains(phrase)).ToList();

            }

            if (!filter.IsNullOrEmpty())
            {
                if (filter.Equals("brand"))
                {
                    cars = cars.OrderBy(x => x.Brand).ToList();
                }
                if (filter.Equals("model"))
                {
                    cars = cars.OrderBy(x => x.Model).ToList();
                }
                if (filter.Equals("mileage"))
                {
                    cars = cars.OrderBy(x => x.Mileage).ToList();
                }
                if (filter.Equals("avilible"))
                {
                    cars = cars.OrderByDescending(x => x.IsAvailable).ToList();
                }
            }
            

            // Paginacja
            var count = cars.Count();
            var items = cars.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Przekazanie danych do widoku
            ViewBag.MaxPage = (count / pageSize) + 1;
            ViewBag.Page = page;

            return View(items);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public async Task<IActionResult> CarToRentDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }



        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create( Car car)
        {
            //string carImageDirectory = "W:\\Study\\Documents\\Semestr VI\\Programowanie zaawansowane\\CarShare\\CarShare\\Views\\Shared\\CarImages\\";
            string carImageDirectory = Path.Combine(@"W:\Study\Documents\Semestr VI\Programowanie zaawansowane\CarShare\CarShare\Views\Shared\CarImages\", car.ImgUrl);
            if (ModelState.IsValid)
            {
                car.ImgUrl = carImageDirectory;
                car.IsAvailable = true;
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string carImageDirectory = Path.Combine(@"W:\Study\Documents\Semestr VI\Programowanie zaawansowane\CarShare\CarShare\Views\Shared\CarImages\", car.ImgUrl);
                try
                {
                    car.ImgUrl = carImageDirectory;
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            return View(car);
        }



        public async Task<IActionResult> ReturnCar(int id)
        {
            var car = _context.Car.FindAsync(id).Result;
            try
            {
                car.IsAvailable = true;
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(car.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("ClientRentHistory", "Rents");
        }

        // GET: Cars/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car != null)
            {
                _context.Car.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "CarImages", imageName);
            return PhysicalFile(imagePath, "image/jpeg"); // Lub odpowiedni typ MIME dla twojego obrazu
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.Id == id);
        }
    }
}
