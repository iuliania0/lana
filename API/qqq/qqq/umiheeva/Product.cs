using System;
using System.Collections.Generic;

namespace qqq.umiheeva
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
            Photos = new HashSet<Photo>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int VendorCode { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
