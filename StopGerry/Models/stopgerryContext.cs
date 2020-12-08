using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace StopGerry.Models
{
    public partial class stopgerryContext : DbContext
    {
        public stopgerryContext()
        {
        }

        public stopgerryContext(DbContextOptions<stopgerryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Block> Block { get; set; }
        public virtual DbSet<BlockPopulationTime> BlockPopulationTime { get; set; }
        public virtual DbSet<Candidate> Candidate { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<Demographic> Demographic { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Office> Office { get; set; }
        public virtual DbSet<PerformanceAnalysis> PerformanceAnalysis { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<State> State { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(GlobalConfig.Configuration.GetConnectionString("Deja"), x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis")
                .HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Block>(entity =>
            {
                entity.ToTable("block");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50);

                entity.Property(e => e.Border).HasColumnName("border");

                entity.Property(e => e.Coordinates)
                    .IsRequired()
                    .HasColumnName("coordinates");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<BlockPopulationTime>(entity =>
            {
                entity.ToTable("block_population_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.BlockId)
                    .IsRequired()
                    .HasColumnName("block_id")
                    .HasMaxLength(50);

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.ReportingDate)
                    .HasColumnName("reporting_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("candidate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Party)
                    .IsRequired()
                    .HasColumnName("party")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.ToTable("county");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50);

                entity.Property(e => e.Border).HasColumnName("border");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source");

                entity.Property(e => e.StateId).HasColumnName("state_id");
            });

            modelBuilder.Entity<Demographic>(entity =>
            {
                entity.ToTable("demographic");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.PopulationTimeId).HasColumnName("population_time_id");

                entity.HasOne(d => d.PopulationTime)
                    .WithMany(p => p.Demographic)
                    .HasForeignKey(d => d.PopulationTimeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_populationtime_demographic");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("district");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Border).HasColumnName("border");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.DistrictCode)
                    .IsRequired()
                    .HasColumnName("district_code")
                    .HasMaxLength(6);

                entity.Property(e => e.DistrictType)
                    .IsRequired()
                    .HasColumnName("district_type")
                    .HasMaxLength(50);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.ToTable("office");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.PositionLevel)
                    .IsRequired()
                    .HasColumnName("position_level")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PerformanceAnalysis>(entity =>
            {
                entity.ToTable("performance_analysis");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Hostname)
                    .IsRequired()
                    .HasColumnName("hostname")
                    .HasMaxLength(60);

                entity.Property(e => e.JobId)
                    .HasColumnName("job_id")
                    .HasMaxLength(255);

                entity.Property(e => e.MemoryUsed).HasColumnName("memory_used");

                entity.Property(e => e.NumberOfBlocks).HasColumnName("number_of_blocks");

                entity.Property(e => e.NumberOfCoresAvailable).HasColumnName("number_of_cores_available");

                entity.Property(e => e.NumberOfDistricts).HasColumnName("number_of_districts");

                entity.Property(e => e.States)
                    .IsRequired()
                    .HasColumnName("states");

                entity.Property(e => e.SystemPageSize).HasColumnName("system_page_size");

                entity.Property(e => e.TotalRuntime).HasColumnName("total_runtime");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("result");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CandidateId).HasColumnName("candidate_id");

                entity.Property(e => e.CountyId)
                    .IsRequired()
                    .HasColumnName("county_id")
                    .HasMaxLength(50);

                entity.Property(e => e.DistrictCode)
                    .HasColumnName("district_code")
                    .HasMaxLength(6);

                entity.Property(e => e.NumberOfVotesRecieved).HasColumnName("number_of_votes_recieved");

                entity.Property(e => e.OfficeId).HasColumnName("office_id");

                entity.Property(e => e.Precinct)
                    .IsRequired()
                    .HasColumnName("precinct")
                    .HasMaxLength(255);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.CandidateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_candidate_result");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_county_result");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_office_result");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("state");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasColumnName("abbreviation")
                    .HasMaxLength(2);

                entity.Property(e => e.Border).HasColumnName("border");

                entity.Property(e => e.CountyType)
                    .IsRequired()
                    .HasColumnName("county_type")
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source");

                entity.Property(e => e.StateType)
                    .IsRequired()
                    .HasColumnName("state_type")
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
