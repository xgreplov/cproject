using System;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    /// <summary>
    /// Represents stock reservation made by customer
    /// </summary>
    public class ProductReservationDto
    {
        /// <summary>
        /// ID of the corresponding stock
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// ID of customer who made the reservation
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Amount of units reserved
        /// </summary>
        public int ReservedAmount { get; set; }

        /// <summary>
        /// Reservation expiration
        /// </summary>
        public DateTime Expiration { get; set; }

        public override string ToString()
        {
            return $"Customer with ID: {CustomerId} reserved {ReservedAmount} units of stock" +
                        $" with ID: {ProductId} with expiration: {Expiration.ToShortTimeString()}";
        }

        protected bool Equals(ProductReservationDto other)
        {
            return ProductId == other.ProductId && CustomerId == other.CustomerId && 
                ReservedAmount == other.ReservedAmount && Expiration.Equals(other.Expiration);
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
            return obj.GetType() == this.GetType() && Equals((ProductReservationDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ProductId.GetHashCode();
                hashCode = (hashCode*397) ^ CustomerId.GetHashCode();
                hashCode = (hashCode*397) ^ ReservedAmount;
                hashCode = (hashCode * 397) ^ Expiration.GetHashCode();
                return hashCode;
            }
        }

    }
}
