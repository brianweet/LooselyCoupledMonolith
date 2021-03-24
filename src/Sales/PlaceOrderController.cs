using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Sales
{
    public class PlaceOrderController : Controller
    {
        private readonly IMessageSession _messageSession;

        public PlaceOrderController(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        [HttpPost("/sales/orders/{orderId:Guid}")]
        public async Task<IActionResult> Action([FromRoute] Guid orderId)
        {
            await _messageSession.Send(new PlaceOrder
            {
                OrderId = orderId
            });

            return NoContent();
        }
    }
}