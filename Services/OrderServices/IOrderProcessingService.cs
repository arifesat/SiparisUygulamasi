using MongoDB.Bson;

public interface IOrderProcessingService
{
    Task ProcessOrderAsync(ObjectId userId);
}