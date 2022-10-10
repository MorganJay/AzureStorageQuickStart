namespace AngularWebApp.Helpers
{
    public interface IBlobStorage
    {
        public Task<List<string>> GetAllDocuments(string connectionString, string containerName);
        Task UploadDocument(string connectionString, string containerName, string fileName, Stream fileContent);
        Task<Stream> GetDocument(string connectionString, string containerName, string fileName);
        Task<bool> DeleteDocument(string connectionString, string containerName, string fileName);
    }
}
