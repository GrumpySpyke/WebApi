using Microsoft.EntityFrameworkCore;
using WebApplication1.Repository.Models;

namespace WebApplication1.Repository.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionArticle> TransactionArticles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionArticle>()
          .HasKey(ta => new { ta.TransactionId, ta.ArticleId });

            modelBuilder.Entity<TransactionArticle>()
                .HasOne(ta => ta.Transaction)
                .WithMany(t => t.TransactionArticles)
                .HasForeignKey(ta => ta.TransactionId);

            modelBuilder.Entity<TransactionArticle>()
                .HasOne(ta => ta.Article)
                .WithMany(a => a.TransactionArticles)
                .HasForeignKey(ta => ta.ArticleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
