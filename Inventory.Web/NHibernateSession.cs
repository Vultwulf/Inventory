using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Web
{
    public class NHibernateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
            var productConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\Product.hbm.xml");
            configuration.AddFile(productConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}