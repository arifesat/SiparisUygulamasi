using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Repositories
{
    public interface IUserRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByNameAsync(string UserName);
        Task<TEntity> GetByEmailAsync(string email);
        Task<TEntity> UpdateUserAsync(object id, User entity);
        Task<TEntity> UpdateUserBookAsync(object id, User entity);
        Task<TEntity> GetUserById(Object _id);
    }
}
