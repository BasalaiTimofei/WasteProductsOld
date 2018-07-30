using System;

namespace WasteProducts.DataAccess.Common.Models
{
    public class Entity
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}