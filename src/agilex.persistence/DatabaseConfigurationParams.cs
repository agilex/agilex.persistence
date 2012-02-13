using System.Collections.Generic;
using System.Reflection;

namespace agilex.persistence
{
    public class DatabaseConfigurationParams : IDatabaseConfigurationParams
    {
        public DatabaseConfigurationParams(string appSettingKeyForDbConnectionString, IEnumerable<Assembly> assemblies,
                                           string schemaExportLocation, bool blowDbAway, Dialect dialect)
        {
            BlowDbAway = blowDbAway;
            SchemaExportLocation = schemaExportLocation;
            Assemblies = assemblies;
            AppSettingKeyForDbConnectionString = appSettingKeyForDbConnectionString;
            Dialect = dialect;
        }

        #region IDatabaseConfigurationParams Members

        public string AppSettingKeyForDbConnectionString { get; private set; }
        public IEnumerable<Assembly> Assemblies { get; private set; }
        public string SchemaExportLocation { get; private set; }
        public bool BlowDbAway { get; private set; }
        public Dialect Dialect { get; private set; }        

        #endregion
    }
}