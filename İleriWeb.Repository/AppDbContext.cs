using İleriWeb.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext:DbContext
	{

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<mostorderedproduct> mostorderedproducts { get; set; }
        public DbSet<mostsoldcategories> mostsoldcategories { get; set; }

        public DbSet<dailyordersummary> dailyordersummary { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach( var item in ChangeTracker.Entries())
            {
                if(item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                        {
                                entityReference.CreatedTime= DateTime.Now.ToUniversalTime();
                                break;                        
                        }
                        case EntityState.Modified:
                        {
                                Entry(entityReference).Property(x => x.CreatedTime).IsModified = false;

                            entityReference.UpdatedTime= DateTime.Now.ToUniversalTime(); 
                            break;
                        }


                    }

                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedTime = DateTime.Now.ToUniversalTime();
                                break;
                            }
                        case EntityState.Modified:
                            {
                                entityReference.UpdatedTime = DateTime.Now.ToUniversalTime();
                                break;
                            }


                    }

                }
            }

            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<mostorderedproduct>(entity =>
            {
                entity.HasNoKey(); // View'lar genellikle birincil anahtar içermez
                entity.ToView("mostorderedproducts"); // Veritabanındaki view adını kullanın
                entity.Property(v => v.productname).HasColumnName("productname");
                entity.Property(v => v.totalquantity).HasColumnName("totalquantity");
            });

            modelBuilder.Entity<mostsoldcategories>(entity =>
            {
                entity.HasNoKey(); // View'lar genellikle birincil anahtar içermez
                entity.ToView("mostsoldcategories");
            });

            modelBuilder.Entity<dailyordersummary>(entity =>
            {
                entity.HasNoKey(); // View'lar genellikle birincil anahtar içermez
                entity.ToView("dailyordersummary");
            });

            base.OnModelCreating(modelBuilder);

		}
	}
}
