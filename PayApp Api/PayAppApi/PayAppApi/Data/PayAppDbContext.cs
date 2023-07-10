using Microsoft.EntityFrameworkCore;
using PayAppApi.Models;

namespace PayAppApi.Data
{
    public class PayAppDbContext:DbContext
    {
        public PayAppDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<PaymentDetail> PaymentDetails { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<MatchDetail> MatchDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PaymentDetail>(p =>
            {
                p.HasKey(x => x.Id);
                p.Property(x => x.Amount)
                    .HasColumnType("decimal(18,2)");

                p.HasOne(x => x.User)
                 .WithMany(y => y.PaymentDetails)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

                p.HasOne(x => x.MatchDetail)
                 .WithMany(y => y.PaymentDetails)
                 .HasForeignKey(x => x.MatchId)
                 .OnDelete(DeleteBehavior.Restrict);
            });


            builder.Entity<MatchDetail>(md =>
            {
                md.HasKey(x => x.Id);
                md.Property(x => x.Amount)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                md.HasOne(x => x.User)
                 .WithOne(x => x.MatchDetail)
                 .HasForeignKey<MatchDetail>(x => x.PayerId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<User>(u =>
            {
                u.HasKey(x => x.Id);
                u.Property(x => x.Name)
                 .HasMaxLength(100)
                 .IsRequired();

                u.Property(x => x.Email)
                 .HasMaxLength(100)
                 .IsRequired();
                u.Property(x => x.Password)
                 .IsRequired();

            });
        }
    }
}
