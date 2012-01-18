using System.Collections.Generic;
using System.Reflection;

namespace agilex.persistence
{
    public class DatabaseConfigurationParams : IDatabaseConfigurationParams
    {
        public DatabaseConfigurationParams(string appSettingKeyForDbConnectionString, IEnumerable<Assembly> assemblies,
                                           string schemaExportLocation, bool blowDbAway)
        {
            BlowDbAway = blowDbAway;
            SchemaExportLocation = schemaExportLocation;
            Assemblies = assemblies;
            AppSettingKeyForDbConnectionString = appSettingKeyForDbConnectionString;
        }

        #region IDatabaseConfigurationParams Members

        public string AppSettingKeyForDbConnectionString { get; private set; }
        public IEnumerable<Assembly> Assemblies { get; private set; }
        public string SchemaExportLocation { get; private set; }
        public bool BlowDbAway { get; private set; }

        #endregion
    }
}