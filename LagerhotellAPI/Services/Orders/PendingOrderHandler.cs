namespace LagerhotellAPI.Services
{
    public class PendingOrderHandler : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<PendingOrderHandler> _logger;
        private Timer _timer;

        public PendingOrderHandler(IServiceScopeFactory serviceScopeFactory, ILogger<PendingOrderHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PendingOrderHandler is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("PendingOrderHandler is executing DoWork.");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();

                List<Order> pendingOrders = await orderService.GetAllOrders(userId: null, skip: 0, take: 0, orderStatus: OrderStatus.Pending);

                if (pendingOrders.Count == 0)
                {
                    _logger.LogInformation("No pending orders found.");
                }
                else
                {
                    foreach (var order in pendingOrders)
                    {
                        _logger.LogInformation($"Processing order ID: {order.OrderId}, Status: {order.Status}");
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PendingOrderHandler is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
