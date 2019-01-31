using Autofac;
using Autofac.Core;
using ProERP.Core.Caching;
using ProERP.Core.Configuration;
using ProERP.Core.Infrastructure;
using ProERP.Core.Infrastructure.DependencyManagement;
using ProERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 2; }
        }

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, ProConfig config)
        {
            //we cache presentation models between requests
            builder.RegisterType<HomeController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
            builder.RegisterType<SiteController>()
               .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
            //builder.RegisterType<BreakdownController>()
            //   .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
            builder.RegisterType<PreventiveMaintenanceController>()
               .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
            builder.RegisterType<Reports.ReportTemplate>()
               .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
            builder.RegisterType<DownloadController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
            builder.RegisterType<FormulationRequestController>()
               .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
            builder.RegisterType<ProductController>()
               .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
            builder.RegisterType<RMItemController>()
               .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("pro_cache_static"));
        }
    }
}