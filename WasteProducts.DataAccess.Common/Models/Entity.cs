using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models
{
    public class Entity
    {
        public int Id { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual DateTime? Modified { get; set; }
    }
}
