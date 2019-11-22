using System;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    /// <summary>
    /// Wrapper class for single item within user Rating
    /// </summary>
    public class RateSongDto : DtoBase
    {
        public int Value { get; set; }

        public ProductDto Product { get; set; }

        

        protected bool Equals(RateSongDto other)
        {
            if (!Id.Equals(Guid.Empty))
            {
                return this.Id == other.Id;
            }
            return Value == other.Value && 
                Equals(Product, other.Product);
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
                Equals((RateSongDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode*397) ^ Value;
                hashCode = (hashCode*397) ^ (Product?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{Product} {Value}x";
        }
    }
}
