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
        public virtual DbSet<StadiumTeamLink> StadiumTeamLink { get; set; }
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
            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever(); //TODO NW: is this necessary? similar with others in file
            });

            modelBuilder.Entity<Stadium>(entity =>
            {
                entity
                    .HasOne(e => e.StadiumTeamLink)
                    .WithOne(e => e.Stadium)
                    .HasForeignKey<StadiumTeamLink>(e => e.StadiumId);

                entity.Property(e => e.Id).ValueGeneratedNever(); 


            });

            modelBuilder.Entity<StadiumTeamLink>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.StadiumId)
                    .IsUnique();

                entity.HasIndex(e => e.TeamId)
                    .IsUnique();
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity
                    .HasOne(e => e.StadiumTeamLink)
                    .WithOne(e => e.Team)
                    .HasForeignKey<StadiumTeamLink>(e => e.StadiumId);

                entity
                    .HasMany(x => x.Players)
                    .WithOne(x => x.Team)
                    .HasForeignKey(x => x.TeamId);

                entity.Property(e => e.Id).ValueGeneratedNever();

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
