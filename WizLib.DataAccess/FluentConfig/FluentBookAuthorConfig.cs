using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizLib.Model.Models;

namespace WizLib.DataAccess.FluentConfig
{
    public class FluentBookAuthorConfig : IEntityTypeConfiguration<Fluent_BookAuthor>
    {
        public void Configure(EntityTypeBuilder<Fluent_BookAuthor> builder)
        {
            builder.HasKey(e => new { e.Author_Id, e.Book_Id });

            builder.HasOne(e => e.Fluent_Book)
                .WithMany(e => e.Fluent_BookAuthors)
                .HasForeignKey(e => e.Book_Id);

            builder.HasOne(e => e.Fluent_Author)
                .WithMany(e => e.Fluent_BookAuthors)
                .HasForeignKey(e => e.Author_Id);
        }
    }
}
