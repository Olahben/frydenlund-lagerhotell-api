using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<LagerhotellAPI.Models.DbModels.Order> _orders;
        public OrderService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _orders = database.GetCollection<LagerhotellAPI.Models.DbModels.Order>("Orders");
        }
        public async Task AddOrder(Order order)
        {
            LagerhotellAPI.Models.DbModels.Order dbOrder = new LagerhotellAPI.Models.DbModels.Order { OrderId = order.Id, OrderDate = order.OrderDate, EndDate = order.EndDate, UserId = order.UserId, StorageUnitId = order.StorageUnitId, Status = order.Status, CustomInstructions = order.CustomInstructions };
            await _orders.InsertOneAsync(dbOrder);
        }
        public async Task DeleteOrder(string orderId)
        {
            await _orders.DeleteOneAsync(order => order.Id == orderId);
        }
        public async Task ModifyOrder(string orderId, Order updatedOrder)
        {
            LagerhotellAPI.Models.DbModels.Order updatedDbOrder = new LagerhotellAPI.Models.DbModels.Order { OrderId = updatedOrder.Id, OrderDate = updatedOrder.OrderDate, EndDate = updatedOrder.EndDate, UserId = updatedOrder.UserId, StorageUnitId = updatedOrder.StorageUnitId, Status = updatedOrder.Status, CustomInstructions = updatedOrder.CustomInstructions };
            await _orders.ReplaceOneAsync(order => order.Id == orderId, updatedDbOrder);
        }
        public async Task<Order?> GetOrder(string orderId)
        {
            var dbOrder = await _orders.Find(order => order.Id == orderId).FirstOrDefaultAsync();
            if (dbOrder == null)
            {
                return null;
            }
            LagerhotellAPI.Models.DomainModels.Order domainOrder = new LagerhotellAPI.Models.DomainModels.Order { Id = dbOrder.OrderId, OrderDate = dbOrder.OrderDate, UserId = dbOrder.UserId, StorageUnitId = dbOrder.StorageUnitId, CustomInstructions = dbOrder.CustomInstructions };
            return domainOrder;
        }
        public async Task<List<LagerhotellAPI.Models.DomainModels.Order>> GetAllOrders(string? userId, int? skip, int? take)
        {
            var filter = Builders<LagerhotellAPI.Models.DbModels.Order>.Filter.Empty; // Default filter

            if (userId != null)
            {
                filter = Builders<LagerhotellAPI.Models.DbModels.Order>.Filter.Eq(order => order.UserId, userId);
            }

            List<LagerhotellAPI.Models.DbModels.Order> dbOrders = await _orders.Find(filter).Skip(skip).Limit(take).ToListAsync();

            List<LagerhotellAPI.Models.DomainModels.Order> domainOrders = dbOrders.ConvertAll(dbOrder =>
            {
                return new LagerhotellAPI.Models.DomainModels.Order
                {
                    Id = dbOrder.OrderId,
                    OrderDate = dbOrder.OrderDate,
                    EndDate = dbOrder.EndDate,
                    UserId = dbOrder.UserId,
                    StorageUnitId = dbOrder.StorageUnitId,
                    Status = dbOrder.Status,
                    CustomInstructions = dbOrder.CustomInstructions
                };
            });

            return domainOrders;
        }
    }
}