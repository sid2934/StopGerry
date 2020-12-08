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
        public virtual DbSet<BlockCountyTime> BlockCountyTime { get; set; }
        public virtual DbSet<BlockDistrictTime> BlockDistrictTime { get; set; }
        public virtual DbSet<BlockPopulationTime> BlockPopulationTime { get; set; }
        public virtual DbSet<Candidate> Candidate { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<CountyTime> CountyTime { get; set; }
        public virtual DbSet<Demographic> Demographic { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<DistrictTime> DistrictTime { get; set; }
        public virtual DbSet<Office> Office { get; set; }
        public virtual DbSet<PerformanceAnalysis> PerformanceAnalysis { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<StateTime> StateTime { get; set; }

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

            modelBuilder.Entity<BlockCountyTime>(entity =>
            {
                entity.ToTable("block_county_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.BlockId)
                    .IsRequired()
                    .HasColumnName("block_id")
                    .HasMaxLength(50);

                entity.Property(e => e.CountyId)
                    .IsRequired()
                    .HasColumnName("county_id")
                    .HasMaxLength(50);

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("date");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("date");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.BlockCountyTime)
                    .HasForeignKey(d => d.BlockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_county_block_block_id");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.BlockCountyTime)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_county_block_county_id");
            });

            modelBuilder.Entity<BlockDistrictTime>(entity =>
            {
                entity.ToTable("block_district_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.BlockId)
                    .IsRequired()
                    .HasColumnName("block_id")
                    .HasMaxLength(50);

                entity.Property(e => e.DistrictId).HasColumnName("district_id");

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("date");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("date");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.BlockDistrictTime)
                    .HasForeignKey(d => d.BlockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_district_block_block_id");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.BlockDistrictTime)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_district_block_district_id");
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

            modelBuilder.Entity<CountyTime>(entity =>
            {
                entity.ToTable("county_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CountyId)
                    .IsRequired()
                    .HasColumnName("county_id")
                    .HasMaxLength(50);

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("date");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("date");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.CountyTime)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_state_county_county_id");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.CountyTime)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_state_county_state_id");
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

            modelBuilder.Entity<DistrictTime>(entity =>
            {
                entity.ToTable("district_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.DistrictId)
                    .IsRequired()
                    .HasColumnName("district_id")
                    .HasMaxLength(50);

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("date");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("date");
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

                entity.Property(e => e.ElectionDate)
                    .HasColumnName("election_date")
                    .HasColumnType("date");

                entity.Property(e => e.ElectionType)
                    .IsRequired()
                    .HasColumnName("election_type")
                    .HasMaxLength(50);

                entity.Property(e => e.NumberOfVotesRecieved).HasColumnName("number_of_votes_recieved");

                entity.Property(e => e.Office)
                    .IsRequired()
                    .HasColumnName("office")
                    .HasMaxLength(255);

                entity.Property(e => e.Precinct)
                    .HasColumnName("precinct")
                    .HasMaxLength(255);

                entity.Property(e => e.ResultResolution)
                    .IsRequired()
                    .HasColumnName("result_resolution")
                    .HasMaxLength(8);

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

            modelBuilder.Entity<StateTime>(entity =>
            {
                entity.ToTable("state_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("date");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("date");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.StateTime)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_state_time_state");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
