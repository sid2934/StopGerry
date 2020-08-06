using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data_Ingest.Models
{
    public partial class StopGerryPrdContext : DbContext
    {
        public StopGerryPrdContext()
        {
        }

        public StopGerryPrdContext(DbContextOptions<StopGerryPrdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Block> Block { get; set; }
        public virtual DbSet<BlockCountyTime> BlockCountyTime { get; set; }
        public virtual DbSet<BlockDistrictTime> BlockDistrictTime { get; set; }
        public virtual DbSet<BlockPopulationTime> BlockPopulationTime { get; set; }
        public virtual DbSet<Candidate> Candidate { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<CountyElection> CountyElection { get; set; }
        public virtual DbSet<CountyTime> CountyTime { get; set; }
        public virtual DbSet<CountyType> CountyType { get; set; }
        public virtual DbSet<Demographic> Demographic { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<DistrictTime> DistrictTime { get; set; }
        public virtual DbSet<DistrictType> DistrictType { get; set; }
        public virtual DbSet<ElectionType> ElectionType { get; set; }
        public virtual DbSet<Party> Party { get; set; }
        public virtual DbSet<PositionLevel> PositionLevel { get; set; }
        public virtual DbSet<Race> Race { get; set; }
        public virtual DbSet<RaceType> RaceType { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<StateTime> StateTime { get; set; }
        public virtual DbSet<StateType> StateType { get; set; }
        public virtual DbSet<VoterTurnout> VoterTurnout { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=hange; Database=StopGerryPrd; User Id=SA; Password=74z2WR#zlcC@1I;", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Block>(entity =>
            {
                entity.ToTable("Block", "Location");

                entity.HasComment("Contains all of the state precincts as defined in the 2010 Census voting districts codes. This is the base unit for all data moving forward. It is possible that these districts have been different over time, we will attempt to make everything fit into this definition.");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Border).HasColumnType("geometry");

                entity.Property(e => e.Coordinates)
                    .IsRequired()
                    .HasColumnType("geometry");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<BlockCountyTime>(entity =>
            {
                entity.ToTable("Block_County_Time", "Location");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BlockId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CountyId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeEnd).HasColumnType("date");

                entity.Property(e => e.TimeStart).HasColumnType("date");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.BlockCountyTime)
                    .HasForeignKey(d => d.BlockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_County_Block_BlockId");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.BlockCountyTime)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_County_Block_CountyId");
            });

            modelBuilder.Entity<BlockDistrictTime>(entity =>
            {
                entity.ToTable("Block_District_Time", "Location");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BlockId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DistrictId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeEnd).HasColumnType("date");

                entity.Property(e => e.TimeStart).HasColumnType("date");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.BlockDistrictTime)
                    .HasForeignKey(d => d.BlockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District_Block_BlockId");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.BlockDistrictTime)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District_Block_DistrictId");
            });

            modelBuilder.Entity<BlockPopulationTime>(entity =>
            {
                entity.ToTable("Block_Population_Time", "Census");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BlockId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReportingDate).HasColumnType("date");
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("Candidate", "Election");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Candidate)
                    .HasForeignKey(d => d.PartyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Party_Candidate");
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.ToTable("County", "Location");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Border).HasColumnType("geometry");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnType("text");
            });

            modelBuilder.Entity<CountyElection>(entity =>
            {
                entity.ToTable("County_Election", "Election");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CountyId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ElectionDate).HasColumnType("date");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.CountyElection)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_County_CountyElection");

                entity.HasOne(d => d.ElectionType)
                    .WithMany(p => p.CountyElection)
                    .HasForeignKey(d => d.ElectionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ElectionType_CountyElection");
            });

            modelBuilder.Entity<CountyTime>(entity =>
            {
                entity.ToTable("County_Time", "Location");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CountyId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeEnd).HasColumnType("date");

                entity.Property(e => e.TimeStart).HasColumnType("date");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.CountyTime)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_State_County_CountyId");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.CountyTime)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_State_County_StateId");
            });

            modelBuilder.Entity<CountyType>(entity =>
            {
                entity.ToTable("CountyType", "Location");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Demographic>(entity =>
            {
                entity.ToTable("Demographic", "Census");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.PopulationTime)
                    .WithMany(p => p.Demographic)
                    .HasForeignKey(d => d.PopulationTimeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PopulationTime_Demographic");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District", "Location");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Border).HasColumnType("geometry");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.DistrictType)
                    .WithMany(p => p.District)
                    .HasForeignKey(d => d.DistrictTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DistrictType_District");
            });

            modelBuilder.Entity<DistrictTime>(entity =>
            {
                entity.ToTable("District_Time", "Location");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DistrictId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeEnd).HasColumnType("date");

                entity.Property(e => e.TimeStart).HasColumnType("date");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.DistrictTime)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DistrictTime_District");
            });

            modelBuilder.Entity<DistrictType>(entity =>
            {
                entity.ToTable("DistrictType", "Location");

                entity.HasIndex(e => e.DistrictTypeCode)
                    .HasName("UQ__District__CCDD80A6E08C2771")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DistrictTypeCode)
                    .IsRequired()
                    .HasColumnName("District_Type_Code")
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<ElectionType>(entity =>
            {
                entity.ToTable("Election_Type", "Election");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Party>(entity =>
            {
                entity.ToTable("Party", "Election");

                entity.Property(e => e.Abbreviation).HasMaxLength(5);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PositionLevel>(entity =>
            {
                entity.ToTable("Position_Level", "Election");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.ToTable("Race", "Election");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.CountyElection)
                    .WithMany(p => p.Race)
                    .HasForeignKey(d => d.CountyElectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountyElection_Race");

                entity.HasOne(d => d.RaceType)
                    .WithMany(p => p.Race)
                    .HasForeignKey(d => d.RaceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RaceType_Race");
            });

            modelBuilder.Entity<RaceType>(entity =>
            {
                entity.ToTable("Race_Type", "Election");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PositionLevel)
                    .WithMany(p => p.RaceType)
                    .HasForeignKey(d => d.PositionLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PositionLevel_RaceType");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Result", "Election");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.CandidateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidate_Result");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.RaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Race_Result");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State", "Location");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.Border).HasColumnType("geometry");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.CountyType)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.CountyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountyType_State");

                entity.HasOne(d => d.StateType)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.StateTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StateType_State");
            });

            modelBuilder.Entity<StateTime>(entity =>
            {
                entity.ToTable("State_Time", "Location");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.TimeEnd).HasColumnType("date");

                entity.Property(e => e.TimeStart).HasColumnType("date");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.StateTime)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StateTime_State");
            });

            modelBuilder.Entity<StateType>(entity =>
            {
                entity.ToTable("StateType", "Location");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VoterTurnout>(entity =>
            {
                entity.ToTable("Voter_Turnout", "Election");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.VoterTurnout)
                    .HasForeignKey(d => d.RaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Race_VoterTurnout");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
