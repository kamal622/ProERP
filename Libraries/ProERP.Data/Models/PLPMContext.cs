using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ProERP.Data.Models.Mapping;

namespace ProERP.Data.Models
{
    public abstract class PLPMContext : DbContext
    {
        static PLPMContext()
        {
            Database.SetInitializer<PLPMContext>(null);
        }

        public PLPMContext(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public DbSet<BreakDown> BreakDowns { get; set; }
        public DbSet<BreakDownAttachment> BreakDownAttachments { get; set; }
        public DbSet<BreakDownMenPower> BreakDownMenPowers { get; set; }
        public DbSet<BreakDownService> BreakDownServices { get; set; }
        public DbSet<BreakDownSpare> BreakDownSpares { get; set; }
        public DbSet<BreakDownType> BreakDownTypes { get; set; }
        public DbSet<BreakDownUploadHistory> BreakDownUploadHistories { get; set; }
        public DbSet<ColourSpecification> ColourSpecifications { get; set; }
        public DbSet<CurrencyMaster> CurrencyMasters { get; set; }
        public DbSet<DailyPackingDetail> DailyPackingDetails { get; set; }
        public DbSet<DocumentAction> DocumentActions { get; set; }
        public DbSet<DocumentHistory> DocumentHistories { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<FormulationChangedHistory> FormulationChangedHistories { get; set; }
        public DbSet<FormulationMaster> FormulationMasters { get; set; }
        public DbSet<FormulationRequestClose> FormulationRequestCloses { get; set; }
        public DbSet<FormulationRequest> FormulationRequests { get; set; }
        public DbSet<FormulationRequestsChangeHistory> FormulationRequestsChangeHistories { get; set; }
        public DbSet<FormulationRequestsDetail> FormulationRequestsDetails { get; set; }
        public DbSet<FormulationRequestsStatu> FormulationRequestsStatus { get; set; }
        public DbSet<IndentBudget> IndentBudgets { get; set; }
        public DbSet<IndentDetail> IndentDetails { get; set; }
        public DbSet<IndentDetailAttachment> IndentDetailAttachments { get; set; }
        public DbSet<Indent> Indents { get; set; }
        public DbSet<IndentStatu> IndentStatus { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<LineMachineActiveHistory> LineMachineActiveHistories { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineReading> MachineReadings { get; set; }
        public DbSet<MachineType> MachineTypes { get; set; }
        public DbSet<MaintanceRequestStatu> MaintanceRequestStatus { get; set; }
        public DbSet<MaintenancePriorityType> MaintenancePriorityTypes { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<MaintenanceUserAssignment> MaintenanceUserAssignments { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<PlantBudget> PlantBudgets { get; set; }
        public DbSet<PLMMVersion> PLMMVersions { get; set; }
        public DbSet<PreventiveHoldHistory> PreventiveHoldHistories { get; set; }
        public DbSet<PreventiveMaintenance> PreventiveMaintenances { get; set; }
        public DbSet<PreventiveReviewHistory> PreventiveReviewHistories { get; set; }
        public DbSet<PreventiveScheduleType> PreventiveScheduleTypes { get; set; }
        public DbSet<PreventiveWorkDescription> PreventiveWorkDescriptions { get; set; }
        public DbSet<ProcessLogSheet1> ProcessLogSheet1 { get; set; }
        public DbSet<ProcessLogSheet2> ProcessLogSheet2 { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductMaster> ProductMasters { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<QASpecification> QASpecifications { get; set; }
        public DbSet<QAStatu> QAStatus { get; set; }
        public DbSet<QOSLine> QOSLines { get; set; }
        public DbSet<ReadingData> ReadingDatas { get; set; }
        public DbSet<RMItem> RMItems { get; set; }
        public DbSet<RMRequestDetail> RMRequestDetails { get; set; }
        public DbSet<RMRequestMaster> RMRequestMasters { get; set; }
        public DbSet<RMRequestStatu> RMRequestStatus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Sequence> Sequences { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ShutdownHistory> ShutdownHistories { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SpareType> SpareTypes { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<SubAssembly> SubAssemblies { get; set; }
        public DbSet<TemplateFixColumn> TemplateFixColumns { get; set; }
        public DbSet<TemplateMapping> TemplateMappings { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<UserAssignment> UserAssignments { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VendorCategory> VendorCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VersionNote> VersionNotes { get; set; }
        public DbSet<WIPStore> WIPStores { get; set; }
        public DbSet<WIPStoreItem> WIPStoreItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BreakDownMap());
            modelBuilder.Configurations.Add(new BreakDownAttachmentMap());
            modelBuilder.Configurations.Add(new BreakDownMenPowerMap());
            modelBuilder.Configurations.Add(new BreakDownServiceMap());
            modelBuilder.Configurations.Add(new BreakDownSpareMap());
            modelBuilder.Configurations.Add(new BreakDownTypeMap());
            modelBuilder.Configurations.Add(new BreakDownUploadHistoryMap());
            modelBuilder.Configurations.Add(new ColourSpecificationMap());
            modelBuilder.Configurations.Add(new CurrencyMasterMap());
            modelBuilder.Configurations.Add(new DailyPackingDetailMap());
            modelBuilder.Configurations.Add(new DocumentActionMap());
            modelBuilder.Configurations.Add(new DocumentHistoryMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new DocumentTypeMap());
            modelBuilder.Configurations.Add(new EmployeeTypeMap());
            modelBuilder.Configurations.Add(new FormulationChangedHistoryMap());
            modelBuilder.Configurations.Add(new FormulationMasterMap());
            modelBuilder.Configurations.Add(new FormulationRequestCloseMap());
            modelBuilder.Configurations.Add(new FormulationRequestMap());
            modelBuilder.Configurations.Add(new FormulationRequestsChangeHistoryMap());
            modelBuilder.Configurations.Add(new FormulationRequestsDetailMap());
            modelBuilder.Configurations.Add(new FormulationRequestsStatuMap());
            modelBuilder.Configurations.Add(new IndentBudgetMap());
            modelBuilder.Configurations.Add(new IndentDetailMap());
            modelBuilder.Configurations.Add(new IndentDetailAttachmentMap());
            modelBuilder.Configurations.Add(new IndentMap());
            modelBuilder.Configurations.Add(new IndentStatuMap());
            modelBuilder.Configurations.Add(new ItemCategoryMap());
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new LineMap());
            modelBuilder.Configurations.Add(new LineMachineActiveHistoryMap());
            modelBuilder.Configurations.Add(new MachineMap());
            modelBuilder.Configurations.Add(new MachineReadingMap());
            modelBuilder.Configurations.Add(new MachineTypeMap());
            modelBuilder.Configurations.Add(new MaintanceRequestStatuMap());
            modelBuilder.Configurations.Add(new MaintenancePriorityTypeMap());
            modelBuilder.Configurations.Add(new MaintenanceRequestMap());
            modelBuilder.Configurations.Add(new MaintenanceUserAssignmentMap());
            modelBuilder.Configurations.Add(new PartMap());
            modelBuilder.Configurations.Add(new PlantMap());
            modelBuilder.Configurations.Add(new PlantBudgetMap());
            modelBuilder.Configurations.Add(new PLMMVersionMap());
            modelBuilder.Configurations.Add(new PreventiveHoldHistoryMap());
            modelBuilder.Configurations.Add(new PreventiveMaintenanceMap());
            modelBuilder.Configurations.Add(new PreventiveReviewHistoryMap());
            modelBuilder.Configurations.Add(new PreventiveScheduleTypeMap());
            modelBuilder.Configurations.Add(new PreventiveWorkDescriptionMap());
            modelBuilder.Configurations.Add(new ProcessLogSheet1Map());
            modelBuilder.Configurations.Add(new ProcessLogSheet2Map());
            modelBuilder.Configurations.Add(new ProductCategoryMap());
            modelBuilder.Configurations.Add(new ProductMasterMap());
            modelBuilder.Configurations.Add(new PurchaseMap());
            modelBuilder.Configurations.Add(new QASpecificationMap());
            modelBuilder.Configurations.Add(new QAStatuMap());
            modelBuilder.Configurations.Add(new QOSLineMap());
            modelBuilder.Configurations.Add(new ReadingDataMap());
            modelBuilder.Configurations.Add(new RMItemMap());
            modelBuilder.Configurations.Add(new RMRequestDetailMap());
            modelBuilder.Configurations.Add(new RMRequestMasterMap());
            modelBuilder.Configurations.Add(new RMRequestStatuMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new SectionMap());
            modelBuilder.Configurations.Add(new SequenceMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new ShutdownHistoryMap());
            modelBuilder.Configurations.Add(new SiteMap());
            modelBuilder.Configurations.Add(new SpareTypeMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new SubAssemblyMap());
            modelBuilder.Configurations.Add(new TemplateFixColumnMap());
            modelBuilder.Configurations.Add(new TemplateMappingMap());
            modelBuilder.Configurations.Add(new TemplateMap());
            modelBuilder.Configurations.Add(new UserAssignmentMap());
            modelBuilder.Configurations.Add(new UserClaimMap());
            modelBuilder.Configurations.Add(new UserLoginMap());
            modelBuilder.Configurations.Add(new UserProfileMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new VendorCategoryMap());
            modelBuilder.Configurations.Add(new VendorMap());
            modelBuilder.Configurations.Add(new VersionNoteMap());
            modelBuilder.Configurations.Add(new WIPStoreMap());
            modelBuilder.Configurations.Add(new WIPStoreItemMap());
        }
    }
}
