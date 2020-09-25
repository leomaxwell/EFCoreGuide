using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib.DataAccess.Data;
using WizLib.Model.Models;
using WizLib.Model.ViewModels;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.Include(x => x.Publisher)
                .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author).ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var obj = new BookVM();
            obj.PublisherList = _context.Publishers.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Publisher_Id.ToString()
            });

            if (id == null)
            {
                return View(obj);
            }

            obj.Book = await _context.Books.FirstOrDefaultAsync(b => b.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BookVM obj)
        {
            if (obj.Book.Book_Id == 0)
            {
                _context.Books.Add(obj.Book);
            }
            else
            {
                _context.Books.Update(obj.Book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Details(int? id)
        {
            var obj = new BookVM();

            if (id == null)
            {
                return View(obj);
            }

            obj.Book = await _context.Books.FirstOrDefaultAsync(b => b.Book_Id == id);
            obj.Book.BookDetail = await _context
                                    .BookDetails
                                    .FirstOrDefaultAsync(b => b.BookDetail_Id == obj.Book.BookDetail_Id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(BookVM obj)
        {
            if (obj.Book.BookDetail.BookDetail_Id == 0)
            {
                _context.BookDetails.Add(obj.Book.BookDetail);
                await _context.SaveChangesAsync();

                var book = await _context.Books
                    .Include(b => b.BookDetail)
                    .FirstOrDefaultAsync(b => b.Book_Id == obj.Book.Book_Id);

                await _context.SaveChangesAsync();
            }
            else
            {
                _context.BookDetails.Update(obj.Book.BookDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(e => e.Book_Id == id);

            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageAuthors(int id)
        {
            var bookAuthor = new BookAuthorVM
            {
                BookAuthorList = _context.BookAuthors
                .Include(e => e.Author)
                 .Include(e => e.Book)
                 .Where(e => e.Book_Id == id).ToList(),

                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },

                Book = _context.Books.FirstOrDefault(e => e.Book_Id == id)
            };

            var assignedAuthors = bookAuthor.BookAuthorList.Select(e => e.Author_Id).ToList();

            //LINQ NOT IN Clause
            //Get all authors whose it id not in assigned Authors;
            var unassignedAuthors = _context.Authors
                .Where(e => !assignedAuthors.Contains(e.Author_Id))
                .ToList();

            bookAuthor.AuthorList = unassignedAuthors.Select(e => new SelectListItem
            {
                Text = e.FullName,
                Value = e.Author_Id.ToString()
            });

            return View(bookAuthor);
        }


        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if(bookAuthorVM.BookAuthor.Book_Id != 0 && 
                bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _context.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            var bookId = bookAuthorVM.Book.Book_Id;
            var bookAuthor = _context.BookAuthors.FirstOrDefault(e =>
                e.Author_Id == authorId &&
                e.Book_Id == bookId);

                _context.BookAuthors.Remove(bookAuthor);
                _context.SaveChanges();
          
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }

        public IActionResult PlayGround()
        {
            //var bookTemp = _db.Books.FirstOrDefault();
            //bookTemp.Price = 100;

            //var bookCollection = _db.Books;
            //double totalPrice = 0;

            //foreach (var book in bookCollection)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookList = _db.Books.ToList();
            //foreach (var book in bookList)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookCollection2 = _db.Books;
            //var bookCount1 = bookCollection2.Count();

            //var bookCount2 = _db.Books.Count();

            IEnumerable<Book> BookList1 = _context.Books;
            var FilteredBook1 = BookList1.Where(b => b.Price > 500).ToList();

            IQueryable<Book> BookList2 = _context.Books;
            var fileredBook2 = BookList2.Where(b => b.Price > 500).ToList();


            //var category = _db.Categories.FirstOrDefault();
            //_db.Entry(category).State = EntityState.Modified;

            //_db.SaveChanges();


            //Updating Related Data
            //var bookTemp1 = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == 4);
            //bookTemp1.BookDetail.NumberOfChapters = 2222;
            //bookTemp1.Price = 12345;
            //_db.Books.Update(bookTemp1);
            //_db.SaveChanges();


            //var bookTemp2 = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == 4);
            //bookTemp2.BookDetail.Weight = 3333;
            //bookTemp2.Price = 123456;
            //_db.Books.Attach(bookTemp2);
            //_db.SaveChanges();


            //VIEWS
            var viewList = _context.BookDetailsFromViews.ToList();
            var viewList1 = _context.BookDetailsFromViews.FirstOrDefault();
            var viewList2 = _context.BookDetailsFromViews.Where(u => u.Price > 500);

            //RAW SQL

            var bookRaw = _context.Books.FromSqlRaw("Select * from dbo.books").ToList();

            //SQL Injection attack prone
            int id = 2;
            var bookTemp1 = _context.Books.FromSqlInterpolated($"Select * from dbo.books where Book_Id={id}").ToList();

            var booksSproc = _context.Books.FromSqlInterpolated($" EXEC dbo.getAllBookDetails {id}").ToList();

            //.NET 5 only
            var BookFilter1 = _context.Books.Include(e => e.BookAuthors.Where(p => p.Author_Id == 5)).ToList();
            var BookFilter2 = _context.Books.Include(e => e.BookAuthors.OrderByDescending(p => p.Author_Id).Take(2)).ToList();


            return RedirectToAction(nameof(Index));

        }
    }
}
