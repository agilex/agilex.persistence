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
        public ISessionFactory GetSessionFactory(IDatabaseConfigurationParams configurationParams)
        {
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008. /*ShowSql().*/ConnectionString(
                        m => m.FromAppSetting(configurationParams.AppSettingKeyForDbConnectionString)))
                .Mappings(m =>
                              {
                                  foreach (Assembly assembly in configurationParams.Assemblies)
                                  {
                                      m.FluentMappings.AddFromAssembly(assembly);
                                  }
                              })
                .ExposeConfiguration(cfg => BuildSchema(cfg, configurationParams.BlowDbAway, configurationParams.SchemaExportLocation))
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