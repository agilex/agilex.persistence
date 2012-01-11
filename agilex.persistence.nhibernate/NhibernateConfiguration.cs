using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace agilex.persistence.nhibernate
{
    public class NhibernateConfiguration
    {
        public ISessionFactory GetSessionFactory(IEnumerable<Assembly> assemblies, bool blowDbAway,
                                                 string schemaExportLocation, string appSettingKeyForDbConnectionString)
        {
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008./*ShowSql().*/ConnectionString(
                        m => m.FromAppSetting(appSettingKeyForDbConnectionString)))
                .Mappings(m =>
                    {
                        foreach (var assembly in assemblies)
                        {
                            m.FluentMappings.AddFromAssembly(assembly);
                        }
                    })
                .ExposeConfiguration(cfg => BuildSchema(cfg, blowDbAway, schemaExportLocation))
                .BuildSessionFactory();
        }


        protected virtual void BuildSchema(Configuration config, bool blowDbAway, string schemaExportLocation)
        {
            if (!blowDbAway) return;
            new SchemaExport(config)
                .SetOutputFile(schemaExportLocation)
                .Execute(true /*script*/, true /*export to db*/,
                         false /*just drop*/);
        }
    }
}