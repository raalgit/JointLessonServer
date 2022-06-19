namespace JL_MongoDB
{
    public interface IMongoDbSettings
    {
        public string FilesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
