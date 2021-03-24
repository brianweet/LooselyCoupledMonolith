using System.Threading.Tasks;
using NServiceBus;

namespace Billing
{
    public class ChargeCustomer : IHandleMessages<Billing.Contracts.BillOrder>
    {
        public async Task Handle(Billing.Contracts.BillOrder message, IMessageHandlerContext context)
        {
            await Task.Delay(1000);
        }
    }
}