using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class RatingDto : DtoBase
    {
        public DateTime Issued { get; set; }

        public decimal TotalPrice { get; set; }

        public Guid CustomerId { get; set; }

        public override string ToString()
        {
            return $"Order by {CustomerId}, issued at: {Issued}";
        }

        protected bool Equals(RatingDto other)
        {
            if (!Id.Equals(Guid.Empty))
            {
                return this.Id == other.Id;
            }
            return Issued.Equals(other.Issued) &&
                TotalPrice == other.TotalPrice &&
                CustomerId == other.CustomerId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == this.GetType() &&
                Equals((RatingDto)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Issued.GetHashCode();
                hashCode = (hashCode * 397) ^ TotalPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ CustomerId.GetHashCode();
                return hashCode;
            }
        }
    }
}
