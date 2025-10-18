using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AsanaFlowDataAccessLayer.Models;

public partial class AsanaFlowDbContext : DbContext
{
    public AsanaFlowDbContext()
    {
    }

    public AsanaFlowDbContext(DbContextOptions<AsanaFlowDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BreathingExercise> BreathingExercises { get; set; }

    public virtual DbSet<MusicPlaylist> MusicPlaylists { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SessionPose> SessionPoses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProgress> UserProgresses { get; set; }

    public virtual DbSet<YogaCategory> YogaCategories { get; set; }

    public virtual DbSet<YogaPose> YogaPoses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("AsanaFlowDBConnection");
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BreathingExercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Breathin__C121418EAE479952");

            entity.ToTable("Breathing_Exercises");

            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.Benefits)
                .HasColumnType("text")
                .HasColumnName("benefits");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExerciseName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("exercise_name");
            entity.Property(e => e.Technique)
                .HasColumnType("text")
                .HasColumnName("technique");
        });

        modelBuilder.Entity<MusicPlaylist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("PK__Music_Pl__FB9C1410C03F04EC");

            entity.ToTable("Music_Playlists");

            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.PlaylistName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("playlist_name");
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("source");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("url");

            entity.HasOne(d => d.Category).WithMany(p => p.MusicPlaylists)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Music_Pla__categ__3A81B327");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Sessions__69B13FDC24969A4C");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.TotalDuration).HasColumnName("total_duration");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Sessions__user_i__32E0915F");
        });

        modelBuilder.Entity<SessionPose>(entity =>
        {
            entity.HasKey(e => e.SessionPoseId).HasName("PK__Session___91166CBE26BCBACE");

            entity.ToTable("Session_Poses");

            entity.Property(e => e.SessionPoseId).HasColumnName("session_pose_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.PoseId).HasColumnName("pose_id");
            entity.Property(e => e.SessionId).HasColumnName("session_id");

            entity.HasOne(d => d.Pose).WithMany(p => p.SessionPoses)
                .HasForeignKey(d => d.PoseId)
                .HasConstraintName("FK__Session_P__pose___37A5467C");

            entity.HasOne(d => d.Session).WithMany(p => p.SessionPoses)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__Session_P__sessi__36B12243");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F1A583390");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164EB36A853").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AccountLockedUntil)
                .HasColumnType("datetime")
                .HasColumnName("account_locked_until");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerified)
                .HasDefaultValue(false)
                .HasColumnName("email_verified");
            entity.Property(e => e.FailedLoginAttempts)
                .HasDefaultValue(0)
                .HasColumnName("failed_login_attempts");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("last_login");
            entity.Property(e => e.MfaEnabled)
                .HasDefaultValue(false)
                .HasColumnName("mfa_enabled");
            entity.Property(e => e.MfaSecret)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mfa_secret");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.PasswordLastChanged)
                .HasColumnType("datetime")
                .HasColumnName("password_last_changed");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("user")
                .HasColumnName("role");

            entity.HasMany(d => d.Poses).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFavorite",
                    r => r.HasOne<YogaPose>().WithMany()
                        .HasForeignKey("PoseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__User_Favo__pose___4222D4EF"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__User_Favo__user___412EB0B6"),
                    j =>
                    {
                        j.HasKey("UserId", "PoseId").HasName("PK__User_Fav__702D373C32957D53");
                        j.ToTable("User_Favorites");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("PoseId").HasColumnName("pose_id");
                    });
        });

        modelBuilder.Entity<UserProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__User_Pro__49B3D8C1D885E2F1");

            entity.ToTable("User_Progress");

            entity.Property(e => e.ProgressId).HasColumnName("progress_id");
            entity.Property(e => e.LastPracticed)
                .HasColumnType("datetime")
                .HasColumnName("last_practiced");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.PoseId).HasColumnName("pose_id");
            entity.Property(e => e.ProficiencyLevel).HasColumnName("proficiency_level");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Pose).WithMany(p => p.UserProgresses)
                .HasForeignKey(d => d.PoseId)
                .HasConstraintName("FK__User_Prog__pose___3E52440B");

            entity.HasOne(d => d.User).WithMany(p => p.UserProgresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__User_Prog__user___3D5E1FD2");
        });

        modelBuilder.Entity<YogaCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Yoga_Cat__D54EE9B47F3B3B49");

            entity.ToTable("Yoga_Categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("category_name");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
        });

        modelBuilder.Entity<YogaPose>(entity =>
        {
            entity.HasKey(e => e.PoseId).HasName("PK__Yoga_Pos__9930033D29CE54EE");

            entity.ToTable("Yoga_Poses");

            entity.Property(e => e.PoseId).HasColumnName("pose_id");
            entity.Property(e => e.Benefits)
                .HasColumnType("text")
                .HasColumnName("benefits");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.DefaultTime).HasColumnName("default_time");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.Instructions)
                .HasColumnType("text")
                .HasColumnName("instructions");
            entity.Property(e => e.PoseName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pose_name");
            entity.Property(e => e.Precautions)
                .HasColumnType("text")
                .HasColumnName("precautions");

            entity.HasOne(d => d.Category).WithMany(p => p.YogaPoses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Yoga_Pose__categ__300424B4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
