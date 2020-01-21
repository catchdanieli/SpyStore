using Microsoft.EntityFrameworkCore;
//using SpyStore.Models.Entities;
//using SpyStore.Models.Entities.Base;
using SpyStore.Models.ViewModels;

public class StoreContext: DbContext
{
    public int CustomerId { get; set; }

    public StoreContext(DbContextOptions<StoreContext> options):base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingCartRecord> ShoppingCartRecords { get; set; }

    public DbQuery<CartRecordWithProductInfo> CartRecordWithProductInfos { get; set; }
    public DbQuery<OrderDetailWithProductInfo> OrderDetailWithProductInfos { get; set; }

    [DbFunction("GetOrderTotal", Schema = "Store")]
    public static int GetOrderTotal(int orderId)
    {
        //code in here doesn't matter
        throw new Exception();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity => {
            entity.HasIndex(e => e.EmailAddress).HasName("IX_Customers").IsUnique();
        });

        modelBuilder.Entity<Order>(entity => {
            entity.Property(e => e.OrderDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(e => e.ShipDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(e => e.OrderTotal).HasColumnType("Money").HasComputedColumnSql("Store.GetOrderTotal([Id])");
        });

        modelBuilder.Entity<Order>().HasQueryFilter(x => x.CustomerId == CustomerId);

        modelBuilder.Entity<OrderDetail>(entity => {
            entity.Property(e => e.UnitCost).HasColumnType("money");
            entity.Property(e => e.LineItemTotal).HasColumnType("money").HasComputedColumnSql("[Quantity]*[UnitCost]");
        });

        modelBuilder.Entity<Product>(entity => {
            entity.Property(e => e.UnitCost).HasColumnType("money");
            entity.Property(e => e.CurrentPrice).HasColumnType("money");
            entity.OwnsOne(o => o.Details, pd => {
                pd.Property(p => p.Description).HasColumnName(nameof(ProductDetails.Description));
                pd.Property(p => p.ModelName).HasColumnName(nameof(ProductDetails.ModelName));
                pd.Property(p => p.ModelNumber).HasColumnName(nameof(ProductDetails.ModelNumber));
                pd.Property(p => p.ProductImage).HasColumnName(nameof(ProductDetails.ProductImage));
                pd.Property(p => p.ProductImageLarge).HasColumnName(nameof(ProductDetails.ProductImageLarge));
                pd.Property(p => p.ProductImageThumb).HasColumnName(nameof(ProductDetails.ProductImageThumb));
            });
        });

        modelBuilder.Entity<ShoppingCartRecord>(entity => {
            entity.Property(e => e.DateCreated).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.HasIndex(e => new { ShoppingCartRecordId = e.Id, e.ProductId, e.CustomerId }).HasName("IX_ShoppingCart").IsUnique();
        });

        modelBuilder.Entity<ShoppingCartRecord>().HasQueryFilter(x => x.CustomerId == CustomerId);

        modelBuilder.Query<CartRecordWithProductInfo>().ToView("CartRecordWithProductInfo", "Store");
        modelBuilder.Query<OrderDetailWithProductInfo>().ToView("OrderDetailWithProductInfo", "Store");
    }
}

