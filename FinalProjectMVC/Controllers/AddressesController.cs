using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        readonly IRepository<Address> addressRepo;

        public AddressesController(IRepository<Address> addressRepo) => this.addressRepo = addressRepo;

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            return View(await addressRepo.FilterAsync(a => a.UserId == User.GetUserId()));
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var address = await addressRepo.GetDetailsAsync(id);
            if (address == null) return NotFound();

            if (address.UserId != User.GetUserId())
            {
                return Unauthorized();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create() => View();

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StreetName,BuildingNumber,City,Region,UserId")] Address address)
        {
            if (address.UserId != User.GetUserId())
                return Unauthorized();

            if (ModelState.IsValid)
            {
                await addressRepo.InsertAsync(address);
                return RedirectToAction(nameof(Index));
            }

            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var address = await addressRepo.GetDetailsAsync(id);
            if (address == null) return NotFound();

            if (address.UserId != User.GetUserId())
            {
                return Unauthorized();
            }

            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StreetName,BuildingNumber,City,Region,UserId")] Address address)
        {
            if (id != address.Id) return NotFound();

            if (address.UserId != User.GetUserId())
                return Unauthorized();

            if (ModelState.IsValid)
            {
                try
                {
                    await addressRepo.UpdateAsync(id, address);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id)) return NotFound();
                    else return BadRequest();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var address = await addressRepo.GetDetailsAsync(id);

            if (address == null) return NotFound();

            if (address.UserId != User.GetUserId())
                return Unauthorized();

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await addressRepo.GetDetailsAsync(id);

            if (address == null) return NotFound();

            if (address.UserId != User.GetUserId())
                return Unauthorized();

            try
            {
                await addressRepo.DeleteAsync(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(address.Id)) return NotFound();
                else return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        bool AddressExists(int id) => addressRepo.GetDetails(id) is not null;
    }
}