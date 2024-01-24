using CafeReadConf.Frontend.Models;

namespace CafeReadConf.Frontend.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Get all users from Azure Table Storage
        /// </summary>
        /// <returns></returns>
        public abstract Task<List<UserFromApi>> GetUsers();
    }
}
