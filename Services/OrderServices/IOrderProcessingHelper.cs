using MongoDB.Bson;

namespace SiparisUygulamasi.Services.OrderServices
{
    public interface IOrderProcessingHelper
    {
        Task ProcessOrderAsync(ObjectId userId);
    }
}
