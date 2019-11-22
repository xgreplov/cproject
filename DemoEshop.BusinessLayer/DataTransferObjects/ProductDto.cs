using System;
using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class ProductDto : DtoBase
    {
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(65536)]
        public string Description { get; set; }

        [Range(0, 10_000)]
        public int StoredUnits { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        public DiscountType DiscountType { get; set; }

        [Range(0, 99)]
        public int DiscountPercentage { get; set; }

        [MaxLength(1024)]
        public string ProductImgUri { get; set; }

        public Guid CategoryId { get; set; }

        public CategoryDto Category { get; set; }

        public int? CurrentlyAvailableUnits { get; set; }

        public override string ToString()
        {
            return Name;
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
            return obj.GetType() == this.GetType() && ((ProductDto)obj).GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            if (!Id.Equals(Guid.Empty))
            {
                return Id.GetHashCode();
            }
            unchecked
            {
                var hashCode = Name?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)DiscountType;
                hashCode = (hashCode * 397) ^ StoredUnits;
                hashCode = (hashCode * 397) ^ DiscountPercentage.GetHashCode();
                hashCode = (hashCode * 397) ^ (ProductImgUri?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ CategoryId.GetHashCode();
                return hashCode;
            }
        }
    }
}
