using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка составного ключа для сущности Skill
            modelBuilder.Entity<Skill>()
                .HasKey(s => new { s.Name, s.PersonId }); // Составной ключ из Name и PersonId

            // Связь между Skill и Person
            modelBuilder.Entity<Skill>()
                .HasOne<Person>()
                .WithMany(p => p.Skills)
                .HasForeignKey(s => s.PersonId);

            // Настройка таблицы Person
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("varchar(100)");
                entity.Property(e => e.DisplayName).HasColumnType("varchar(100)");
            });

            // Настройка таблицы Skill
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("varchar(100)");
                entity.Property(e => e.PersonId).HasColumnType("bigint");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
