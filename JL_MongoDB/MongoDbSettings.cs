namespace JL_MongoDB
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string FilesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
