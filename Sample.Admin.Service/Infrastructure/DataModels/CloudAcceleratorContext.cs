using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class CloudAcceleratorContext : DbContext
    {
        public CloudAcceleratorContext(DbContextOptions<CloudAcceleratorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountServices> AccountServices { get; set; }
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<AdminRefreshTokens> AdminRefreshTokens { get; set; }
        public virtual DbSet<AdminUsers> AdminUsers { get; set; }
        public virtual DbSet<ApplicationConfigs> ApplicationConfigs { get; set; }
        public virtual DbSet<AuthenticationConfigKeys> AuthenticationConfigKeys { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<GroupRights> GroupRights { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<LoginHistories> LoginHistories { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<PasswordPolicies> PasswordPolicies { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Subscriptions> Subscriptions { get; set; }
        public virtual DbSet<UserGroupMappings> UserGroupMappings { get; set; }
        public virtual DbSet<UserRights> UserRights { get; set; }
        public virtual DbSet<VersionModules> VersionModules { get; set; }
        public virtual DbSet<Versions> Versions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=172.29.17.138;Database=SampleQA;Username=postgres;Password=userpassdb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountServices>(entity =>
            {
                entity.HasKey(e => e.AccountServiceId)
                    .HasName("AccountServices_pkey");

                entity.ToTable("AccountServices", "admin");

                entity.Property(e => e.AccountServiceId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DbName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DbSchema)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DbServer)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Port).HasMaxLength(10);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountServices)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountServices__Accounts__AccountId");
            });

            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("Accounts_pkey");

                entity.ToTable("Accounts", "admin");

                entity.Property(e => e.AccountId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AccountGuid).HasColumnName("AccountGUID");

                entity.Property(e => e.AccountUrl).HasMaxLength(250);

                entity.Property(e => e.AuthenticationCategory)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BillingAddress)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.BillingContactPerson)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BillingEmailAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactPerson)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.GetStartVideoUrl)
                    .HasColumnName("GetStartVideoURL")
                    .HasColumnType("character varying");

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Locale)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TenantCss).HasColumnName("TenantCSS");

                entity.Property(e => e.TenantLogo).HasMaxLength(255);

                entity.Property(e => e.TimeZone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TourVideoUrl)
                    .HasColumnName("TourVideoURL")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<AdminRefreshTokens>(entity =>
            {
                entity.HasKey(e => e.AdminRefreshTokenId)
                    .HasName("AdminRefreshTokens_pkey");

                entity.ToTable("AdminRefreshTokens", "admin");

                entity.Property(e => e.AdminRefreshTokenId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ExpiryDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Token).IsRequired();
            });

            modelBuilder.Entity<AdminUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("AdminUsers_pkey");

                entity.ToTable("AdminUsers", "admin");

                entity.Property(e => e.UserId)
                    .HasIdentityOptions(null, null, 2L, null, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AuthenticationCategory)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ExternalUserId).HasMaxLength(500);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MfatypeId).HasColumnName("MFATypeId");

                entity.Property(e => e.Mobile).HasMaxLength(10);

                entity.Property(e => e.PasswordExpirationDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ApplicationConfigs>(entity =>
            {
                entity.HasKey(e => e.ApplicationConfigId)
                    .HasName("ApplicationConfig_pkey");

                entity.ToTable("ApplicationConfigs", "admin");

                entity.Property(e => e.ApplicationConfigId).ValueGeneratedNever();

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CopyrightDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.DefaultLogo)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.DefaultOrgName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<AuthenticationConfigKeys>(entity =>
            {
                entity.HasKey(e => e.AuthenticationConfigKeyId)
                    .HasName("AuthenticationConfigKeys_pkey");

                entity.ToTable("AuthenticationConfigKeys", "admin");

                entity.Property(e => e.AuthenticationConfigKeyId)
                    .HasIdentityOptions(null, null, null, 99999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AuthenticationType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ConfigKey)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City", "admin");

                entity.HasIndex(e => e.StateId)
                    .HasName("fki_City_FK");

                entity.Property(e => e.CityId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("City_FK");
            });

            modelBuilder.Entity<GroupRights>(entity =>
            {
                entity.HasKey(e => e.GroupRightId)
                    .HasName("GrpModule_Key");

                entity.ToTable("GroupRights", "admin");

                entity.Property(e => e.GroupRightId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupRights)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupsRights_Groups_GroupId");
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("Groups_pkey");

                entity.ToTable("Groups", "admin");

                entity.HasIndex(e => e.Name)
                    .HasName("UQ_Name")
                    .IsUnique();

                entity.Property(e => e.GroupId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<LoginHistories>(entity =>
            {
                entity.HasKey(e => e.LoginHistoryId)
                    .HasName("LoginHis_Id");

                entity.ToTable("LoginHistories", "admin");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_UserId");

                entity.Property(e => e.LoginHistoryId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.LastRequestMade).HasColumnType("timestamp with time zone");

                entity.Property(e => e.LogoutTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Token).IsRequired();

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Modules>(entity =>
            {
                entity.HasKey(e => e.ModuleId)
                    .HasName("Modules_pkey");

                entity.ToTable("Modules", "admin");

                entity.Property(e => e.ModuleId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Url).HasMaxLength(255);
            });

            modelBuilder.Entity<PasswordPolicies>(entity =>
            {
                entity.HasKey(e => e.PasswordPolicyId)
                    .HasName("PasswordPolicy_pkey");

                entity.ToTable("PasswordPolicies", "admin");

                entity.HasIndex(e => e.AccountId)
                    .HasName("Uq_PasswordPlicyAccountID")
                    .IsUnique();

                entity.Property(e => e.PasswordPolicyId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("Services _pkey");

                entity.ToTable("Services", "admin");

                entity.Property(e => e.ServiceId).ValueGeneratedNever();

                entity.Property(e => e.EndPointBaseAddress)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State", "admin");

                entity.Property(e => e.StateId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Subscriptions>(entity =>
            {
                entity.HasKey(e => e.SubscriptionId)
                    .HasName("Subscriptions_pkey");

                entity.ToTable("Subscriptions", "admin");

                entity.Property(e => e.SubscriptionId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CancelledOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CancelledReason).HasMaxLength(500);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.EndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.StartDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscriptions__Accounts__AccountId");
            });

            modelBuilder.Entity<UserGroupMappings>(entity =>
            {
                entity.HasKey(e => e.UserGroupMappingId)
                    .HasName("UsersGroupsMapping_pkey");

                entity.ToTable("UserGroupMappings", "admin");

                entity.Property(e => e.UserGroupMappingId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroupMappings)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersGroupMapping_Groups_GroupId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroupMappings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersGroupMapping_Users_UserId");
            });

            modelBuilder.Entity<UserRights>(entity =>
            {
                entity.HasKey(e => e.UserRightId)
                    .HasName("UserModule_pkey");

                entity.ToTable("UserRights", "admin");

                entity.Property(e => e.UserRightId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRights)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsersRights__Users__UserId");
            });

            modelBuilder.Entity<VersionModules>(entity =>
            {
                entity.HasKey(e => e.VersionModuleId)
                    .HasName("VersionModules_pkey");

                entity.ToTable("VersionModules", "admin");

                entity.Property(e => e.VersionModuleId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Versions>(entity =>
            {
                entity.HasKey(e => e.VersionId)
                    .HasName("Version_pkey");

                entity.ToTable("Versions", "admin");

                entity.Property(e => e.VersionId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.VersionCode)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
