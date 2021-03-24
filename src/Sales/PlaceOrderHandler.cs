using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Sales.Contracts;

namespace Sales
{

    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        private readonly SalesDbContext _dbContext;

        public PlaceOrderHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            Debug.WriteLine($"PlaceOrderHandler. OrderId: {message.OrderId}. MessageId: {context.MessageId}");
            await _dbContext.Orders.AddAsync(new Order
            {
                OrderId = message.OrderId,
                Status = OrderStatus.Pending
            });
            await _dbContext.SaveChangesAsync();

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            await context.Publish(orderPlaced);
        }
    }
}