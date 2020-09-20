using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizLib.Model.Models;

namespace WizLib.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }       
        
        public DbSet<Category> Categories { get; set; }
        //public DbSet<Genre> Genres { get; set; }
    }
}
