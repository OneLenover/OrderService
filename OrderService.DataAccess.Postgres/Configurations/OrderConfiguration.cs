using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.DataAccess.Postgres.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.DataAccess.Postgres.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder) 
        {
            builder.ToTable("orders");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.Property(o => o.ProductId).IsRequired();
            builder.Property(o => o.Amount).IsRequired();
            builder.Property(o => o.EmailClient).IsRequired().HasMaxLength(200);
            builder.Property(o => o.Price).IsRequired().HasColumnType("numeric(18,2)");
            builder.Property(o => o.PhoneNumber).IsRequired().HasMaxLength(50);
            builder.Property(o => o.CreatedAt).IsRequired();
        }
    }
}
