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
        public virtual DbSet<CountyElection> CountyElection { get; set; }
        public virtual DbSet<CountyTime> CountyTime { get; set; }
        public virtual DbSet<Countytype> Countytype { get; set; }
        public virtual DbSet<Demographic> Demographic { get; set; }
        public virtual DbSet<DieselSchemaMigrations> DieselSchemaMigrations { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Districttype> Districttype { get; set; }
        public virtual DbSet<ElectionType> ElectionType { get; set; }
        public virtual DbSet<Electionrace> Electionrace { get; set; }
        public virtual DbSet<ElectionraceType> ElectionraceType { get; set; }
        public virtual DbSet<Party> Party { get; set; }
        public virtual DbSet<PerformanceAnalysis> PerformanceAnalysis { get; set; }
        public virtual DbSet<PositionLevel> PositionLevel { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<StateTime> StateTime { get; set; }
        public virtual DbSet<Statetype> Statetype { get; set; }
        public virtual DbSet<VoterTurnout> VoterTurnout { get; set; }

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

                entity.Property(e => e.Blockid)
                    .IsRequired()
                    .HasColumnName("blockid")
                    .HasMaxLength(50);

                entity.Property(e => e.Countyid)
                    .IsRequired()
                    .HasColumnName("countyid")
                    .HasMaxLength(50);

                entity.Property(e => e.Timeend)
                    .HasColumnName("timeend")
                    .HasColumnType("date");

                entity.Property(e => e.Timestart)
                    .HasColumnName("timestart")
                    .HasColumnType("date");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.BlockCountyTime)
                    .HasForeignKey(d => d.Blockid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_county_block_blockid");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.BlockCountyTime)
                    .HasForeignKey(d => d.Countyid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_county_block_countyid");
            });

            modelBuilder.Entity<BlockDistrictTime>(entity =>
            {
                entity.ToTable("block_district_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Blockid)
                    .IsRequired()
                    .HasColumnName("blockid")
                    .HasMaxLength(50);

                entity.Property(e => e.Districtid)
                    .IsRequired()
                    .HasColumnName("districtid")
                    .HasMaxLength(50);

                entity.Property(e => e.Timeend)
                    .HasColumnName("timeend")
                    .HasColumnType("date");

                entity.Property(e => e.Timestart)
                    .HasColumnName("timestart")
                    .HasColumnType("date");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.BlockDistrictTime)
                    .HasForeignKey(d => d.Blockid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_district_block_blockid");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.BlockDistrictTime)
                    .HasForeignKey(d => d.Districtid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_district_block_districtid");
            });

            modelBuilder.Entity<BlockPopulationTime>(entity =>
            {
                entity.ToTable("block_population_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Blockid)
                    .IsRequired()
                    .HasColumnName("blockid")
                    .HasMaxLength(50);

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.Reportingdate)
                    .HasColumnName("reportingdate")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("candidate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dateofbirth)
                    .HasColumnName("dateofbirth")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Partyid).HasColumnName("partyid");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Candidate)
                    .HasForeignKey(d => d.Partyid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_party_candidate");
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
            });

            modelBuilder.Entity<CountyElection>(entity =>
            {
                entity.ToTable("county_election");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Countyid)
                    .IsRequired()
                    .HasColumnName("countyid")
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.Electiondate)
                    .HasColumnName("electiondate")
                    .HasColumnType("date");

                entity.Property(e => e.Electiontypeid).HasColumnName("electiontypeid");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.CountyElection)
                    .HasForeignKey(d => d.Countyid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_county_countyelection");

                entity.HasOne(d => d.Electiontype)
                    .WithMany(p => p.CountyElection)
                    .HasForeignKey(d => d.Electiontypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_electiontype_countyelection");
            });

            modelBuilder.Entity<CountyTime>(entity =>
            {
                entity.ToTable("county_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Countyid)
                    .IsRequired()
                    .HasColumnName("countyid")
                    .HasMaxLength(50);

                entity.Property(e => e.Stateid).HasColumnName("stateid");

                entity.Property(e => e.Timeend)
                    .HasColumnName("timeend")
                    .HasColumnType("date");

                entity.Property(e => e.Timestart)
                    .HasColumnName("timestart")
                    .HasColumnType("date");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.CountyTime)
                    .HasForeignKey(d => d.Countyid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_state_county_countyid");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.CountyTime)
                    .HasForeignKey(d => d.Stateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_state_county_stateid");
            });

            modelBuilder.Entity<Countytype>(entity =>
            {
                entity.ToTable("countytype");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Demographic>(entity =>
            {
                entity.ToTable("demographic");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Populationtimeid).HasColumnName("populationtimeid");

                entity.HasOne(d => d.Populationtime)
                    .WithMany(p => p.Demographic)
                    .HasForeignKey(d => d.Populationtimeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_populationtime_demographic");
            });

            modelBuilder.Entity<DieselSchemaMigrations>(entity =>
            {
                entity.HasKey(e => e.Version)
                    .HasName("__diesel_schema_migrations_pkey");

                entity.ToTable("__diesel_schema_migrations");

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasMaxLength(50);

                entity.Property(e => e.RunOn)
                    .HasColumnName("run_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("district");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50);

                entity.Property(e => e.Border).HasColumnName("border");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.Districttypeid).HasColumnName("districttypeid");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source");

                entity.HasOne(d => d.Districttype)
                    .WithMany(p => p.District)
                    .HasForeignKey(d => d.Districttypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_districttype_district");
            });

            modelBuilder.Entity<Districttype>(entity =>
            {
                entity.ToTable("districttype");

                entity.HasIndex(e => e.DistrictTypeCode)
                    .HasName("districttype_district_type_code_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.DistrictTypeCode)
                    .IsRequired()
                    .HasColumnName("district_type_code")
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<ElectionType>(entity =>
            {
                entity.ToTable("election_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Electionrace>(entity =>
            {
                entity.ToTable("electionrace");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Countyelectionid).HasColumnName("countyelectionid");

                entity.Property(e => e.Electionracetypeid).HasColumnName("electionracetypeid");

                entity.HasOne(d => d.Countyelection)
                    .WithMany(p => p.Electionrace)
                    .HasForeignKey(d => d.Countyelectionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_countyelection_electionrace");

                entity.HasOne(d => d.Electionracetype)
                    .WithMany(p => p.Electionrace)
                    .HasForeignKey(d => d.Electionracetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_electionracetype_electionrace");
            });

            modelBuilder.Entity<ElectionraceType>(entity =>
            {
                entity.ToTable("electionrace_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.Positionlevelid).HasColumnName("positionlevelid");

                entity.HasOne(d => d.Positionlevel)
                    .WithMany(p => p.ElectionraceType)
                    .HasForeignKey(d => d.Positionlevelid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_positionlevel_electionracetype");
            });

            modelBuilder.Entity<Party>(entity =>
            {
                entity.ToTable("party");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Abbreviation)
                    .HasColumnName("abbreviation")
                    .HasMaxLength(5);

                entity.Property(e => e.Name)
                    .IsRequired()
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

                entity.Property(e => e.Memoryused).HasColumnName("memoryused");

                entity.Property(e => e.Numberofblocks).HasColumnName("numberofblocks");

                entity.Property(e => e.Numberofcoresavailable).HasColumnName("numberofcoresavailable");

                entity.Property(e => e.Numberofdistricts).HasColumnName("numberofdistricts");

                entity.Property(e => e.States)
                    .IsRequired()
                    .HasColumnName("states");

                entity.Property(e => e.Systempagesize).HasColumnName("systempagesize");

                entity.Property(e => e.Totalruntime).HasColumnName("totalruntime");
            });

            modelBuilder.Entity<PositionLevel>(entity =>
            {
                entity.ToTable("position_level");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("result");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Candidateid).HasColumnName("candidateid");

                entity.Property(e => e.Electionraceid).HasColumnName("electionraceid");

                entity.Property(e => e.Numberofvotesrecieved).HasColumnName("numberofvotesrecieved");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.Candidateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_candidate_result");

                entity.HasOne(d => d.Electionrace)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.Electionraceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_electionrace_result");
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

                entity.Property(e => e.Countytypeid).HasColumnName("countytypeid");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source");

                entity.Property(e => e.Statetypeid).HasColumnName("statetypeid");

                entity.HasOne(d => d.Countytype)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.Countytypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_countytype_state");

                entity.HasOne(d => d.Statetype)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.Statetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_statetype_state");
            });

            modelBuilder.Entity<StateTime>(entity =>
            {
                entity.ToTable("state_time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Stateid).HasColumnName("stateid");

                entity.Property(e => e.Timeend)
                    .HasColumnName("timeend")
                    .HasColumnType("date");

                entity.Property(e => e.Timestart)
                    .HasColumnName("timestart")
                    .HasColumnType("date");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.StateTime)
                    .HasForeignKey(d => d.Stateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_statetime_state");
            });

            modelBuilder.Entity<Statetype>(entity =>
            {
                entity.ToTable("statetype");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VoterTurnout>(entity =>
            {
                entity.ToTable("voter_turnout");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Electionraceid).HasColumnName("electionraceid");

                entity.Property(e => e.Registeredvoters).HasColumnName("registeredvoters");

                entity.Property(e => e.Totalvoters).HasColumnName("totalvoters");

                entity.HasOne(d => d.Electionrace)
                    .WithMany(p => p.VoterTurnout)
                    .HasForeignKey(d => d.Electionraceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_electionrace_voterturnout");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
