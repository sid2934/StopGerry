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
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<CountyTime> CountyTime { get; set; }
        public virtual DbSet<CountyType> CountyType { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<DistrictTime> DistrictTime { get; set; }
        public virtual DbSet<DistrictType> DistrictType { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<StateTime> StateTime { get; set; }
        public virtual DbSet<StateType> StateType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
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
                    .HasColumnType("text");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnType("text");
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

            modelBuilder.Entity<County>(entity =>
            {
                entity.ToTable("County", "Location");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Border).HasColumnType("geometry");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.CountyType)
                    .WithMany(p => p.County)
                    .HasForeignKey(d => d.CountyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountyType_County");
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
                    .HasName("UQ__District__CCDD80A6D2A69062")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DistrictTypeCode)
                    .IsRequired()
                    .HasColumnName("District_Type_Code")
                    .HasMaxLength(5);
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
