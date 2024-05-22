using System;
using System.Collections.Generic;

namespace qqq.umiheeva
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DeliveryTypeId { get; set; }
        public int PaymentTypeId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual DeliveryType DeliveryType { get; set; } = null!;
        public virtual PaymentType PaymentType { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
