using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SmileyC300API.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessLog> AccessLog { get; set; }
        public virtual DbSet<AccessPoint> AccessPoint { get; set; }
        public virtual DbSet<Authorisation> Authorisation { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<Sensor> Sensor { get; set; }
        public virtual DbSet<SensorData> SensorData { get; set; }
        public virtual DbSet<SmileyUser> SmileyUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLog>(entity =>
            {
                entity.HasKey(e => e.Sno)
                    .HasName("PK__Access_L__DDDF6446200FB78B");

                entity.ToTable("Access_Log");

                entity.Property(e => e.Sno).HasColumnName("sno");

                entity.Property(e => e.Direction)
                    .IsRequired()
                    .HasColumnName("direction")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp)
                    .HasColumnName("time_stamp")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AccessLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk5");
            });

            modelBuilder.Entity<AccessPoint>(entity =>
            {
                entity.ToTable("Access_Point");

                entity.Property(e => e.AccessPointId).HasColumnName("access_point_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityId).HasColumnName("facility_id");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.AccessPoint)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk2");
            });

            modelBuilder.Entity<Authorisation>(entity =>
            {
                entity.HasKey(e => new { e.AccessPointId, e.UserId });

                entity.Property(e => e.AccessPointId).HasColumnName("access_point_id");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.AccessPoint)
                    .WithMany(p => p.Authorisation)
                    .HasForeignKey(d => d.AccessPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Authorisation)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk4");
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.Property(e => e.FacilityId).HasColumnName("facility_id");

                entity.Property(e => e.AdminId)
                    .IsRequired()
                    .HasColumnName("admin_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BannerPic)
                    .HasColumnName("banner_pic")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BlockNumber)
                    .IsRequired()
                    .HasColumnName("block_number")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityName)
                    .IsRequired()
                    .HasColumnName("facility_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode).HasColumnName("postal_code");

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasColumnName("street_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Facility)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk1");
            });

            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.Property(e => e.SensorId).HasColumnName("sensor_id");

                entity.Property(e => e.AccessPointId).HasColumnName("access_point_id");

                entity.Property(e => e.SensorName)
                    .IsRequired()
                    .HasColumnName("sensor_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AccessPoint)
                    .WithMany(p => p.Sensor)
                    .HasForeignKey(d => d.AccessPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk6");
            });

            modelBuilder.Entity<SensorData>(entity =>
            {
                entity.HasKey(e => e.Sno)
                    .HasName("PK__Sensor_D__DDDF6446856944F8");

                entity.ToTable("Sensor_Data");

                entity.Property(e => e.Sno).HasColumnName("sno");

                entity.Property(e => e.FeedbackGesture)
                    .IsRequired()
                    .HasColumnName("feedback_gesture")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SensorId).HasColumnName("sensor_id");

                entity.Property(e => e.TimeStamp)
                    .HasColumnName("time_stamp")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Sensor)
                    .WithMany(p => p.SensorData)
                    .HasForeignKey(d => d.SensorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk7");
            });

            modelBuilder.Entity<SmileyUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Smiley_U__B9BE370F6FB588A2");

                entity.ToTable("Smiley_User");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNum)
                    .HasColumnName("phone_num")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Picfile)
                    .IsRequired()
                    .HasColumnName("picfile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Voicefile)
                    .IsRequired()
                    .HasColumnName("voicefile")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
