using IleriWeb.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using IleriWeb.Core;
using IleriWeb.Core.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace IleriWeb.Repository
{
    public class AppDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Basket> Baskets { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }



		public DbSet<ApplicationUser> GetUserByIdResult { get; set; }


		public DbSet<maskedaspnetusers> maskedaspnetusers { get; set; }
        public DbSet<mostorderedproduct> mostorderedproducts { get; set; }
        public DbSet<mostsoldcategories> mostsoldcategories { get; set; }
        public DbSet<dailyordersummary> dailyordersummary { get; set; }
        public DbSet<mostactivecustomers> mostactivecustomers { get; set; }
        public DbSet<productstockstatus> productstockstatus { get; set; }


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

			modelBuilder.Entity<ProductFeature>()
	        .Property(pf => pf.Id)
	        .ValueGeneratedOnAdd(); // Otomatik artan Id

			modelBuilder.Entity<ApplicationUser>()
           .HasMany(u => u.Orders) // Bir ApplicationUser'ın birden çok Order'ı olabilir
           .WithOne(o => o.IdentityUser) // Her Order bir ApplicationUser'a aittir
           .HasForeignKey(o => o.IdentityUserId) // Foreign key UserId'dir
           .OnDelete(DeleteBehavior.Cascade); // User silindiğinde Order'lar da silinsin

			modelBuilder.Entity<Category>()
		.HasMany(c => c.Products) // Bir Category'nin birden çok Product'ı olabilir
		.WithOne(p => p.Category) // Her Product bir Category'ye aittir
		.HasForeignKey(p => p.CategoryId) // Foreign key CategoryId'dir
		.OnDelete(DeleteBehavior.Cascade); // Category silindiğinde Product'lar da silinsin

			modelBuilder.Entity<Product>()
		    .HasOne(p => p.ProductFeature) // Her Product'ın bir ProductFeature'ı vardır
		    .WithOne(pf => pf.Product) // Her ProductFeature bir Product'a aittir
		    .HasForeignKey<ProductFeature>(pf => pf.ProductId) // Foreign key ProductFeature tablosunda
		    .OnDelete(DeleteBehavior.Cascade); // Product silindiğinde ProductFeature da silinsin

		    modelBuilder.Entity<Order>()
	       .HasOne(o => o.OrderDetails) // Her Order'ın bir OrderDetail'ı vardır
	       .WithOne(od => od.Order) // Her OrderDetail bir Order'a aittir
	       .HasForeignKey<OrderDetail>(od => od.OrderId) // Foreign key OrderDetail tablosunda
	       .OnDelete(DeleteBehavior.Cascade); // Order silindiğinde OrderDetail da silinsin 

			modelBuilder.Entity<Basket>()
		  .HasMany(b => b.Items)
		  .WithOne(bi => bi.Basket)
		  .HasForeignKey(bi => bi.BasketId)
		  .OnDelete(DeleteBehavior.Cascade);



			modelBuilder.Entity<ApplicationUser>().ToTable(tb => tb.HasTrigger("user_created_trigger"));

            modelBuilder.Entity<maskedaspnetusers>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("maskedaspnetusers");
            });

            modelBuilder.Entity<mostorderedproduct>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("mostorderedproducts");
            });

			modelBuilder.Entity<userordersview>(entity =>
			{
				entity.HasNoKey();
				entity.ToView("userordersview");
			});

			modelBuilder.Entity<mostsoldcategories>(entity =>
            {
                entity.HasNoKey(); 
                entity.ToView("mostsoldcategories");
            });

            modelBuilder.Entity<dailyordersummary>(entity =>
            {
                entity.HasNoKey(); 
                entity.ToView("dailyordersummary");
            });

            modelBuilder.Entity<mostactivecustomers>(entity =>
            {
                entity.HasNoKey(); 
                entity.ToView("mostactivecustomers");
            });

            modelBuilder.Entity<productstockstatus>(entity =>
            {
                entity.HasNoKey(); 
                entity.ToView("productstockstatus");
            });


		
			modelBuilder.Entity<ApplicationUser>().HasNoKey(); // Stored procedure sonucu için primary key olmayabilir.
		

                
            base.OnModelCreating(modelBuilder);

		}
	}
}
