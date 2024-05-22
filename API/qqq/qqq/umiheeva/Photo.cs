using System;
using System.Collections.Generic;

namespace qqq.umiheeva
{
    public partial class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
