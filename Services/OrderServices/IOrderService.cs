using MongoDB.Bson;

namespace SiparisUygulamasi.Services.OrderServices
{
    public interface IOrderService
    {
        Task ProcessOrderAsync(ObjectId userId);
    }
}
