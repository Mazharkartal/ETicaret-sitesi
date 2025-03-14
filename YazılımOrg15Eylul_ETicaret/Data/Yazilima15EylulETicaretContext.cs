using Microsoft.EntityFrameworkCore;
using YazılımOrg15Eylul_ETicaret.Models;
using YazılımOrg15Eylul_ETicaret.ViewModels;

namespace YazılımOrg15Eylul_ETicaret.Data;

public class Yazilima15EylulETicaretContext : DbContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory
            .GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var configuration = builder.Build();

        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:ETicaretConnection"]);

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<Message> Messages { get; set; }
    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public DbSet<Status> Statuses { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<TopluEmail> TopluEmails { get; set; }

    //---------------- Aşağıdakiler tablo değil
    public DbSet<MyOrdersViewModel> vw_MyOrders { get; set; }

    public DbSet<QuickSearchViewModel> sp_Aramas { get; set; }















}
