using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FootballManagerApi.Models;

namespace FootballManagerApi.Contexts
{
    public partial class FootballManagerContext : DbContext
    {
        public FootballManagerContext()
        {
        }

        public FootballManagerContext(DbContextOptions<FootballManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Stadium> Stadium { get; set; }
        public virtual DbSet<StadiumTeam> StadiumTeam { get; set; }
        public virtual DbSet<Team> Team { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=Contexts/FootballManager.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasIndex(x => x.Id);
            modelBuilder.Entity<Team>().HasIndex(x => x.Id);
            modelBuilder.Entity<Stadium>().HasIndex(x => x.Id);
            modelBuilder.Entity<StadiumTeam>().HasIndex(x => x.Id);

            modelBuilder.Entity<Player>()
                .HasOne(x => x.Team)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<StadiumTeam>()
                .HasOne(x => x.Team)
                .WithOne(x => x.StadiumTeam)
                .HasForeignKey<StadiumTeam>(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StadiumTeam>()
                .HasOne(x => x.Stadium)
                .WithOne(x => x.StadiumTeam)
                .HasForeignKey<StadiumTeam>(x => x.StadiumId)
                .OnDelete(DeleteBehavior.Cascade);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
