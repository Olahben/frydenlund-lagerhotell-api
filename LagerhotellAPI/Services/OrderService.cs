using LagerhotellAPI.Models;
using MongoDB.Driver;

namespace LagerhotellAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;
        public OrderService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("Lagerhotell");
            _orders = database.GetCollection<Order>("Orders");
        }
        public async Task AddOrder(Order order)
        {
            await _orders.InsertOneAsync(order);
        }
        public async Task DeleteOrder(string orderId)
        {
            await _orders.DeleteOneAsync(order => order.Id == orderId);
        }
        public async Task ModifyOrder(string orderId, Order updatedOrder)
        {
            await _orders.ReplaceOneAsync(order => order.Id == orderId, updatedOrder);
        }
        public async Task<Order> GetOrder(string orderId)
        {
            return await _orders.Find(order => order.Id == orderId).FirstOrDefaultAsync();
        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await _orders.Find(_ => true).ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersByUser(User user)
        {
            var filter = Builders<Order>.Filter.Eq("UserId", user.Id);
            return await _orders.Find(filter).ToListAsync();
        }
    }
}