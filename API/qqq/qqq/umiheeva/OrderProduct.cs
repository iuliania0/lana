using System;
using System.Collections.Generic;

namespace qqq.umiheeva
{
    public partial class OrderProduct
    {
        public int Id { get; set; }
        public decimal TransactionAmount { get; set; }
        public int NumberOfProducts { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
