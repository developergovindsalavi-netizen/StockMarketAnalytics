using Microsoft.EntityFrameworkCore;
using StockMarketAnalytics.Api.Models;

namespace StockMarketAnalytics.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Instrument> Instruments => Set<Instrument>();
        public DbSet<HistoricalPrice> HistoricalPrice => Set<HistoricalPrice>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.ToTable("Instruments", "dbo");

                entity.HasKey(x => x.InstrumentId);

                entity.HasIndex(x => new { x.Exchange, x.Symbol })
                    .IsUnique();

                entity.Property(x => x.Symbol)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.CompanyName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Exchange)
                    .HasMaxLength(10)
                    .IsRequired();
            });

            modelBuilder.Entity<HistoricalPrice>(entity =>
            {
                entity.ToTable("HistoricalPrices", "dbo");

                entity.HasKey(x => x.HistoricalPriceId);

                entity.HasIndex(x => new { x.InstrumentId, x.CandleTimeUtc })
                .IsUnique();

                entity.Property(x => x.Open).HasPrecision(18, 4);
                entity.Property(x => x.High).HasPrecision(18, 4);
                entity.Property(x => x.Low).HasPrecision(18, 4);
                entity.Property(x => x.Close).HasPrecision(18, 4);
                entity.Property(x => x.Volume)
                .HasColumnType("bigint")
                .IsRequired();

                entity.HasOne(x => x.Instrument)
                .WithMany()
                .HasForeignKey(x => x.InstrumentId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
