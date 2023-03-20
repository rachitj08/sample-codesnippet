using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class CloudAcceleratorContext : DbContext
    {
        public readonly string schemaId;
        public CloudAcceleratorContext(DbContextOptions options, string schema = "customer")
            : base(options)
        {
            this.schemaId = schema ?? "customer";
        }

        public virtual DbSet<ActivityCode> ActivityCode { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<AgreementTemplate> AgreementTemplate { get; set; }
        public virtual DbSet<AgreementType> AgreementType { get; set; }
        public virtual DbSet<Airline> Airline { get; set; }
        public virtual DbSet<Airports> Airports { get; set; }
        public virtual DbSet<AirportsParking> AirportsParking { get; set; }
        public virtual DbSet<ApplicationGroupMapping> ApplicationGroupMapping { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<AuthenticationConfigKeysValues> AuthenticationConfigKeysValues { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<ControlType> ControlType { get; set; }
        public virtual DbSet<ControlTypePickListOptions> ControlTypePickListOptions { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<DataSourceFieldValidation> DataSourceFieldValidation { get; set; }
        public virtual DbSet<DataSourceFields> DataSourceFields { get; set; }
        public virtual DbSet<DataSources> DataSources { get; set; }
        public virtual DbSet<EmailParserDetail> EmailParserDetail { get; set; }
        public virtual DbSet<FlightReservation> FlightReservation { get; set; }
        public virtual DbSet<GroupRights> GroupRights { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public virtual DbSet<LoginHistories> LoginHistories { get; set; }
        public virtual DbSet<MoneyTransferTypes> MoneyTransferTypes { get; set; }
        public virtual DbSet<OtpLog> OtpLog { get; set; }
        public virtual DbSet<ParkingHeads> ParkingHeads { get; set; }
        public virtual DbSet<ParkingHeadsCustomRate> ParkingHeadsCustomRate { get; set; }
        public virtual DbSet<ParkingHeadsRate> ParkingHeadsRate { get; set; }
        public virtual DbSet<ParkingProviders> ParkingProviders { get; set; }
        public virtual DbSet<ParkingProvidersLocations> ParkingProvidersLocations { get; set; }
        public virtual DbSet<ParkingProvidersLocationsSubLocations> ParkingProvidersLocationsSubLocations { get; set; }
        public virtual DbSet<ParkingProvidersLocationsSubLocationsBkp> ParkingProvidersLocationsSubLocationsBkp { get; set; }
        public virtual DbSet<ParkingProvidersLocationsSubLocationsQrBkp> ParkingProvidersLocationsSubLocationsQrBkp { get; set; }
        public virtual DbSet<ParkingReservation> ParkingReservation { get; set; }
        public virtual DbSet<ParkingSpotType> ParkingSpotType { get; set; }
        public virtual DbSet<ParkingSpots> ParkingSpots { get; set; }
        public virtual DbSet<PasswordHistories> PasswordHistories { get; set; }
        public virtual DbSet<PasswordPolicies> PasswordPolicies { get; set; }
        public virtual DbSet<PaymentDetails> PaymentDetails { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public virtual DbSet<RentalHeadRates> RentalHeadRates { get; set; }
        public virtual DbSet<RentalHeads> RentalHeads { get; set; }
        public virtual DbSet<RentalRate> RentalRate { get; set; }
        public virtual DbSet<RentalReservation> RentalReservation { get; set; }
        public virtual DbSet<RentalReservationCarFeatures> RentalReservationCarFeatures { get; set; }
        public virtual DbSet<RentalReservationOption> RentalReservationOption { get; set; }
        public virtual DbSet<RequestLogs> RequestLogs { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<ReservationActivityCode> ReservationActivityCode { get; set; }
        public virtual DbSet<ReservationPaymentHistory> ReservationPaymentHistory { get; set; }
        public virtual DbSet<ReservationType> ReservationType { get; set; }
        public virtual DbSet<ReservationVehicle> ReservationVehicle { get; set; }
        public virtual DbSet<ScreenFields> ScreenFields { get; set; }
        public virtual DbSet<Screens> Screens { get; set; }
        public virtual DbSet<Shuttle> Shuttle { get; set; }
        public virtual DbSet<Source> Source { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<SubLocationType> SubLocationType { get; set; }
        public virtual DbSet<TripPaxAndBags> TripPaxAndBags { get; set; }
        public virtual DbSet<UserBankAccounts> UserBankAccounts { get; set; }
        public virtual DbSet<UserCreditCardsWillbedelete> UserCreditCardsWillbedelete { get; set; }
        public virtual DbSet<UserDeviceHistory> UserDeviceHistory { get; set; }
        public virtual DbSet<UserDrivingLicense> UserDrivingLicense { get; set; }
        public virtual DbSet<UserGroupMappings> UserGroupMappings { get; set; }
        public virtual DbSet<UserRights> UserRights { get; set; }
        public virtual DbSet<UserVehiclePreferenceCategory> UserVehiclePreferenceCategory { get; set; }
        public virtual DbSet<UserVehiclePreferenceFeatures> UserVehiclePreferenceFeatures { get; set; }
        public virtual DbSet<UserVehicles> UserVehicles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersPaymentMethod> UsersPaymentMethod { get; set; }
        public virtual DbSet<ValidationTypes> ValidationTypes { get; set; }
        public virtual DbSet<VehicleAvailablity> VehicleAvailablity { get; set; }
        public virtual DbSet<VehicleCategory> VehicleCategory { get; set; }
        public virtual DbSet<VehicleFeatures> VehicleFeatures { get; set; }
        public virtual DbSet<VehicleFeaturesMapping> VehicleFeaturesMapping { get; set; }
        public virtual DbSet<VehicleTagMapping> VehicleTagMapping { get; set; }
        public virtual DbSet<Vehicles> Vehicles { get; set; }
        public virtual DbSet<VehiclesEventLog> VehiclesEventLog { get; set; }
        public virtual DbSet<VehiclesMediaStorage> VehiclesMediaStorage { get; set; }
        public virtual DbSet<VehiclesMediaType> VehiclesMediaType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Server=prod-Sample-rds.cjddjaqddx3b.us-east-2.rds.amazonaws.com; Database=SampleUAT; user id=postgresadmin; password=C6l0A+rltU21hlg!TRaV");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityCode>(entity =>
            {
                entity.ToTable("ActivityCode", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_FK_ActivityCode_AccountId");

                entity.Property(e => e.ActivityCodeId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ActivityCodeFor)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ScanType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address", schemaId);

                entity.Property(e => e.AddressId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Latitude).HasMaxLength(100);

                entity.Property(e => e.Longitude).HasMaxLength(100);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Streat1)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Streat2).HasMaxLength(500);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<AddressType>(entity =>
            {
                entity.ToTable("AddressType", schemaId);

                entity.Property(e => e.AddressTypeId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<AgreementTemplate>(entity =>
            {
                entity.ToTable("AgreementTemplate", schemaId);

                entity.HasIndex(e => e.AgreementTypeId)
                    .HasName("fki_AgreementTemplate_AgreementTypeId_FK");

                entity.HasIndex(e => e.StateId)
                    .HasName("fki_AgreementTypeId_StateId");

                entity.Property(e => e.AgreementTemplateId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AgreementEffectiveDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.AgreementExpirationDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.TemplateText)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.AgreementType)
                    .WithMany(p => p.AgreementTemplate)
                    .HasForeignKey(d => d.AgreementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AgreementTemplate_AgreementTypeId_FK");
            });

            modelBuilder.Entity<AgreementType>(entity =>
            {
                entity.ToTable("AgreementType", schemaId);

                entity.Property(e => e.AgreementTypeId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Airline>(entity =>
            {
                entity.ToTable("Airline", schemaId);

                entity.Property(e => e.AirlineId)
                    .HasIdentityOptions(1098L, null, null, null, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CallSign).HasMaxLength(100);

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Iata)
                    .HasColumnName("IATA")
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Airports>(entity =>
            {
                entity.HasKey(e => e.AirportId)
                    .HasName("Airports_pkey");

                entity.ToTable("Airports", schemaId);

                entity.HasIndex(e => e.AddressId)
                    .HasName("fki_Airpots_Address_FK");

                entity.Property(e => e.AirportId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Airports)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Airpots_Address_FK");
            });

            modelBuilder.Entity<AirportsParking>(entity =>
            {
                entity.ToTable("AirportsParking", schemaId);

                entity.HasIndex(e => e.AirportId)
                    .HasName("fki_AirportsParking_Temp_AirportId_FK");

                entity.HasIndex(e => e.ParkingProvidersLocationId)
                    .HasName("fki_AirportsParking_ParkingProvidersLocationId_FK");

                entity.Property(e => e.AirportsParkingId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Airport)
                    .WithMany(p => p.AirportsParking)
                    .HasForeignKey(d => d.AirportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AirportsParking_AirportId_FK");

                entity.HasOne(d => d.ParkingProvidersLocation)
                    .WithMany(p => p.AirportsParking)
                    .HasForeignKey(d => d.ParkingProvidersLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AirportsParking_ParkingProvidersLocationId_FK");
            });

            modelBuilder.Entity<ApplicationGroupMapping>(entity =>
            {
                entity.ToTable("ApplicationGroupMapping", schemaId);

                entity.Property(e => e.ApplicationGroupMappingId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationGroupMapping)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ApplicationGroupMapping_ApplicationId_fkey");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ApplicationGroupMapping)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GroupId");
            });

            modelBuilder.Entity<Applications>(entity =>
            {
                entity.ToTable("Applications", schemaId);

                entity.Property(e => e.ApplicationsId).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<AuthenticationConfigKeysValues>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.AuthenticationConfigKeyId })
                    .HasName("AuthenticationConfigKeysValues_pkey");

                entity.ToTable("AuthenticationConfigKeysValues", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_fk_authconfigkeyvalue_accounts");

                entity.HasIndex(e => e.AuthenticationConfigKeyId)
                    .HasName("fki_fk_authconfigkeyvalue_authenticationconfigkeyid");

                entity.Property(e => e.AuthenticationConfigKeysValueId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("City", schemaId);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<ControlType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ControlType", schemaId);

                entity.HasComment("This table is the master table for all control types");

                entity.Property(e => e.ControlType1)
                    .IsRequired()
                    .HasColumnName("ControlType")
                    .HasMaxLength(100)
                    .HasComment("Type of control");

                entity.Property(e => e.ControlTypeId)
                    .HasComment("Identity Field (1,1), Represents unique id for a control type")
                    .ValueGeneratedOnAdd()
                    .UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<ControlTypePickListOptions>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ControlTypePickListOptions", schemaId);

                entity.HasComment("This tablecontains option values for Pick List Control Type");

                entity.Property(e => e.ControlTypeFieldId).HasComment("ID of ControlType Field (ControlType.ControlTypeID)");

                entity.Property(e => e.DisplayOrder).HasComment("Contain Order number value");

                entity.Property(e => e.ItemText)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Text of Control");

                entity.Property(e => e.ItemValue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Value of Control");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Country", schemaId);

                entity.Property(e => e.CountryCode).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<DataSourceFieldValidation>(entity =>
            {
                entity.ToTable("DataSourceFieldValidation", schemaId);

                entity.Property(e => e.DataSourceFieldValidationId)
                    .HasIdentityOptions(null, null, null, 99999L, null, null)
                    .UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<DataSourceFields>(entity =>
            {
                entity.HasKey(e => e.FieldId)
                    .HasName("DataSourceFields_pkey");

                entity.ToTable("DataSourceFields", schemaId);

                entity.Property(e => e.FieldId)
                    .HasComment("Identity Field (1,1), Represents unique id for a datasourcefield")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ControlTypeForAdd)
                    .HasMaxLength(100)
                    .HasComment("Represents which type of control to be used on Add screen for this field");

                entity.Property(e => e.ControlTypeForEdit).HasMaxLength(100);

                entity.Property(e => e.ControlTypeForList)
                    .HasMaxLength(100)
                    .HasComment("Represents which type of control to be used on List screen for this field");

                entity.Property(e => e.ControlTypeForSearch).HasMaxLength(100);

                entity.Property(e => e.ControlTypeForView)
                    .HasMaxLength(100)
                    .HasComment("Represents which type of control to be used on View screen for this field");

                entity.Property(e => e.DataSourceName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("FK (DataSources.DataSourceName) Name of corresponding DataSource");

                entity.Property(e => e.DefaultValue)
                    .HasMaxLength(100)
                    .HasComment("Represents if any default value is associated with this field");

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("PK, Name of the data field");

                entity.Property(e => e.HelpText)
                    .HasColumnType("character varying")
                    .HasComment("About this field");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ListDataSourceName)
                    .HasMaxLength(100)
                    .HasComment("Datasource for dropdown list");

                entity.Property(e => e.ListPrimaryKey).HasMaxLength(100);

                entity.Property(e => e.ListTextField)
                    .HasMaxLength(100)
                    .HasComment("Datasource field name for text to be displayed on screen");

                entity.Property(e => e.ListValueField)
                    .HasMaxLength(100)
                    .HasComment("Datasource field name for value");

                entity.Property(e => e.MaxLength).HasComment("Maximum no. of characters allowed");

                entity.Property(e => e.MaxValue).HasComment("Maximum value");

                entity.Property(e => e.MinValue).HasComment("Manimum value");

                entity.Property(e => e.MultiLine).HasComment("Represents if value is displayed in single line or multiline");

                entity.Property(e => e.Required).HasComment("1 represents that value for this field cannot be left blank");

                entity.Property(e => e.ShortLabel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Represents short lable for this field in case of space constraint");

                entity.Property(e => e.ValidationType)
                    .HasMaxLength(100)
                    .HasComment("Represents type of validation to be applied on field");

                entity.HasOne(d => d.DataSourceNameNavigation)
                    .WithMany(p => p.DataSourceFields)
                    .HasForeignKey(d => d.DataSourceName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DataSourceName");
            });

            modelBuilder.Entity<DataSources>(entity =>
            {
                entity.HasKey(e => e.DataSourceName)
                    .HasName("DataSources_pkey");

                entity.ToTable("DataSources", schemaId);

                entity.HasComment("This table contains information of the data source tables");

                entity.Property(e => e.DataSourceName)
                    .HasMaxLength(100)
                    .HasComment("PK, Name of data source table");

                entity.Property(e => e.AccountId).HasComment("Id of the Tenant");

                entity.Property(e => e.AddScreenTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Represents title on Add screen");

                entity.Property(e => e.CanAdd).HasComment("Used to indicate if value can be added or not");

                entity.Property(e => e.CanDelete).HasComment("Used to indicate if value can be deleted or not");

                entity.Property(e => e.CanEdit).HasComment("Used to indicate if value can be edited or not");

                entity.Property(e => e.DataSourceId)
                    .HasComment("Identity Field (1,1), Represents unique id for a datasource")
                    .ValueGeneratedOnAdd()
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.DataSourcePrimaryKey).HasColumnType("character varying");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasComment("Description of data source table");

                entity.Property(e => e.EditScreenTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Represents title on Edit screen");

                entity.Property(e => e.ListScreenTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Represents title on List screen");

                entity.Property(e => e.ServiceId).HasComment("Id of the microservice to which Tenant is subscribed.");
            });

            modelBuilder.Entity<EmailParserDetail>(entity =>
            {
                entity.ToTable("EmailParserDetail", schemaId);

                entity.Property(e => e.EmailParserDetailId)
                    .HasIdentityOptions(null, null, null, 99999999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Mobile).HasMaxLength(13);

                entity.Property(e => e.MobileCode).HasMaxLength(4);

                entity.Property(e => e.RequestId).IsRequired();

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<FlightReservation>(entity =>
            {
                entity.ToTable("FlightReservation", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_Flightreservation_Accountid_FK");

                entity.HasIndex(e => e.DepaurtureAirportId)
                    .HasName("fki_FlightReservation_DepartureAirportId_FK");

                entity.HasIndex(e => e.FlyingToAirportld)
                    .HasName("fki_FlightReservation_FlyingToAirportId_FK");

                entity.HasIndex(e => e.ReservationId)
                    .HasName("fki_FlightReservation_Reservationid_FK");

                entity.HasIndex(e => e.ReturningToAirportld)
                    .HasName("fki_FlightReservation_ReturningAirportId_FK");

                entity.Property(e => e.FlightReservationId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DepaurtureDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.FlyingToAirline)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FlyingToFlightNo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ReservationCode).HasMaxLength(100);

                entity.Property(e => e.ReturnDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ReturningToAirline).HasColumnType("character varying");

                entity.Property(e => e.ReturningToFlightNo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Status).HasComment("0~Pending|1~Confirm|2~Skip");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.DepaurtureAirport)
                    .WithMany(p => p.FlightReservationDepaurtureAirport)
                    .HasForeignKey(d => d.DepaurtureAirportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FlightReservation_DepartureAirportId_FK");

                entity.HasOne(d => d.FlyingToAirportldNavigation)
                    .WithMany(p => p.FlightReservationFlyingToAirportldNavigation)
                    .HasForeignKey(d => d.FlyingToAirportld)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FlightReservation_FlyingToAirportId_FK");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.FlightReservation)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Flightreservation_Reservationid_FK");

                entity.HasOne(d => d.ReturningToAirportldNavigation)
                    .WithMany(p => p.FlightReservationReturningToAirportldNavigation)
                    .HasForeignKey(d => d.ReturningToAirportld)
                    .HasConstraintName("FlightReservation_ReturningAirportId_FK");
            });

            modelBuilder.Entity<GroupRights>(entity =>
            {
                entity.HasKey(e => e.GroupRightId)
                    .HasName("GrpModule_Key");

                entity.ToTable("GroupRights", schemaId);

                entity.Property(e => e.GroupRightId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupRights)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_GroupRights");
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("Groups_pkey");

                entity.ToTable("Groups", schemaId);

                entity.HasIndex(e => new { e.Name, e.AccountId })
                    .HasName("UQ_Name_Account")
                    .IsUnique();

                entity.Property(e => e.GroupId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_Invoice_Accountid_FK");

                entity.HasIndex(e => e.ParkingReservationId)
                    .HasName("fki_Invoice_ReservationId_FK");

                entity.Property(e => e.InvoiceId)
                    .HasIdentityOptions(null, null, null, 999999999999999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.InvoiceDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.InvoiceNo).HasMaxLength(100);

                entity.Property(e => e.InvoicePath).HasMaxLength(200);

                entity.Property(e => e.InvoiceType).HasComment("QUOTE|INVOICE|LEASE|PARKING");

                entity.Property(e => e.TotalAmount).HasColumnType("money");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ParkingReservation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.ParkingReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Invoice_ParkingReservationId_FK");
            });

            modelBuilder.Entity<InvoiceDetails>(entity =>
            {
                entity.HasKey(e => e.InvoiceDetailId)
                    .HasName("InvoiceDetail_pkey");

                entity.ToTable("InvoiceDetails", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_InvoiceDetails_AccountId_FK");

                entity.HasIndex(e => e.InvoiceId)
                    .HasName("fki_InvoiceDetails_InvoiceId_FK");

                entity.HasIndex(e => e.ParkingHeadRateId)
                    .HasName("fki_InvoiceDetail_ParkingHeadRate_FK");

                entity.Property(e => e.InvoiceDetailId)
                    .HasIdentityOptions(null, null, null, 99999999999999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.DiscountAmount).HasColumnType("money");

                entity.Property(e => e.DiscountType).HasMaxLength(100);

                entity.Property(e => e.Rate).HasColumnType("money");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceDetails_InvoiceId_FK");

                entity.HasOne(d => d.ParkingHeadRate)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.ParkingHeadRateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("InvoiceDetail_ParkingHeadRate_FK");
            });

            modelBuilder.Entity<LoginHistories>(entity =>
            {
                entity.HasKey(e => e.LoginHistoryId)
                    .HasName("LoginHis_Id");

                entity.ToTable("LoginHistories", schemaId);

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

            modelBuilder.Entity<MoneyTransferTypes>(entity =>
            {
                entity.HasKey(e => e.MoneyTransferTypeId)
                    .HasName("MoneyTransferTypes_pkey");

                entity.ToTable("MoneyTransferTypes", schemaId);

                entity.Property(e => e.MoneyTransferTypeId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<OtpLog>(entity =>
            {
                entity.ToTable("OtpLog", schemaId);

                entity.Property(e => e.ApiName).HasMaxLength(500);

                entity.Property(e => e.ContactNumber).HasMaxLength(30);

                entity.Property(e => e.CountryCode).HasMaxLength(10);

                entity.Property(e => e.DeviceId).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.ImeiNumber).HasMaxLength(500);

                entity.Property(e => e.Otptype)
                    .HasColumnName("OTPType")
                    .HasMaxLength(200);

                entity.Property(e => e.PassCode)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ParkingHeads>(entity =>
            {
                entity.HasKey(e => e.ParkingHeadId)
                    .HasName("ParkingHeads_pkey");

                entity.ToTable("ParkingHeads", schemaId);

                entity.HasIndex(e => e.BasisOn)
                    .HasName("fki_ParkingHeads_BasisOn_FK");

                entity.Property(e => e.ParkingHeadId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.HeadName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("% | Flat | Daily");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.BasisOnNavigation)
                    .WithMany(p => p.InverseBasisOnNavigation)
                    .HasForeignKey(d => d.BasisOn)
                    .HasConstraintName("ParkingHeads_BasisOn_FK");
            });

            modelBuilder.Entity<ParkingHeadsCustomRate>(entity =>
            {
                entity.ToTable("ParkingHeadsCustomRate", schemaId);

                entity.HasIndex(e => e.ParkingHeadId)
                    .HasName("fki_ParkingHeadsCustomRate_FK");

                entity.HasIndex(e => e.ParkingProviderLocationId)
                    .HasName("fki_ParkingHeadsCustomRate_ParkingProviderLocationId_FK");

                entity.Property(e => e.ParkingHeadsCustomRateId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.FromDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.MaxDiscountDollars).HasColumnType("numeric");

                entity.Property(e => e.MaxDiscountPercentage).HasColumnType("numeric");

                entity.Property(e => e.Rate).HasColumnType("money");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ParkingHead)
                    .WithMany(p => p.ParkingHeadsCustomRate)
                    .HasForeignKey(d => d.ParkingHeadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ParkingHeadsCustomRate_FK");

                entity.HasOne(d => d.ParkingProviderLocation)
                    .WithMany(p => p.ParkingHeadsCustomRate)
                    .HasForeignKey(d => d.ParkingProviderLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ParkingHeadsCustomRate_ParkingProviderLocationId_FK");
            });

            modelBuilder.Entity<ParkingHeadsRate>(entity =>
            {
                entity.ToTable("ParkingHeadsRate", schemaId);

                entity.HasIndex(e => e.ParkingHeadId)
                    .HasName("fki_ParkingHeadRate_FK");

                entity.HasIndex(e => e.ParkingProviderLocationId)
                    .HasName("fki_ParkingHeadRate_ParkingProviderLocationId_FK");

                entity.Property(e => e.ParkingHeadsRateId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.FromDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.MaxDiscountDollars).HasColumnType("numeric");

                entity.Property(e => e.MaxDiscountPercentage).HasColumnType("numeric");

                entity.Property(e => e.Rate).HasColumnType("money");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ParkingHead)
                    .WithMany(p => p.ParkingHeadsRate)
                    .HasForeignKey(d => d.ParkingHeadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ParkingHeadRate_FK");

                entity.HasOne(d => d.ParkingProviderLocation)
                    .WithMany(p => p.ParkingHeadsRate)
                    .HasForeignKey(d => d.ParkingProviderLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ParkingHeadRate_ParkingProviderLocationId_FK");
            });

            modelBuilder.Entity<ParkingProviders>(entity =>
            {
                entity.HasKey(e => e.ProviderId)
                    .HasName("ParkingProviders_pkey");

                entity.ToTable("ParkingProviders", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_ParkingProvider_FK");

                entity.Property(e => e.ProviderId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Address).HasMaxLength(2000);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(10);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Website).HasMaxLength(250);
            });

            modelBuilder.Entity<ParkingProvidersLocations>(entity =>
            {
                entity.HasKey(e => e.ParkingProvidersLocationId)
                    .HasName("ParkingProvidersLocations_pkey");

                entity.ToTable("ParkingProvidersLocations", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_ParkingProvidersLocations_AccountID_FK");

                entity.HasIndex(e => e.AddressId)
                    .HasName("fki_ParkingProvidersLocations_Addressid_FK");

                entity.HasIndex(e => e.ProviderId)
                    .HasName("fki_pk_ParkingProvidersLocations_providerid");

                entity.Property(e => e.ParkingProvidersLocationId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AirportToParkingEtamin).HasColumnName("AirportToParkingETAMin");

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Website).HasMaxLength(100);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.ParkingProvidersLocations)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ParkingProvidersLocations_Addressid_FK");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.ParkingProvidersLocations)
                    .HasForeignKey(d => d.ProviderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pk_ParkingProvidersLocations_providerid");
            });

            modelBuilder.Entity<ParkingProvidersLocationsSubLocations>(entity =>
            {
                entity.HasKey(e => e.ParkingProvidersLocationSubLocationId)
                    .HasName("ParkingProvidersLocationsSubLocations_pkey");

                entity.ToTable("ParkingProvidersLocationsSubLocations", schemaId);

                entity.HasIndex(e => e.ActivityCodeId)
                    .HasName("fki_FK_ActivityCodeId");

                entity.HasIndex(e => e.ParkingProviderLocationId)
                    .HasName("fki_FK_ParkingProviderLocationId");

                entity.Property(e => e.ParkingProvidersLocationSubLocationId).UseIdentityAlwaysColumn();

                entity.Property(e => e.QrcodeEncryptedValue)
                    .HasColumnName("QRCodeEncryptedValue")
                    .HasMaxLength(500);

                entity.Property(e => e.QrcodeMapping)
                    .HasColumnName("QRCodeMapping")
                    .HasMaxLength(50);

                entity.Property(e => e.QrcodePath).HasColumnName("QRCodePath");

                entity.Property(e => e.SubLocationType).HasMaxLength(50);

                entity.HasOne(d => d.ActivityCode)
                    .WithMany(p => p.ParkingProvidersLocationsSubLocations)
                    .HasForeignKey(d => d.ActivityCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivityCodeId");

                entity.HasOne(d => d.ParkingProviderLocation)
                    .WithMany(p => p.ParkingProvidersLocationsSubLocations)
                    .HasForeignKey(d => d.ParkingProviderLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingProviderLocationId");
            });

            modelBuilder.Entity<ParkingProvidersLocationsSubLocationsBkp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ParkingProvidersLocationsSubLocations_BKP", schemaId);

                entity.Property(e => e.QrcodePath).HasColumnName("QRCodePath");

                entity.Property(e => e.SubLocationType).HasMaxLength(50);
            });

            modelBuilder.Entity<ParkingProvidersLocationsSubLocationsQrBkp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ParkingProvidersLocationsSubLocations_QR_BKP", schemaId);

                entity.Property(e => e.QrcodeEncryptedValue)
                    .HasColumnName("QRCodeEncryptedValue")
                    .HasMaxLength(500);

                entity.Property(e => e.QrcodeMapping)
                    .HasColumnName("QRCodeMapping")
                    .HasMaxLength(50);

                entity.Property(e => e.QrcodePath).HasColumnName("QRCodePath");

                entity.Property(e => e.SubLocationType).HasMaxLength(50);
            });

            modelBuilder.Entity<ParkingReservation>(entity =>
            {
                entity.ToTable("ParkingReservation", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_ParkingReservation_Accountid_");

                entity.HasIndex(e => e.AgreementTemplateId)
                    .HasName("fki_AirportsParkingId_Agreementtemplate_FK");

                entity.HasIndex(e => e.AirportsParkingId)
                    .HasName("fki_ParkingReservation_AirportsParkingId_FK");

                entity.Property(e => e.ParkingReservationId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.BookingConfirmationNo).HasMaxLength(100);

                entity.Property(e => e.CheckInDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CheckOutDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Comment).HasMaxLength(500);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EndDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Source).HasMaxLength(5);

                entity.Property(e => e.StartDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ValletLocation).HasMaxLength(100);

                entity.Property(e => e.VehicleKeyLocation).HasMaxLength(100);

                entity.Property(e => e.VehicleLocation).HasMaxLength(100);

                entity.HasOne(d => d.AgreementTemplate)
                    .WithMany(p => p.ParkingReservation)
                    .HasForeignKey(d => d.AgreementTemplateId)
                    .HasConstraintName("AirportsParkingId_Agreementtemplate_FK");

                entity.HasOne(d => d.AirportsParking)
                    .WithMany(p => p.ParkingReservation)
                    .HasForeignKey(d => d.AirportsParkingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ParkingReservation_AirportsParkingId_FK");

                entity.HasOne(d => d.ParkingProvidersLocation)
                    .WithMany(p => p.ParkingReservation)
                    .HasForeignKey(d => d.ParkingProvidersLocationId)
                    .HasConstraintName("ParkingReservation_ParkingProvidersLocationId_FK");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.ParkingReservation)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("Fk_ReservationId");
            });

            modelBuilder.Entity<ParkingSpotType>(entity =>
            {
                entity.ToTable("ParkingSpotType", schemaId);

                entity.Property(e => e.ParkingSpotTypeId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<ParkingSpots>(entity =>
            {
                entity.HasKey(e => e.ParkingSpotId)
                    .HasName("ParkingSpotId_pkey");

                entity.ToTable("ParkingSpots", schemaId);

                entity.HasIndex(e => e.ParkingProvidersLocationSubLocationId)
                    .HasName("fki_Parkingspot_ParkingProvidersLocationId_FK");

                entity.HasIndex(e => e.ParkingSpotTypeId)
                    .HasName("fki_Parkingspot_ParkingSpotType_FK");

                entity.Property(e => e.ParkingSpotId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ParkingProvidersLocationSubLocation)
                    .WithMany(p => p.ParkingSpots)
                    .HasForeignKey(d => d.ParkingProvidersLocationSubLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Parkingspot_ParkingProvidersLocationSubLocationId_FK");

                entity.HasOne(d => d.ParkingSpotType)
                    .WithMany(p => p.ParkingSpots)
                    .HasForeignKey(d => d.ParkingSpotTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Parkingspot_ParkingSpotType_FK");
            });

            modelBuilder.Entity<PasswordHistories>(entity =>
            {
                entity.HasKey(e => e.PwdHstId)
                    .HasName("PasswordHistory_pkey");

                entity.ToTable("PasswordHistories", schemaId);

                entity.Property(e => e.PwdHstId).UseIdentityAlwaysColumn();

                entity.Property(e => e.LastUsedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();
            });

            modelBuilder.Entity<PasswordPolicies>(entity =>
            {
                entity.HasKey(e => e.PasswordPolicyId)
                    .HasName("PasswordPolicy_pkey");

                entity.ToTable("PasswordPolicies", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("Uq_PasswordPlicyAccountID")
                    .IsUnique();

                entity.Property(e => e.PasswordPolicyId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<PaymentDetails>(entity =>
            {
                entity.HasKey(e => e.PaymentDetailId)
                    .HasName("PaymentDetails_pkey");

                entity.ToTable("PaymentDetails", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_PaymentDetails_AccountId_FK");

                entity.HasIndex(e => e.ReservationId)
                    .HasName("fki_PaymentDetails_ReservationId_FK");

                entity.Property(e => e.PaymentDetailId)
                    .HasIdentityOptions(null, null, null, 999999999999999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ConnectedAccountId).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CustomerId).HasMaxLength(100);

                entity.Property(e => e.PaymentDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PaymentIntentId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PaymentMethodId).HasMaxLength(100);

                entity.Property(e => e.ReceiptEmail).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.PaymentDetails)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PaymentDetails_ReservationId_FK");
            });

            modelBuilder.Entity<RefreshTokens>(entity =>
            {
                entity.HasKey(e => e.RefreshTokenId)
                    .HasName("RefreshTokens_pkey");

                entity.ToTable("RefreshTokens", schemaId);

                entity.Property(e => e.RefreshTokenId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ExpiryDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Token).IsRequired();
            });

            modelBuilder.Entity<RentalHeadRates>(entity =>
            {
                entity.HasKey(e => e.RentalHeadRateId)
                    .HasName("RentalHeadRatesIdPK");

                entity.ToTable("RentalHeadRates", schemaId);

                entity.HasIndex(e => e.RentalHeadId)
                    .HasName("fki_RentalHeadRate_fk");

                entity.Property(e => e.RentalHeadRateId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.From).HasColumnType("timestamp with time zone");

                entity.Property(e => e.To).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.RentalHead)
                    .WithMany(p => p.RentalHeadRates)
                    .HasForeignKey(d => d.RentalHeadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RentalHeadRates_fk");
            });

            modelBuilder.Entity<RentalHeads>(entity =>
            {
                entity.HasKey(e => e.RentalHeadId)
                    .HasName("RentalHeads_pkey");

                entity.ToTable("RentalHeads", schemaId);

                entity.Property(e => e.RentalHeadId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OptionName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RateType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<RentalRate>(entity =>
            {
                entity.ToTable("RentalRate", schemaId);

                entity.HasIndex(e => e.ParkingProvidersLocationId)
                    .HasName("fki_RentalRate_fk");

                entity.Property(e => e.RentalRateId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CarType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.From).HasColumnType("timestamp with time zone");

                entity.Property(e => e.RentalRate1).HasColumnName("RentalRate");

                entity.Property(e => e.To).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<RentalReservation>(entity =>
            {
                entity.ToTable("RentalReservation", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_RentalReservation_Accountid_FK");

                entity.HasIndex(e => e.AgreementTemplateId)
                    .HasName("fki_RentalReservation_AgreementTemplateId_FK");

                entity.HasIndex(e => e.ReservationId)
                    .HasName("fki_RentalReservation_ReservationId_FK");

                entity.HasIndex(e => e.VehicleCategoryId)
                    .HasName("fki_V");

                entity.Property(e => e.RentalReservationId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ActualEnd).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ActualStart).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DropOffCity)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DropOffDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PickupCity)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PickupDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.RentalReservationCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ThirdPartyConfirmationNumber).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.AgreementTemplate)
                    .WithMany(p => p.RentalReservation)
                    .HasForeignKey(d => d.AgreementTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RentalReservation_AgreementTemplateId_FK");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.RentalReservation)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RentalReservation_ReservationId_FK");

                entity.HasOne(d => d.VehicleCategory)
                    .WithMany(p => p.RentalReservation)
                    .HasForeignKey(d => d.VehicleCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReservationId_Vehiclecat_FK");
            });

            modelBuilder.Entity<RentalReservationCarFeatures>(entity =>
            {
                entity.ToTable("RentalReservationCarFeatures", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_RentalReservationCarFeatures_Accountid_");

                entity.HasIndex(e => e.RentalReservationId)
                    .HasName("fki_fk_RentalReservationCarFeatures_RentalReservationId");

                entity.HasIndex(e => e.VehicleFeatureId)
                    .HasName("fki_RentalReservationCarFeatures_VehicleFeature_FK");

                entity.Property(e => e.RentalReservationCarFeaturesId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.RentalReservation)
                    .WithMany(p => p.RentalReservationCarFeatures)
                    .HasForeignKey(d => d.RentalReservationId)
                    .HasConstraintName("fk_RentalReservationCarFeatures_RentalReservationId");

                entity.HasOne(d => d.VehicleFeature)
                    .WithMany(p => p.RentalReservationCarFeatures)
                    .HasForeignKey(d => d.VehicleFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RentalReservationCarFeatures_VehicleFeature_FK");
            });

            modelBuilder.Entity<RentalReservationOption>(entity =>
            {
                entity.ToTable("RentalReservationOption", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_RentalReservationOption_AccountId_FK");

                entity.HasIndex(e => e.RentalHeadId)
                    .HasName("fki_RentalReservationOption_RentalHead_FK");

                entity.HasIndex(e => e.ReservationId)
                    .HasName("fki_RentalReservationOption_Reservation_");

                entity.Property(e => e.RentalReservationOptionId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.StartDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.RentalHead)
                    .WithMany(p => p.RentalReservationOption)
                    .HasForeignKey(d => d.RentalHeadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RentalReservationOption_RentalHead_FK");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.RentalReservationOption)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RentalReservationOption_Reservation_");
            });

            modelBuilder.Entity<RequestLogs>(entity =>
            {
                entity.HasKey(e => e.RequestLogId)
                    .HasName("RequestLog_pkey");

                entity.ToTable("RequestLogs", schemaId);

                entity.Property(e => e.Apiname)
                    .IsRequired()
                    .HasColumnName("APIName")
                    .HasMaxLength(250);

                entity.Property(e => e.AppVersion).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeviceId).HasMaxLength(500);

                entity.Property(e => e.DeviceManufacture).HasMaxLength(500);

                entity.Property(e => e.DeviceModel).HasMaxLength(500);

                entity.Property(e => e.DeviceOs)
                    .HasColumnName("DeviceOS")
                    .HasMaxLength(500);

                entity.Property(e => e.DeviceOsversion)
                    .HasColumnName("DeviceOSVersion")
                    .HasMaxLength(500);

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.RequestedFrom)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UserAgent).HasMaxLength(500);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_Reservation_Accountid_FK");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_Reservation_Userid_FK");

                entity.Property(e => e.ReservationId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.IsCancelled).HasDefaultValueSql("false");

                entity.Property(e => e.IsChanged).HasDefaultValueSql("false");

                entity.Property(e => e.ReservationCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Reservation_Userid_FK");
            });

            modelBuilder.Entity<ReservationActivityCode>(entity =>
            {
                entity.ToTable("ReservationActivityCode", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_FK_ReservationActivityCode_AccountId");

                entity.HasIndex(e => e.ReservationActivityCodeId)
                    .HasName("fki_FK_ReservationActivityCode_ActivityCodeId");

                entity.Property(e => e.ReservationActivityCodeId)
                    .HasIdentityOptions(null, null, null, 99999999999999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ActivityDoneBy).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ActivityCode)
                    .WithMany(p => p.ReservationActivityCode)
                    .HasForeignKey(d => d.ActivityCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReservationActivityCode_ActivityCodeId");
            });

            modelBuilder.Entity<ReservationPaymentHistory>(entity =>
            {
                entity.ToTable("ReservationPaymentHistory", schemaId);

                entity.Property(e => e.ReservationPaymentHistoryId).UseIdentityAlwaysColumn();

                entity.Property(e => e.BaseRate).HasColumnType("money");

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.IsCustomRate).HasDefaultValueSql("false");

                entity.Property(e => e.IsSystemRate).HasDefaultValueSql("false");

                entity.Property(e => e.PaymentMode)
                    .HasMaxLength(100)
                    .HasComment("Online, Cash, POS");

                entity.Property(e => e.PaymentOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(100)
                    .HasComment("Prepaid, Outstanding, Due after expected checkout date");

                entity.Property(e => e.SourceId).HasComment("Billing Party");

                entity.Property(e => e.Tax).HasColumnType("money");

                entity.Property(e => e.TotalAmout).HasColumnType("money");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<ReservationType>(entity =>
            {
                entity.ToTable("ReservationType", schemaId);

                entity.Property(e => e.ReservationTypeId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<ReservationVehicle>(entity =>
            {
                entity.ToTable("ReservationVehicle", schemaId);

                entity.HasIndex(e => e.ParkingProvidersLocationId)
                    .HasName("fki_fk_reservationVehicle_ParkingProvidersLocationId");

                entity.HasIndex(e => e.ReservationId)
                    .HasName("fki_reservationvehicle_Reservationid_FK");

                entity.HasIndex(e => e.VehicleId)
                    .HasName("fki_ReservationVehicle_vehicleid_FK");

                entity.Property(e => e.ReservationVehicleId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ParkingProvidersLocation)
                    .WithMany(p => p.ReservationVehicle)
                    .HasForeignKey(d => d.ParkingProvidersLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_reservationVehicle_ParkingProvidersLocationId");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.ReservationVehicle)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reservationvehicle_Reservationid_FK");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.ReservationVehicle)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReservationVehicle_vehicleid_FK");
            });

            modelBuilder.Entity<ScreenFields>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ScreenFields", schemaId);

                entity.HasComment("This table contains details of fields that are displayed in Screen");

                entity.Property(e => e.ColumnWidthPer).HasComment("Value represents width of the field in a respective screen");

                entity.Property(e => e.DatasourceDisplayType).HasComment("1=Value Only, 2 = Text Only, 3 = Both Value and Text");

                entity.Property(e => e.DisplayMaxLength)
                    .HasMaxLength(50)
                    .HasComment("Value represents maximum length of the field in a respective screen");

                entity.Property(e => e.DisplayOrder).HasComment("Represents order value of the fields in a respective screen");

                entity.Property(e => e.FieldId).HasComment("Id of the field");

                entity.Property(e => e.ScreenId).HasComment("Id of the screen (Screens.ScreenId)");

                entity.Property(e => e.UseShortLabel).HasComment("Boolean value represents if short label value to use or not");
            });

            modelBuilder.Entity<Screens>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Screens", schemaId);

                entity.Property(e => e.ScreenId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ScreenName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Shuttle>(entity =>
            {
                entity.ToTable("Shuttle", schemaId);

                entity.HasIndex(e => e.ParkingProvidersLocationId)
                    .HasName("fki_Shuttle_FK");

                entity.HasIndex(e => e.VehicleId)
                    .HasName("fki_Shuttle_VehicleId_FK");

                entity.Property(e => e.ShuttleId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ShuttleNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Shuttle)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Shuttle_VehicleId_FK");
            });

            modelBuilder.Entity<Source>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Source", schemaId);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SourceId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("State", schemaId);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<SubLocationType>(entity =>
            {
                entity.ToTable("SubLocationType", schemaId);

                entity.Property(e => e.SubLocationTypeId).UseIdentityAlwaysColumn();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TripPaxAndBags>(entity =>
            {
                entity.ToTable("TripPaxAndBags", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_ReservationVehicle");

                entity.HasIndex(e => e.ActivityCodeId)
                    .HasName("fki_TripPaxAndBags_Activitycode_FK");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_TripPaxAndBags_Userid_FK");

                entity.Property(e => e.TripPaxAndBagsId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ActivityCode)
                    .WithMany(p => p.TripPaxAndBags)
                    .HasForeignKey(d => d.ActivityCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TripPaxAndBags_Activitycode_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TripPaxAndBags)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TripPaxAndBags_Userid_FK");
            });

            modelBuilder.Entity<UserBankAccounts>(entity =>
            {
                entity.HasKey(e => e.UserBankAccountId)
                    .HasName("UserBankAccounts_pkey");

                entity.ToTable("UserBankAccounts", schemaId);

                entity.HasIndex(e => e.MoneyTransferTypeId)
                    .HasName("fki_UserBankAccounts_MoneyTransfer_FK");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_C");

                entity.Property(e => e.UserBankAccountId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.BankAccountNumber)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.RoutingNumber)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.MoneyTransferType)
                    .WithMany(p => p.UserBankAccounts)
                    .HasForeignKey(d => d.MoneyTransferTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserBankAccounts_MoneyTransfer_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBankAccounts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserBankAccounts_Userid_FK");
            });

            modelBuilder.Entity<UserCreditCardsWillbedelete>(entity =>
            {
                entity.HasKey(e => e.UserCreditCardId)
                    .HasName("UserCreditCards_pkey");

                entity.ToTable("UserCreditCards_Willbedelete", schemaId);

                entity.Property(e => e.UserCreditCardId).ValueGeneratedNever();

                entity.Property(e => e.BillingAddress1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Ccvcode)
                    .IsRequired()
                    .HasColumnName("CCVCode")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ExpirationMonth)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ExpirationYear)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<UserDeviceHistory>(entity =>
            {
                entity.ToTable("UserDeviceHistory", schemaId);

                entity.Property(e => e.AppVersion).HasMaxLength(250);

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DeviceManufacture).HasMaxLength(250);

                entity.Property(e => e.DeviceModel).HasMaxLength(250);

                entity.Property(e => e.DeviceOsversion)
                    .HasColumnName("DeviceOSVersion")
                    .HasMaxLength(250);

                entity.Property(e => e.DeviceToken).HasMaxLength(500);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDeviceHistory)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDeviceHistory_Users");
            });

            modelBuilder.Entity<UserDrivingLicense>(entity =>
            {
                entity.ToTable("UserDrivingLicense", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_UserDrivingLicense_Accountid_FK");

                entity.HasIndex(e => e.AddressId)
                    .HasName("fki_UserDrivingLicense_Address_FK");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_UserDrivingLicense_Userid_FK");

                entity.Property(e => e.UserDrivingLicenseId)
                    .HasIdentityOptions(null, null, null, 99999999999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DateOfBirth).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DateOfExpiration).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DateOfIssue).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DrivingLicenseNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EyeColor)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Hight)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.UserDrivingLicense)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserDrivingLicense_Address_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDrivingLicense)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserDrivingLicense_Userid_FK");
            });

            modelBuilder.Entity<UserGroupMappings>(entity =>
            {
                entity.HasKey(e => e.UserGroupMappingId)
                    .HasName("UsersGroupsMapping_pkey");

                entity.ToTable("UserGroupMappings", schemaId);

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

                entity.ToTable("UserRights", schemaId);

                entity.Property(e => e.UserRightId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRights)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsersRights__Users__UserId");
            });

            modelBuilder.Entity<UserVehiclePreferenceCategory>(entity =>
            {
                entity.ToTable("UserVehiclePreferenceCategory", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_FK_UserVehiclePreCategory_AccountId");

                entity.Property(e => e.UserVehiclePreferenceCategoryId).UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<UserVehiclePreferenceFeatures>(entity =>
            {
                entity.ToTable("UserVehiclePreferenceFeatures", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_UserVihiclePreFeatures_AccountId");

                entity.Property(e => e.UserVehiclePreferenceFeaturesId).UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<UserVehicles>(entity =>
            {
                entity.HasKey(e => e.UserVehicleId)
                    .HasName("UserVehicles_pkey");

                entity.ToTable("UserVehicles", schemaId);

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_UserVehicles_Userid_FK");

                entity.HasIndex(e => e.VehicleId)
                    .HasName("fki_UserVehicles_Vehicleid_FK");

                entity.Property(e => e.UserVehicleId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserVehicles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserVehicles_Userid_FK");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.UserVehicles)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserVehicles_Vehicleid_FK");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("Users_pkey");

                entity.ToTable("Users", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_Users_FK");

                entity.HasIndex(e => e.ParkingProvidersLocationId)
                    .HasName("fki_User_ParkingProvidersLocationId_FK");

                entity.HasIndex(e => new { e.AccountId, e.UserName })
                    .HasName("username_unique")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AuthenticationCategory).HasMaxLength(10);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DateOfBirth).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DeviceId).HasMaxLength(500);

                entity.Property(e => e.DrivingLicense).HasMaxLength(100);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ExternalUserId).HasMaxLength(500);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.ImagePath).HasMaxLength(4000);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Mobile).HasMaxLength(13);

                entity.Property(e => e.MobileCode).HasMaxLength(4);

                entity.Property(e => e.PasswordExpirationDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.StripeCustomerId).HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.ParkingProvidersLocation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ParkingProvidersLocationId)
                    .HasConstraintName("User_ParkingProvidersLocationId_FK");
            });

            modelBuilder.Entity<UsersPaymentMethod>(entity =>
            {
                entity.ToTable("UsersPaymentMethod", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_FK_AccountId");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_FK_UserId");

                entity.Property(e => e.UsersPaymentMethodId).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PaymentMethodId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.StripeCustomerId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersPaymentMethod)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserId");
            });

            modelBuilder.Entity<ValidationTypes>(entity =>
            {
                entity.HasKey(e => e.ValidationTypeId)
                    .HasName("ValidationTypes_pkey");

                entity.ToTable("ValidationTypes", schemaId);

                entity.Property(e => e.ValidationTypeId)
                    .HasIdentityOptions(null, null, null, 32767L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ValidationRule).HasMaxLength(50);

                entity.Property(e => e.ValidationType)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<VehicleAvailablity>(entity =>
            {
                entity.ToTable("VehicleAvailablity", schemaId);

                entity.HasIndex(e => e.ReservationVehicleId)
                    .HasName("fki_VehicleAvailablity_Reservationvehicleid_FK");

                entity.Property(e => e.VehicleAvailablityId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.FromDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ToDateTime).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ReservationVehicle)
                    .WithMany(p => p.VehicleAvailablity)
                    .HasForeignKey(d => d.ReservationVehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VehicleAvailablity_Reservationvehicleid_FK");
            });

            modelBuilder.Entity<VehicleCategory>(entity =>
            {
                entity.ToTable("VehicleCategory", schemaId);

                entity.Property(e => e.VehicleCategoryId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<VehicleFeatures>(entity =>
            {
                entity.HasKey(e => e.VehicleFeatureId)
                    .HasName("VehicleFeatures_pkey");

                entity.ToTable("VehicleFeatures", schemaId);

                entity.Property(e => e.VehicleFeatureId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Icon).HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<VehicleFeaturesMapping>(entity =>
            {
                entity.ToTable("VehicleFeaturesMapping", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_VehicleFeaturesMapping_Accountid_FK");

                entity.HasIndex(e => e.VehicleFeatureId)
                    .HasName("fki_VehicleFeaturesMapping_Featureid_FK");

                entity.HasIndex(e => e.VehicleId)
                    .HasName("fki_VehicleFeaturesMapping_VehicleID_FK");

                entity.Property(e => e.VehicleFeaturesMappingId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.HasOne(d => d.VehicleFeature)
                    .WithMany(p => p.VehicleFeaturesMapping)
                    .HasForeignKey(d => d.VehicleFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VehicleFeaturesMapping_Featureid_FK");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleFeaturesMapping)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VehicleFeaturesMapping_VehicleID_FK");
            });

            modelBuilder.Entity<VehicleTagMapping>(entity =>
            {
                entity.ToTable("VehicleTagMapping", schemaId);

                entity.Property(e => e.TagId)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleTagMapping)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VehicleTagMapping_VehicleID_FK");
            });

            modelBuilder.Entity<Vehicles>(entity =>
            {
                entity.HasKey(e => e.VehicleId)
                    .HasName("Vehicles_pkey");

                entity.ToTable("Vehicles", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_Vehicles_AccountId_FK");

                entity.HasIndex(e => e.VehicleCategoryId)
                    .HasName("fki_Vehicles_VehicleCategoryId_FK");

                entity.Property(e => e.VehicleId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.LicensePlate).HasMaxLength(100);

                entity.Property(e => e.Logo).HasMaxLength(500);

                entity.Property(e => e.Make).HasColumnType("character varying");

                entity.Property(e => e.Model).HasColumnType("character varying");

                entity.Property(e => e.RegistrationState).HasColumnType("character varying");

                entity.Property(e => e.Ticket).HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Vinnumber)
                    .HasColumnName("VINNumber")
                    .HasMaxLength(17);

                entity.Property(e => e.Year).HasColumnType("character varying");

                entity.HasOne(d => d.VehicleCategory)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.VehicleCategoryId)
                    .HasConstraintName("Vehicles_VehicleCategoryId_FK");
            });

            modelBuilder.Entity<VehiclesEventLog>(entity =>
            {
                entity.ToTable("VehiclesEventLog", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_Vehicleeventlog_Accountid_FK");

                entity.HasIndex(e => e.VehicleId)
                    .HasName("fki_Vehicleeventlog_VehicleId_FK");

                entity.Property(e => e.VehiclesEventLogId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehiclesEventLog)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Vehicleeventlog_VehicleId_FK");
            });

            modelBuilder.Entity<VehiclesMediaStorage>(entity =>
            {
                entity.ToTable("VehiclesMediaStorage", schemaId);

                entity.HasIndex(e => e.AccountId)
                    .HasName("fki_VehiclesMediaStorage_Accountid_FK");

                entity.HasIndex(e => e.ActivityCodeId)
                    .HasName("fki_p");

                entity.HasIndex(e => e.VehiclesMediaTypeId)
                    .HasName("fki_VehiclesMediaStorage_Mediatype_fk");

                entity.Property(e => e.VehiclesMediaStorageId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EntryTimeStamp).HasColumnType("timestamp with time zone");

                entity.Property(e => e.MediaPath)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ActivityCode)
                    .WithMany(p => p.VehiclesMediaStorage)
                    .HasForeignKey(d => d.ActivityCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_vehiclesMediaStorage_activityCodeId");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehiclesMediaStorage)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VehiclesMediaStorage_Vehicleid_FK");

                entity.HasOne(d => d.VehiclesMediaType)
                    .WithMany(p => p.VehiclesMediaStorage)
                    .HasForeignKey(d => d.VehiclesMediaTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VehiclesMediaStorage_Mediatype_fk");
            });

            modelBuilder.Entity<VehiclesMediaType>(entity =>
            {
                entity.ToTable("VehiclesMediaType", schemaId);

                entity.Property(e => e.VehiclesMediaTypeId)
                    .HasIdentityOptions(null, null, null, 9999999L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedOn).HasColumnType("timestamp with time zone");

                entity.Property(e => e.MediaType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("timestamp with time zone");
            });

            modelBuilder.HasSequence("EmailParserDetail_EmailParserDetailId_seq", schemaId).HasMax(99999999999);

            modelBuilder.HasSequence("ReservationActivityCode_ReservationActivityCodeId_seq", schemaId).HasMax(99999999999999999);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
