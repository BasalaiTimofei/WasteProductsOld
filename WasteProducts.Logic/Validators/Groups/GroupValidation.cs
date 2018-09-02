using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Validators
{
    public class GroupValidation : Exception
    {
        public string Property { get; protected set; }
        public GroupValidation(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
