using System.Security.Cryptography.X509Certificates;
using Raven.Client.Documents;

namespace storagemanagement3.Services
{
    public class RavenDbService
    {
        public IDocumentStore DocumentStore { get; }

        public RavenDbService(IConfiguration configuration)
        {
            var urls = configuration.GetSection("RavenDb:Urls").Get<string[]>();
            var databaseName = configuration.GetSection("RavenDb:Database").Value;
            var certPath = configuration.GetSection("RavenDb:CertificatePath").Value;

            if (urls == null || string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(certPath))
            {
                throw new InvalidOperationException("Missing RavenDB configuration in appsettings.json.");
            }

            var certificate = new X509Certificate2(certPath);

            DocumentStore = new DocumentStore
            {
                Urls = urls,
                Database = databaseName,
                Certificate = certificate
            };
            DocumentStore.Initialize();
        }
    }
}
