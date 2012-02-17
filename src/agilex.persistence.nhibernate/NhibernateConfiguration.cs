using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

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
                .ExposeConfiguration(cfg => BuildSchema(cfg, configurationParams.BlowDbAway, configurationParams.ShowSql, configurationParams.SchemaExportLocation))
                .BuildSessionFactory();
        }

        IPersistenceConfigurer ConfigureDbWith(IDatabaseConfigurationParams configurationParams)
        {
            var connectionString =
                ConfigurationManager.AppSettings[configurationParams.AppSettingKeyForDbConnectionString];

            switch (configurationParams.Dialect)
            {
                case Dialect.SqlServer2008:
                    return configurationParams.ShowSql
                        ? MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(connectionString)
                        : MsSqlConfiguration.MsSql2008.ConnectionString(connectionString);
                case Dialect.SqlServer2005:
                    return configurationParams.ShowSql
                        ? MsSqlConfiguration.MsSql2005.ShowSql().ConnectionString(connectionString)
                        : MsSqlConfiguration.MsSql2005.ConnectionString(connectionString);
                case Dialect.Oracle10:
                    return configurationParams.ShowSql
                        ? OracleClientConfiguration.Oracle10.ShowSql().ConnectionString(connectionString)
                        : OracleClientConfiguration.Oracle10.ConnectionString(connectionString);
                case Dialect.Oracle9:
                    return configurationParams.ShowSql
                        ? OracleClientConfiguration.Oracle9.ShowSql().ConnectionString(connectionString)
                        : OracleClientConfiguration.Oracle9.ConnectionString(connectionString);
                case Dialect.MySQL:
                    return configurationParams.ShowSql
                        ? MySQLConfiguration.Standard.ShowSql().ConnectionString(connectionString)
                        : MySQLConfiguration.Standard.ConnectionString(connectionString);
                case Dialect.Postgres:
                    return configurationParams.ShowSql
                        ? PostgreSQLConfiguration.Standard.ShowSql().ConnectionString(connectionString)
                        : PostgreSQLConfiguration.Standard.ConnectionString(connectionString);
                case Dialect.SqlLite:
                    return configurationParams.ShowSql
                        ? SQLiteConfiguration.Standard.ShowSql().ConnectionString(connectionString)
                        : SQLiteConfiguration.Standard.ConnectionString(connectionString);
                default:
                    throw new Exception("Unknown DB Dialiect");
            }
        }

        protected virtual void BuildSchema(Configuration config, bool blowDbAway, bool showSql, string schemaExportLocation)
        {
            if (showSql) config.Interceptor = new LoggingInterceptor();
            if (!blowDbAway) return;
            new SchemaExport(config)
                .SetOutputFile(schemaExportLocation)
                .Execute(true /*script*/, true /*export to db*/,
                         false /*just drop*/);
        }
    }

    public class LoggingInterceptor : EmptyInterceptor
    {
        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            Debug.WriteLine(sql);
            return base.OnPrepareStatement(sql);
        }
    }
}