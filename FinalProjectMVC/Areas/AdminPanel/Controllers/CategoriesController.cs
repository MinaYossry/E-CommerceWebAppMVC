using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoriesController : Controller
    {
        public IRepository<Category> CategoryRepository { get; }

        public CategoriesController(IRepository<Category> categoryRepository)
        {
            CategoryRepository = categoryRepository;
        }

        // GET: AdminPanel/Categories
        public async Task<IActionResult> Index() => View(await CategoryRepository.GetAllAsync());

        // GET: AdminPanel/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await CategoryRepository.GetDetailsAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // GET: AdminPanel/Categories/Create
        public IActionResult Create() => View();

        // POST: AdminPanel/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await CategoryRepository.InsertAsync(category);
                }
                catch
                {
                    throw new Exception("Sorry, Couldn't add category");
                }

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: AdminPanel/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await CategoryRepository.GetDetailsAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: AdminPanel/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        await CategoryRepository.UpdateAsync(id, category);
                    }
                    catch
                    {
                        throw new Exception("Sorry, Couldn't update category");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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

            return View(category);
        }

        // GET: AdminPanel/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await CategoryRepository.GetDetailsAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: AdminPanel/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await CategoryRepository.GetDetailsAsync(id);

            if (category != null)
            {
                try
                {
                    await CategoryRepository.DeleteAsync(id);
                }
                catch
                {
                    throw new Exception("Sorry, Couldn't Delete category");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        bool CategoryExists(int id)
        {
            return (CategoryRepository.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}