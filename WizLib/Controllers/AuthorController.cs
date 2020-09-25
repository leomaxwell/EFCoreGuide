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
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var author = await _context.Authors.ToListAsync();
            return View(author);
        }

        public async Task<IActionResult> Upsert(int? id)
        {

            if (id == null)
            {
                var author = new Author();
                return View(author);
            }

            else
            {
                var author = await _context.Authors.FirstOrDefaultAsync(e => e.Author_Id == id);
               
                if (author == null)
                {
                    return NotFound();
                }

                return View(author);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            if (author.Author_Id == 0)
            {
                await _context.Authors.AddAsync(author);

            }
            else
            {
                _context.Authors.Update(author);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors
                .FirstOrDefaultAsync(e => e.Author_Id == id);

            if (author != null)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
