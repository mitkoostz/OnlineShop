
using Core.Entities;
using Core.Entities.Admin;
using Core.Entities.ContactUs;
using Core.Entities.OrderAggregate;
using Core.Entities.ProductDiscounts;
using Core.Entities.ProductSizeAndQuantityNameSpace;
using Core.Entities.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Reflection;

namespace Ifrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGenderBase> ProductGenderBase { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductSizeAndQuantity> ProductSizeAndQuantity { get; set; }
        public DbSet<Size> Sizes { get; set; }

        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<ProductDiscountCode> ProductDiscountCodes { get; set; }
        public DbSet<DiscountCodeUsed> DiscountCodeUsed { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<AdminActionHistory> AdminActionHistory { get; set; }
        public DbSet<ContactUsMessage> ContactUsMessages { get; set; }


        protected override void  OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly
                .GetExecutingAssembly());

            //Setting the QuantitySize connection Table
            modelBuilder.Entity<ProductSizeAndQuantity>()
             .HasIndex(p => new {p.ProductId , p.SizeId}).IsUnique();

            //Configure the relation Size with ProductType Cycle OnDelete - Restrict
            modelBuilder.Entity<Size>()
                .HasOne<ProductType>(s => s.ProductType)
                .WithMany(type => type.Sizes)
                .HasForeignKey(u => u.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            { 
                foreach (var entitytype in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entitytype.ClrType.GetProperties()
                        .Where(p => p.PropertyType
                    == typeof(decimal));
                    var dateTimeProperties = entitytype.ClrType.GetProperties()
                        .Where(p => p.PropertyType == typeof(DateTimeOffset));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entitytype.Name).Property(property.Name)
                            .HasConversion<double>();                          
                    }

                    foreach (var property in dateTimeProperties)
                    {
                        modelBuilder.Entity(entitytype.Name).Property(property.Name)
                              .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }

            }
        }
    }
}
