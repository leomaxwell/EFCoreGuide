using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizLib.DataAccess.Data;
using WizLib.Model.Models;

namespace WizLib.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Upsert(int? id)
        {

            if (id == null)
            {
                var category = new Category();
                return View(category);
            }

            else
            {
                var category = await _context.Categories.FirstOrDefaultAsync(e => e.Category_Id == id);
                // var category =  _context.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (category.Category_Id == 0)
            {
                await _context.Categories.AddAsync(category);

            }
            else
            {
                _context.Categories.Update(category);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(e => e.Category_Id == id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CreateMultiple2()
        {
            var categories = new List<Category>();
            for (int i = 1; i <= 2; i++)
            {
                categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }

            _context.Categories.AddRange(categories);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> CreateMultiple5()
        {
            var categories = new List<Category>();
            for (int i = 1; i <= 5; i++)
            {
                categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }

            _context.Categories.AddRange(categories);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> RemoveMultiple2()
        {
            var categories = _context.Categories
                            .OrderByDescending(e => e.Category_Id)
                            .Take(2)
                            .ToList();

            _context.Categories.RemoveRange(categories);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> RemoveMultiple5()
        {
            var categories = _context.Categories
                            .OrderByDescending(e => e.Category_Id)
                            .Take(5)
                            .ToList();

            _context.Categories.RemoveRange(categories);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
