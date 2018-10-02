using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Donations;

namespace WasteProducts.DataAccess.Common.Comparers.Donations
{
    public class DonorDBComparer : EqualityComparer<DonorDB>
    {
        public override bool Equals(DonorDB x, DonorDB y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x == null ^ y == null)
                return false;
            AddressDBComparer addressDBComparer = new AddressDBComparer();
            return addressDBComparer.Equals(x.Address, y.Address) &&
                x.Email == y.Email &&
                x.FirstName == y.FirstName &&
                x.Id == y.Id &&
                x.IsVerified == y.IsVerified &&
                x.LastName == y.LastName;
        }

        public override int GetHashCode(DonorDB obj)
        {
            return Tuple.Create(
                obj.Address,
                obj.Created,
                obj.Email,
                obj.FirstName,
                obj.Id,
                obj.LastName,
                obj.Modified).GetHashCode();
        }
    }
}