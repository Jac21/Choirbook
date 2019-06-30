using Choirbook.Models.Interfaces;

namespace Choirbook.Models
{
    public class ChoirbookDatabaseSettings : IChoirbookDatabaseSettings
    {
        public string ChoirbookCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}