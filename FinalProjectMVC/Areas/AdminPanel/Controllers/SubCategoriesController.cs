using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin")]
    public class SubCategoriesController : Controller
    {
        public IRepository<SubCategory> SubCategoryRepository { get; }
        public IRepository<Category> CategoryRepository { get; }

        public SubCategoriesController(IRepository<SubCategory> subCategoryRepository, IRepository<Category> categoryRepository)
        {
            SubCategoryRepository = subCategoryRepository;
            CategoryRepository = categoryRepository;
        }

        // GET: AdminPanel/SubCategories
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = (await SubCategoryRepository.GetAllAsync());
            return View(ApplicationDbContext);
        }

        // GET: AdminPanel/SubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var subCategory = (await SubCategoryRepository.GetDetailsAsync(id));
            if (subCategory == null) return NotFound();

            return View(subCategory);
        }

        // GET: AdminPanel/SubCategories/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await CategoryRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: AdminPanel/SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await SubCategoryRepository.InsertAsync(subCategory);
                }
                catch
                {
                    throw new Exception("Couldn't insert new subCategory");
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(await CategoryRepository.GetAllAsync(), "Id", "Name");
            return View(subCategory);
        }

        // GET: AdminPanel/SubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var subCategory = (await SubCategoryRepository.GetDetailsAsync(id));
            if (subCategory == null) return NotFound();
            ViewData["CategoryId"] = new SelectList(await CategoryRepository.GetAllAsync(), "Id", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: AdminPanel/SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId")] SubCategory subCategory)
        {
            if (id != subCategory.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await SubCategoryRepository.UpdateAsync(id, subCategory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategory.Id))
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

            ViewData["CategoryId"] = new SelectList(await CategoryRepository.GetAllAsync(), "Id", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: AdminPanel/SubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var subCategory = (await SubCategoryRepository.GetDetailsAsync(id));
            if (subCategory == null) return NotFound();

            return View(subCategory);
        }

        // POST: AdminPanel/SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = (await SubCategoryRepository.GetDetailsAsync(id));

            if (subCategory != null)
            {
                try
                {
                    await SubCategoryRepository.DeleteAsync(subCategory.Id);
                }
                catch
                {
                    throw new Exception("Sorry, Couldn't Delete Sub Category");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        bool SubCategoryExists(int id)
        {
            return (SubCategoryRepository.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}