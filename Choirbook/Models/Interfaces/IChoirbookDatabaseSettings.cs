namespace Choirbook.Models.Interfaces
{
    public interface IChoirbookDatabaseSettings
    {
        string ChoirbookCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}