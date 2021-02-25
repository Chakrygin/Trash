using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace EFApp.DataAccess
{
    public sealed class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) :
            base(options)
        {
        }

        public DbSet<Cart> Carts => Set<Cart>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Cart>(cart =>
            {
                cart.OwnsMany(x => x.Items, item =>
                {
                    item.HasKey("CartId", "ItemId");
                });
            });
        }
    }

    public sealed class Cart
    {
        public Guid Id { get; set; }
        public List<CartItem> Items { get; set; }
    }

    public sealed class CartItem
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
    }
}