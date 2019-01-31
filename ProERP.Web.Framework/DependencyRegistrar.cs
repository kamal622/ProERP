using Autofac;
using Autofac.Integration.Mvc;
using ProERP.Core.Configuration;
using ProERP.Core.Data;
using ProERP.Core.Fakes;
using ProERP.Core.Infrastructure;
using ProERP.Core.Infrastructure.DependencyManagement;
using ProERP.Data;
using ProERP.Data.Models;
using ProERP.Services;
using ProERP.Services.Breakdown;
using ProERP.Services.Dashboard;
using ProERP.Services.Document;
using ProERP.Services.FormulationRequest;
using ProERP.Services.Indent;
//using ProERP.Services.Indents;
using ProERP.Services.Line;
using ProERP.Services.Machine;
using ProERP.Services.MaintenanceRequest;
using ProERP.Services.Part;
using ProERP.Services.Plant;
using ProERP.Services.PreventiveMaintenance;
using ProERP.Services.Product;
using ProERP.Services.Reports;
using ProERP.Services.RMItem;
using ProERP.Services.Site;
using ProERP.Services.User;
using ProERP.Services.Utility;
using ProERP.Services.WIPStore;
//using ProERP.Services.SubAssembly;
using ProERP.Web.Framework.UI;
using System.Linq;
using System.Web;

namespace ProERP.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 0; }
        }

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, ProConfig config)
        {
            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //web helper
            //builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            builder.Register<IDbContext>(c => new ProERPContext()).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<SiteService>().InstancePerRequest();
            builder.RegisterType<BreakdownService>().InstancePerRequest();
            builder.RegisterType<PlantService>().InstancePerRequest();
            builder.RegisterType<PartService>().InstancePerRequest();
            builder.RegisterType<LineService>().InstancePerRequest();
            builder.RegisterType<MachineService>().InstancePerRequest();
            // builder.RegisterType<SubAssemblyService>().InstancePerRequest();
            builder.RegisterType<UserService>().InstancePerRequest();
            builder.RegisterType<PreventiveMaintenanceService>().InstancePerRequest();
            builder.RegisterType<PreventiveWorkDescriptionService>().InstancePerRequest();
            builder.RegisterType<UserAssignmentsService>().InstancePerRequest();
            builder.RegisterType<PreventiveReviewHistoryService>().InstancePerRequest();
            builder.RegisterType<MaintenanceRequestServices>().InstancePerRequest();
            builder.RegisterType<MaintenancePriorityTypeServices>().InstancePerRequest();
            builder.RegisterType<MaintenanceUserAssignmentsServices>().InstancePerRequest();
            builder.RegisterType<StatusServices>().InstancePerRequest();
            builder.RegisterType<ScheduleTypeService>().InstancePerRequest();
            //builder.RegisterType<IndentServices>().InstancePerRequest();
            builder.RegisterType<PreventiveHoldHistoryService>().InstancePerRequest();
            builder.RegisterType<VendorCategoryService>().InstancePerRequest();
            builder.RegisterType<VendorService>().InstancePerRequest();
            builder.RegisterType<EmployeeTypeService>().InstancePerRequest();
            builder.RegisterType<ItemsServices>().InstancePerRequest();
            builder.RegisterType<IndentsServices>().InstancePerRequest();
            builder.RegisterType<IndentDetailServices>().InstancePerRequest();
            builder.RegisterType<IndentStatusServices>().InstancePerRequest();
            builder.RegisterType<IndentBudgetServices>().InstancePerRequest();
            builder.RegisterType<IndentDetailAttachmentServices>().InstancePerRequest();
            builder.RegisterType<DocumentTypeServices>().InstancePerRequest();
            builder.RegisterType<DocumentService>().InstancePerRequest();
            builder.RegisterType<DocumentHistoryService>().InstancePerRequest();
            builder.RegisterType<BreakDownAttachmentServices>().InstancePerRequest();
            builder.RegisterType<TemplateService>().InstancePerRequest();
            builder.RegisterType<UtilityService>().InstancePerRequest();
            builder.RegisterType<ReportService>().InstancePerRequest();
            builder.RegisterType<DashboardService>().InstancePerRequest();
            builder.RegisterType<FormulationRequestService>().InstancePerRequest();
            builder.RegisterType<RMItemService>().InstancePerRequest();
            builder.RegisterType<ProductService>().InstancePerRequest();
            builder.RegisterType<WIPStoreService>().InstancePerRequest();
        }
    }
}

