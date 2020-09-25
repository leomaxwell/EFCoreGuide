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
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublisherController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var publisher = await _context.Publishers.ToListAsync();
            return View(publisher);
        }

        public async Task<IActionResult> Upsert(int? id)
        {

            if (id == null)
            {
                var publisher = new Publisher();
                return View(publisher);
            }

            else
            {
                var publisher = await _context.Publishers.FirstOrDefaultAsync(e => e.Publisher_Id == id);
                // var Publisher =  _context.Categories.Find(id);
                if (publisher == null)
                {
                    return NotFound();
                }

                return View(publisher);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return View(publisher);
            }

            if (publisher.Publisher_Id == 0)
            {
                await _context.Publishers.AddAsync(publisher);

            }
            else
            {
                _context.Publishers.Update(publisher);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(e => e.Publisher_Id == id);

            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
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
