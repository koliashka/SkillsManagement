using Domain;
using Microsoft.EntityFrameworkCore;

namespace SkillsManagement.Data
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
            modelBuilder.Entity<Skill>()
                .HasOne<Person>()
                .WithMany(p => p.Skills)
                .HasForeignKey(s => s.PersonId);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("varchar(100)");
                entity.Property(e => e.DisplayName).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("varchar(100)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
