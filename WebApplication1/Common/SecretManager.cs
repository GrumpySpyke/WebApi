using Microsoft.Extensions.Options;
using WebApplication1.Common.Contracts;
using WebApplication1.Common.Entity;

namespace WebApplication1.Common
{
    public class FileSecretManager : ISecretManager
    {
        private DbSecret _dbSecret { get; set; }
        public FileSecretManager(IOptions<DbSecret> dbsecret) 
        {
            _dbSecret = dbsecret.Value;
        }

        public string GetDbUsername()
        {
            return _dbSecret.username;
        }
        
        public string GetDbPassword()
        {
            return _dbSecret.password;
        }
    }
}
