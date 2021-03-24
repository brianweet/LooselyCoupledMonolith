using System;
using NServiceBus;

namespace Sales
{
    public class PlaceOrderSagaData : ContainSagaData
    {
        public Guid OrderId { get; set; }
        public bool HasBeenBilled { get; set; }
    }
}