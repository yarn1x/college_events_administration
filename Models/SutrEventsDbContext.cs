using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace college_events_admin_API.Models;

public partial class SutrEventsDbContext : DbContext
{
    public SutrEventsDbContext()
    {
    }

    public SutrEventsDbContext(DbContextOptions<SutrEventsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActualAttendance> ActualAttendances { get; set; }

    public virtual DbSet<AuthorizedUser> AuthorizedUsers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Criterion> Criteria { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCriterion> EventCriteria { get; set; }

    public virtual DbSet<EventGroup> EventGroups { get; set; }

    public virtual DbSet<EventPhoto> EventPhotos { get; set; }

    public virtual DbSet<EventResponsible> EventResponsibles { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPhoto> UserPhotos { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<UserUsertype> UserUsertypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActualAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__actual_a__3213E83FF8040CE4");

            entity.ToTable("actual_attendances");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualListenersCount).HasColumnName("actual_listeners_count");
            entity.Property(e => e.ActualParticipantsCount).HasColumnName("actual_participants_count");
            entity.Property(e => e.ActualSuperParticipantsCount).HasColumnName("actual_super_participants_count");
            entity.Property(e => e.EventGroupId).HasColumnName("event_group_id");

            entity.HasOne(d => d.EventGroup).WithMany(p => p.ActualAttendances)
                .HasForeignKey(d => d.EventGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__actual_at__event__66603565");
        });

        modelBuilder.Entity<AuthorizedUser>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("PK__authoriz__1F5EF42F82821064");

            entity.ToTable("authorized_users");

            entity.Property(e => e.LoginId).HasColumnName("loginID");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.MobilePhone)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("mobile_phone");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.UsernameId).HasColumnName("usernameID");

            entity.HasOne(d => d.Username).WithMany(p => p.AuthorizedUsers)
                .HasForeignKey(d => d.UsernameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__authorize__usern__4BAC3F29");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__categori__23CAF1F83CF629C1");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Criterion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__criteria__3213E83FEA01747A");

            entity.ToTable("criteria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Score).HasColumnName("score");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__events__2DC7BD694737DBA5");

            entity.ToTable("events");

            entity.Property(e => e.EventId).HasColumnName("eventID");
            entity.Property(e => e.AdditionalInfo)
                .IsUnicode(false)
                .HasColumnName("additional_info");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.FullDescription)
                .IsUnicode(false)
                .HasColumnName("full_description");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.MaxListenersCount).HasColumnName("max_listeners_count");
            entity.Property(e => e.MaxParticipantsCount).HasColumnName("max_participants_count");
            entity.Property(e => e.OrganizerId).HasColumnName("organizerID");
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(1023)
                .IsUnicode(false)
                .HasColumnName("short_description");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__events__category__59063A47");

            entity.HasOne(d => d.Location).WithMany(p => p.Events)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__events__location__59FA5E80");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__events__organize__5BE2A6F2");

            entity.HasOne(d => d.Status).WithMany(p => p.Events)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__events__status_i__5AEE82B9");
        });

        modelBuilder.Entity<EventCriterion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__event_cr__3213E83F29D9D9D2");

            entity.ToTable("event_criteria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CriterionId).HasColumnName("criterion_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");

            entity.HasOne(d => d.Criterion).WithMany(p => p.EventCriteria)
                .HasForeignKey(d => d.CriterionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_cri__crite__5FB337D6");

            entity.HasOne(d => d.Event).WithMany(p => p.EventCriteria)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_cri__event__5EBF139D");
        });

        modelBuilder.Entity<EventGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__event_gr__3213E83FAC667AF3");

            entity.ToTable("event_groups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.ExpectedListenersCount).HasColumnName("expected_listeners_count");
            entity.Property(e => e.ExpectedParticipantsCount).HasColumnName("expected_participants_count");
            entity.Property(e => e.ExpectedSuperParticipantsCount).HasColumnName("expected_super_participants_count");
            entity.Property(e => e.GroupId).HasColumnName("group_id");

            entity.HasOne(d => d.Event).WithMany(p => p.EventGroups)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_gro__event__628FA481");

            entity.HasOne(d => d.Group).WithMany(p => p.EventGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_gro__group__6383C8BA");
        });

        modelBuilder.Entity<EventPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__event_ph__3213E83F49142319");

            entity.ToTable("event_photos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("photo_url");

            entity.HasOne(d => d.Event).WithMany(p => p.EventPhotos)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_pho__event__693CA210");
        });

        modelBuilder.Entity<EventResponsible>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__event_re__3213E83F2998D02F");

            entity.ToTable("event_responsibles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Event).WithMany(p => p.EventResponsibles)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_res__event__6C190EBB");

            entity.HasOne(d => d.User).WithMany(p => p.EventResponsibles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_res__user___6D0D32F4");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__groups__88C102AD086AD704");

            entity.ToTable("groups");

            entity.Property(e => e.GroupId).HasColumnName("groupID");
            entity.Property(e => e.LoginId).HasColumnName("loginID");
            entity.Property(e => e.Name)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Login).WithMany(p => p.Groups)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__groups__loginID__4E88ABD4");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__location__30646B0EBBA47B2E");

            entity.ToTable("locations");

            entity.Property(e => e.LocationId).HasColumnName("locationID");
            entity.Property(e => e.Place)
                .HasMaxLength(511)
                .IsUnicode(false)
                .HasColumnName("place");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__news__3213E83F6212394C");

            entity.ToTable("news");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Headline)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("headline");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(1023)
                .IsUnicode(false)
                .HasColumnName("image_url");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__statuses__36257A382DD98BC5");

            entity.ToTable("statuses");

            entity.Property(e => e.StatusId).HasColumnName("statusID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(127)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsernameId).HasName("PK__users__AD82AF5F39116005");

            entity.ToTable("users");

            entity.Property(e => e.UsernameId).HasColumnName("usernameID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(127)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(127)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(127)
                .IsUnicode(false)
                .HasColumnName("middle_name");
        });

        modelBuilder.Entity<UserPhoto>(entity =>
        {
            entity.HasKey(e => e.UserPhotoId).HasName("PK__user_pho__E5F75086E7F87CA1");

            entity.ToTable("user_photos");

            entity.Property(e => e.UserPhotoId).HasColumnName("userPhotoID");
            entity.Property(e => e.LoginId).HasColumnName("loginID");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(511)
                .IsUnicode(false)
                .HasColumnName("photo_url");

            entity.HasOne(d => d.Login).WithMany(p => p.UserPhotos)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_phot__login__71D1E811");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__user_typ__F04DF11A817A9C4F");

            entity.ToTable("user_types");

            entity.Property(e => e.TypeId).HasColumnName("typeID");
            entity.Property(e => e.TypeName)
                .HasMaxLength(63)
                .IsUnicode(false)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<UserUsertype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_use__3213E83F30E20F90");

            entity.ToTable("user_usertypes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LoginId).HasColumnName("loginID");
            entity.Property(e => e.TypeId).HasColumnName("typeID");

            entity.HasOne(d => d.Login).WithMany(p => p.UserUsertypes)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_user__login__778AC167");

            entity.HasOne(d => d.Type).WithMany(p => p.UserUsertypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_user__typeI__76969D2E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
