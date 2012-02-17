using System.Collections.Generic;
using System.Reflection;

namespace agilex.persistence
{
    public class DatabaseConfigurationParams : IDatabaseConfigurationParams
    {
        public DatabaseConfigurationParams(string appSettingKeyForDbConnectionString, IEnumerable<Assembly> assemblies,
                                           string schemaExportLocation, bool blowDbAway, Dialect dialect) : this(appSettingKeyForDbConnectionString, assemblies, dialect)
        {
            BlowDbAway = blowDbAway;
            SchemaExportLocation = schemaExportLocation;
        }

        public DatabaseConfigurationParams(string appSettingKeyForDbConnectionString, IEnumerable<Assembly> assemblies,
                                           Dialect dialect)
        {
            Assemblies = assemblies;
            AppSettingKeyForDbConnectionString = appSettingKeyForDbConnectionString;
            Dialect = dialect;
        }

        #region IDatabaseConfigurationParams Members

        public string AppSettingKeyForDbConnectionString { get; set; }
        public IEnumerable<Assembly> Assemblies { get; set; }
        public string SchemaExportLocation { get; set; }
        public bool BlowDbAway { get; set; }
        public Dialect Dialect { get; set; }
        public bool ShowSql { get; set; }

        #endregion
    }
}