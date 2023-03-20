using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Models;

namespace FinalProjectMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminsController : Controller
    {
        private readonly StoreDbContext _context;

        public AdminsController(StoreDbContext context)
        {
            _context = context;
        }

        // GET: AdminPanel/Admins
        public async Task<IActionResult> Index()
        {
              return _context.Admins != null ? 
                          View(await _context.Admins.ToListAsync()) :
                          Problem("Entity set 'StoreDbContext.Admins'  is null.");
        }

        // GET: AdminPanel/Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: AdminPanel/Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Address")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: AdminPanel/Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: AdminPanel/Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,Address")] Admin admin)
        {
            if (id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.Id))
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
            return View(admin);
        }

        // GET: AdminPanel/Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: AdminPanel/Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Admins == null)
            {
                return Problem("Entity set 'StoreDbContext.Admins'  is null.");
            }
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
          return (_context.Admins?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
