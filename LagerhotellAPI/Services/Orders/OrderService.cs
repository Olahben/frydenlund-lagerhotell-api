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

        /// <summary>
        /// Adds a order to the database
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Id</returns>
        public async Task<string> AddOrder(Order order)
        {
            string orderId = Guid.NewGuid().ToString();
            // Id is set automatically by mongoDB
            LagerhotellAPI.Models.DbModels.Order dbOrder = new()
            {
                OrderId = orderId,
                OrderPeriod = order.OrderPeriod,
                UserId = order.UserId,
                StorageUnitId = order.StorageUnitId,
                Status = order.Status,
                CustomInstructions = order.CustomInstructions,
                Insurance = order.Insurance

            };
            await _orders.InsertOneAsync(dbOrder);
            return orderId;
        }

        /// <summary>
        /// Deletes a order from the database
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task DeleteOrder(string orderId)
        {
            await _orders.DeleteOneAsync(order => order.Id == orderId);
        }

        /// <summary>
        /// Modifies an order in the database (does not modify the orderId)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="updatedOrder"></param>
        /// <returns></returns>
        public async Task ModifyOrder(string orderId, Order updatedOrder)
        {
            LagerhotellAPI.Models.DbModels.Order updatedDbOrder = new(orderId, updatedOrder.UserId, updatedOrder.OrderPeriod, updatedOrder.StorageUnitId, updatedOrder.Status, updatedOrder.CustomInstructions);
            await _orders.ReplaceOneAsync(order => order.OrderId == orderId, updatedDbOrder);
        }

        /// <summary>
        /// Gets an order from the database with the given order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>order</returns>
        public async Task<Order?> GetOrder(string orderId)
        {
            var dbOrder = await _orders.Find(order => order.Id == orderId).FirstOrDefaultAsync();
            if (dbOrder == null)
            {
                return null;
            }
            LagerhotellAPI.Models.DomainModels.Order domainOrder = new(dbOrder.OrderId, dbOrder.OrderPeriod, dbOrder.UserId, dbOrder.StorageUnitId, dbOrder.Status, dbOrder.CustomInstructions);
            return domainOrder;
        }

        /// <summary>
        /// Gets the order with the given order Id from the database
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>database model of order</returns>
        public async Task<Models.DbModels.Order> GetOrderDbModel(string orderId)
        {
            var dbOrder = await _orders.Find(order => order.Id == orderId).FirstOrDefaultAsync();
            return dbOrder;
        }

        /// <summary>
        /// Gets all orders from the database optionally filtered by userId and orderStatus.
        /// </summary>
        /// <param name="userId">The user ID to filter orders by (optional).</param>
        /// <param name="skip">Number of records to skip (optional).</param>
        /// <param name="take">Number of records to take (optional).</param>
        /// <param name="orderStatus">The order status to filter by (optional).</param>
        /// <returns>A list of orders filtered by the specified criteria.</returns>
        public async Task<List<Models.DomainModels.Order>> GetAllOrders(string? userId, int? skip, int? take, OrderStatus? orderStatus)
        {
            var filterBuilder = Builders<Models.DbModels.Order>.Filter;
            var filter = filterBuilder.Empty; // Default filter

            if (userId != null)
            {
                filter = filterBuilder.Eq(order => order.UserId, userId);
            }

            if (orderStatus != null)
            {
                filter &= filterBuilder.Eq(order => order.Status, orderStatus);
            }

            List<Models.DbModels.Order> dbOrders = await _orders.Find(filter)
                                                       .Skip(skip ?? 0)
                                                       .Limit(take ?? int.MaxValue)
                                                       .ToListAsync();

            List<Models.DomainModels.Order> domainOrders = dbOrders.ConvertAll(dbOrder =>
                new Models.DomainModels.Order(dbOrder.Id, dbOrder.OrderPeriod, dbOrder.UserId, dbOrder.StorageUnitId, dbOrder.Status, dbOrder.CustomInstructions));

            return domainOrders;
        }
    }
}