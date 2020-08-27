using DietAssistant.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DietAssistant.DAL.DataContext
{
    public sealed class DietAssistantContext : DbContext
    {
        private string _connectionString; 

        public DbSet<ConsumedDish> ConsumedDishes { get; set; }
        public DbSet<ConsumeTimeType> ConsumeTimeTypes { get; set; }
        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DietAssistantContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DietAssistantContext(DbContextOptions<DietAssistantContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    sqlOptions.ExecutionStrategy(deps => new SqlServerRetryingExecutionStrategy(
                        dependencies: deps,
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: new int[] { 10060, 233, 40613 }
                        )
                    );
                });
            }   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConsumedDish>(cd =>
            {
                cd.HasKey(p => p.Id);

                cd.ToTable(nameof(ConsumedDish));

                cd.Property(p => p.Name)
                  .HasMaxLength(30);

                cd.Property(p => p.Description)
                  .HasMaxLength(150)
                  .IsRequired(false);

                cd.HasOne(p => p.ConsumeTimeType)
                  .WithMany(ct => ct.ConsumedDishes)
                  .HasForeignKey(k => k.ConsumeTimeTypeId);

                cd.HasOne(p => p.User)
                  .WithMany(u => u.ConsumedDishes)
                  .HasForeignKey(k => k.UserId);

                cd.Property(p => p.IsFoodStuff)
                  .HasDefaultValue(true);
            });

            modelBuilder.Entity<DailyReport>(dr =>
            {
                dr.HasKey(p => p.Id);

                dr.ToTable(nameof(DailyReport));

                dr.Property(p => p.Warnings)
                   .HasMaxLength(200)
                   .HasDefaultValue("");

                dr.HasOne(p => p.User)
                  .WithMany(u => u.DailyReports)
                  .HasForeignKey(k => k.UserId);
            });

            modelBuilder.Entity<ConsumeTimeType>(ctt => 
            {
                ctt.HasKey(p => p.Id);

                ctt.ToTable(nameof(ConsumeTimeType));

                ctt.Property(p => p.Name)
                   .HasMaxLength(30);
            });

            modelBuilder.Entity<BodyType>(bt =>
            {
                bt.HasKey(p => p.Id);

                bt.ToTable(nameof(BodyType));
            });

            modelBuilder.Entity<Role>(r =>
            {
                r.HasKey(p => p.Id);

                r.ToTable(nameof(Role));

                r.Property(p => p.Name)
                   .HasMaxLength(30);
            });

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(p => p.Id);

                u.ToTable(nameof(User));

                u.Property(p => p.Name)
                  .HasMaxLength(30);

                u.Property(p => p.Surname)
                  .HasMaxLength(30);

                u.Property(p => p.MiddleName)
                  .HasMaxLength(30)
                  .IsRequired(false);

                u.Property(p => p.WeightInKilos);

                u.Property(p => p.HeightInMeters);

                u.HasOne(p => p.Role)
                  .WithMany(ct => ct.Users)
                  .HasForeignKey(k => k.RoleId);

                u.HasOne(p => p.BodyType)
                 .WithMany(bt => bt.Users)
                 .HasForeignKey(p => p.BodyTypeId);
            });
        }
    }
}
