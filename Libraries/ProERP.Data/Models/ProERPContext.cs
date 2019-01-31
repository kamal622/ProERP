using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ProERP.Data.Models.Mapping;

namespace ProERP.Data.Models
{
    public abstract class ProERPContext : DbContext
    {
        static ProERPContext()
        {
            Database.SetInitializer<ProERPContext>(null);
        }

        public ProERPContext(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public DbSet<BreakDown> BreakDowns { get; set; }
        public DbSet<BreakDownType> BreakDownTypes { get; set; }
        public DbSet<Indent> Indents { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineType> MachineTypes { get; set; }
        public DbSet<MaintenancePriorityType> MaintenancePriorityTypes { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<MaintenanceUserAssignment> MaintenanceUserAssignments { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<PreventiveHoldHistory> PreventiveHoldHistories { get; set; }
        public DbSet<PreventiveMaintenance> PreventiveMaintenances { get; set; }
        public DbSet<PreventiveReviewHistory> PreventiveReviewHistories { get; set; }
        public DbSet<PreventiveScheduleType> PreventiveScheduleTypes { get; set; }
        public DbSet<PreventiveWorkDescription> PreventiveWorkDescriptions { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SpareType> SpareTypes { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<SubAssembly> SubAssemblies { get; set; }
        public DbSet<UserAssignment> UserAssignments { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VendorCategory> VendorCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BreakDownMap());
            modelBuilder.Configurations.Add(new BreakDownTypeMap());
            modelBuilder.Configurations.Add(new IndentMap());
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new LineMap());
            modelBuilder.Configurations.Add(new MachineMap());
            modelBuilder.Configurations.Add(new MachineTypeMap());
            modelBuilder.Configurations.Add(new MaintenancePriorityTypeMap());
            modelBuilder.Configurations.Add(new MaintenanceRequestMap());
            modelBuilder.Configurations.Add(new MaintenanceUserAssignmentMap());
            modelBuilder.Configurations.Add(new PartMap());
            modelBuilder.Configurations.Add(new PlantMap());
            modelBuilder.Configurations.Add(new PreventiveHoldHistoryMap());
            modelBuilder.Configurations.Add(new PreventiveMaintenanceMap());
            modelBuilder.Configurations.Add(new PreventiveReviewHistoryMap());
            modelBuilder.Configurations.Add(new PreventiveScheduleTypeMap());
            modelBuilder.Configurations.Add(new PreventiveWorkDescriptionMap());
            modelBuilder.Configurations.Add(new PurchaseMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new SectionMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new SiteMap());
            modelBuilder.Configurations.Add(new SpareTypeMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new SubAssemblyMap());
            modelBuilder.Configurations.Add(new UserAssignmentMap());
            modelBuilder.Configurations.Add(new UserClaimMap());
            modelBuilder.Configurations.Add(new UserLoginMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new VendorCategoryMap());
            modelBuilder.Configurations.Add(new VendorMap());
        }
    }
}
