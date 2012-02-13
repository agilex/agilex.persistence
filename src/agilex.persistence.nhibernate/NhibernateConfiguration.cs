using System;
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
                    ConfigureDbWith(configurationParams))
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

        IPersistenceConfigurer ConfigureDbWith(IDatabaseConfigurationParams configurationParams)
        {
            if (configurationParams.Dialect == Dialect.SqlServer2008)
                return MsSqlConfiguration.MsSql2008.ConnectionString(
                    m => m.FromAppSetting(configurationParams.AppSettingKeyForDbConnectionString));
            if (configurationParams.Dialect == Dialect.SqlServer2005)
                return MsSqlConfiguration.MsSql2005.ConnectionString(
                    m => m.FromAppSetting(configurationParams.AppSettingKeyForDbConnectionString));
            if (configurationParams.Dialect == Dialect.Oracle10)
                return OracleClientConfiguration.Oracle10.ConnectionString(
                    m => m.FromAppSetting(configurationParams.AppSettingKeyForDbConnectionString));
            if (configurationParams.Dialect == Dialect.Oracle9)
                return OracleClientConfiguration.Oracle9.ConnectionString(
                    m => m.FromAppSetting(configurationParams.AppSettingKeyForDbConnectionString));
            if (configurationParams.Dialect == Dialect.MySQL)
                return MySQLConfiguration.Standard.ConnectionString(
                    m => m.FromAppSetting(configurationParams.AppSettingKeyForDbConnectionString));
            if (configurationParams.Dialect == Dialect.Postgres)
                return PostgreSQLConfiguration.Standard.ConnectionString(
                    m => m.FromAppSetting(configurationParams.AppSettingKeyForDbConnectionString));
            throw new Exception("Unknown DB Dialiect");
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