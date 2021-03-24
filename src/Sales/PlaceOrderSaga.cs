using System.Threading.Tasks;
using Billing.Contracts;
using NServiceBus;
using Sales.Contracts;
using Shipping.Contracts;

namespace Sales
{
    public class PlaceOrderSaga :
        Saga<PlaceOrderSagaData>,
        IAmStartedByMessages<OrderPlaced>,
        IHandleMessages<OrderBilled>,
        IHandleMessages<ShippingLabelCreated>,
        IHandleMessages<Backordered>,
        IHandleMessages<OrderRefunded>
    {

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PlaceOrderSagaData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<OrderBilled>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<ShippingLabelCreated>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<Backordered>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<OrderRefunded>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
        }

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            await context.Send(new BillOrder
            {
                OrderId = message.OrderId
            });
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Data.HasBeenBilled = true;
        }

        public async Task Handle(ShippingLabelCreated message, IMessageHandlerContext context)
        {
            await context.SendLocal(new ReadyToShipOrder
            {
                OrderId = Data.OrderId
            });
            MarkAsComplete();
        }

        public async Task Handle(Backordered message, IMessageHandlerContext context)
        {
            if (Data.HasBeenBilled)
            {
                await context.Send(new RefundOrder()
                {
                    OrderId = message.OrderId
                });
            }
            else
            {
                MarkAsComplete();
            }
        }

        public async Task Handle(OrderRefunded message, IMessageHandlerContext context)
        {
            await context.SendLocal(new CancelOrder
            {
                OrderId = message.OrderId
            });
            MarkAsComplete();
        }
    }
}