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
    public class FluentBookConfig : IEntityTypeConfiguration<Fluent_Book>
    {
        public void Configure(EntityTypeBuilder<Fluent_Book> builder)
        {
            builder.HasKey(e => e.Book_Id);
            
            builder.Property(e => e.Title)
                .IsRequired();

            builder.Property(e => e.ISBN)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Price)
                .IsRequired();

            builder.HasOne(e => e.Fluent_BookDetail)
                .WithOne(e => e.Fluent_Book)
                .HasForeignKey<Fluent_Book>(e => e.BookDetail_Id);
            
            builder.HasOne(e => e.Fluent_Publisher)
                .WithMany(e => e.Fluent_Books)
                .HasForeignKey(e => e.Publisher_Id);

        }
    }
}
