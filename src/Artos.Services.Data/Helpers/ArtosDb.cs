using Artos.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artos.Services.Data.Helpers
{
    public class ArtosDB : DbContext
    {
        public ArtosDB(DbContextOptions<ArtosDB> options) : base(options)
        { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserCard> UserCards { get; set; }
        public DbSet<EMoney> EMoneys { get; set; }
        public DbSet<ArtosTransaction> ArtosTransactions { get; set; }
        public DbSet<ArtosTransactionDetail> ArtosTransactionDetails { get; set; }
        public DbSet<LanguageResource> LanguageResources { get; set; }
        public DbSet<StaticContent> StaticContents { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<TicketPool> TicketPools { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<CardTransaction> CardTransactions { get; set; }
        public DbSet<CardTapHistory> CardTapHistorys { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<ContactCenter> ContactCenters { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<GlobalConfig> GlobalConfigs { get; set; }
        public DbSet<DeviceSyncLog> DeviceSyncLogs { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);

            // shadow properties
            builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");
            */
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            /*
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SourceInfo>();
            updateUpdatedProperty<DataEventRecord>();
            */
            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            /*
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
            */
        }

        public DbSet<Artos.Entities.Route> Route { get; set; }
    }
}
