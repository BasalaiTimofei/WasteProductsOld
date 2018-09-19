using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Donations;

namespace WasteProducts.DataAccess.Common.Comparers.Donations
{
    public class AddressDBComparer : EqualityComparer<AddressDB>
    {
        public override bool Equals(AddressDB x, AddressDB y)
        {
            if (x == null && y == null)
                return true;
            if (x == null ^ y == null)
                return false;
            return x.City == y.City &&
                x.Country == y.Country &&
                x.IsConfirmed == y.IsConfirmed &&
                x.Name == y.Name &&
                x.State == y.State &&
                x.Street == y.Street &&
                x.Zip == y.Zip;
        }

        public override int GetHashCode(AddressDB obj)
        {
            return Tuple.Create(
                obj.City,
                obj.Country,
                obj.Created,
                obj.Name,
                obj.State,
                obj.Street,
                obj.Zip).GetHashCode();
        }
    }
}
