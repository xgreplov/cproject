using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using Nito.AsyncEx;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace DemoEshop.BusinessLayer.Services.Reservation
{
    public class ReservationService : ServiceBase, IReservationService
    {
        /// <summary>
        /// Preserves number of reserved units per customer for each product
        /// Key is id of the corresponding product
        /// </summary>
        private static readonly IDictionary<Guid, IList<ProductReservationDto>> ReservedProductUnits = new Dictionary<Guid, IList<ProductReservationDto>>();

        private static readonly AsyncLock AvailableUnitsLock = new AsyncLock();

        private readonly IRepository<Product> productRepository;
        
        public ReservationService(IMapper mapper, IRepository<Product> productRepository) : base(mapper)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Performs product reservation in Order to ensure
        /// simultaneous Orders can be satisfied
        /// </summary>
        /// <param name="productReservation">Product reservation details containing 
        ///   customer reserving the product, amount, and expiration</param>
        /// <returns>true if reservation is successfull, otherwise false</returns>
        public async Task<bool> ReserveProduct(ProductReservationDto productReservation)
        {
            ReleaseProductReservationsCore(reservation => DateTime.Now > reservation.Expiration);
            CheckProductReservationIsValid(productReservation);
            EnsureDictionaryRecordIsInitialized(productReservation.ProductId);

            using (AvailableUnitsLock.Lock())
            {
                return await ReserveProductWithinLockCore(productReservation);
            }
        }

        /// <summary>
        /// Performs product release 
        /// (typically when user confirms or cancels the Order)
        /// </summary>
        /// <param name="customerId">ID of customer who reserved the product</param>
        /// <param name="productIds">IDs of the product to release</param>
        public void ReleaseProductReservations(Guid customerId, params Guid[] productIds)
        {
            if (customerId.Equals(Guid.Empty))
            {
                return;
            }

            Func<ProductReservationDto, bool> deletePredicate;
            var cancelAllCustomerReservations = productIds.Length == 0;

            if (cancelAllCustomerReservations)
            {
                deletePredicate = reservation => reservation.CustomerId == customerId;
            }
            else
            {
                deletePredicate = reservation => productIds.Contains(reservation.ProductId) && reservation.CustomerId == customerId;
            }

            ReleaseProductReservationsCore(deletePredicate);
        }

        /// <summary>
        /// Performs product expedition by decreasing product 
        /// stored units and releasing corresponding reservations
        /// </summary>
        /// <param name="customerId">Id of customer whose reserved product should be dispatched</param>
        public async Task DispatchProduct(Guid customerId)
        {
            using (AvailableUnitsLock.Lock())
            {
                var allCustomerReservations = new List<ProductReservationDto>();

                foreach (var reservationList in ReservedProductUnits.Values
                    .Where(list => list.Any(item => item.CustomerId == customerId))
                    .ToList())
                {
                    foreach (var reservation in reservationList
                        .Where(reservation => reservation.CustomerId == customerId).ToList())
                    {
                        allCustomerReservations.Add(reservation);
                        reservationList.Remove(reservation);
                    }
                }

                foreach (var customerReservation in allCustomerReservations)
                {
                    var product = await TryGetProductWithinLock(customerReservation.ProductId);
                    if (product != null)
                    {
                        product.StoredUnits = Math.Max(0, product.StoredUnits - customerReservation.ReservedAmount);
                        productRepository.Update(product);
                    }
                }
            }
        }

        /// <summary>
        /// Performs Order of required product from distributor
        /// </summary>
        /// <param name="productId">id of product to Order</param>
        /// <param name="value">Number of units to Order</param>
        public async Task OrderProductFromDistributor(Guid productId, int value)
        {
            using (AvailableUnitsLock.Lock())
            {
                var product = (await productRepository.GetAsync(productId)) ??
                              throw new ArgumentException("ProductService - OrderProductFromDistributor(...): product ID does not belong to any stored product");
                if (value < 1)
                {
                    throw new ArgumentException("ProductService - OrderProductFromDistributor(...): Value must be greater than zero");
                }
                product.StoredUnits += value;
                productRepository.Update(product);
            }
        }

        /// <summary>
        /// Gets number of currently available (free)
        /// units for product
        /// </summary>
        /// <param name="productId">ID of the product for which product availability should be checked</param>
        /// <returns>Number of free units for this product</returns>
        public async Task<int> GetCurrentlyAvailableUnits(Guid productId)
        {
            ReleaseProductReservationsCore(reservation => DateTime.Now > reservation.Expiration);
            EnsureDictionaryRecordIsInitialized(productId);
            using (AvailableUnitsLock.Lock())
            {
                return await GetCurrentlyAvailableUnitsWithinLock(productId);
            }
        }
        

        /// <summary>
        /// Gets number of currently available (free)
        /// units for product
        /// </summary>
        /// <param name="productId">ID of the product for which product availability should be checked</param>
        /// <returns>Number of free units for this product</returns>
        private async Task<int> GetCurrentlyAvailableUnitsWithinLock(Guid productId)
        {
            var product = await TryGetProductWithinLock(productId);
            if (product== null)
            {
                return 0;
            }
            var storedUnits = product.StoredUnits;
            var reservedItems = ReservedProductUnits[productId].ToList();
            var reservedUnits = reservedItems.Sum(item => item.ReservedAmount);
            return storedUnits - reservedUnits;
        }
 
        /// <summary>
        /// Performs product the actual product reservation in Order to ensure
        /// simultaneous Orders can be satisfied properly
        /// </summary>
        /// <param name="productReservation">Product reservation details containing 
        /// customer reserving the product, amount, productID and expiration</param>
        /// <returns>true if reservation is successfull, otherwise false</returns>
        private async Task<bool> ReserveProductWithinLockCore(ProductReservationDto productReservation)
        {
            var freeUnits = await GetCurrentlyAvailableUnitsWithinLock(productReservation.ProductId);

            var customerReservation = ReservedProductUnits[productReservation.ProductId]
                .FirstOrDefault(item => item.CustomerId == productReservation.CustomerId);

            if (customerReservation != null)
            {
                // refresh expiration time
                customerReservation.Expiration = productReservation.Expiration;
                if (productReservation.ReservedAmount == customerReservation.ReservedAmount)
                {
                    return true;
                }
                if (productReservation.ReservedAmount > freeUnits + customerReservation.ReservedAmount)
                {
                    return false;
                }
                customerReservation.ReservedAmount = productReservation.ReservedAmount;
                return true;
            }
            if (productReservation.ReservedAmount > freeUnits)
            {
                return false;
            }
            ReservedProductUnits[productReservation.ProductId].Add(productReservation);
            return true;
        }

        /// <summary>
        /// Gets product according to ID.
        /// This method should be called within synchronised context.
        /// </summary>
        /// <param name="productId">id of the product</param>
        /// <param name="product">retrieved product</param>
        /// <returns>true if product retrieval was successfull</returns>
        private async Task<Product> TryGetProductWithinLock(Guid productId)
        {
            return await productRepository.GetAsync(productId);
        }

        /// <summary>
        /// Performs validation of product reservation.
        /// </summary>
        /// <param name="productReservation">Product reservation to validate</param>
        private static void CheckProductReservationIsValid(ProductReservationDto productReservation)
        {
            var context = new ValidationContext(productReservation);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(productReservation, context, results, true);
            if (isValid)
            {
                return;
            }
            var msgBuilder = new StringBuilder();
            foreach (var result in results)
            {
                msgBuilder.Append(result.ErrorMessage);
            }
            throw new ArgumentException($"ProductService - product reservation Dto is not valid ({msgBuilder}).");
        }

        /// <summary>
        /// Releases all expired reservation from ReservedProductUnits based on given predicate.
        /// </summary>
        /// <param name="deletePredicate">Predicate which decides whether respective reservation should be removed.</param>
        private static void ReleaseProductReservationsCore(Func<ProductReservationDto, bool> deletePredicate)
        {
            using (AvailableUnitsLock.Lock())
            {
                foreach (var reservationList in ReservedProductUnits.Values)
                {
                    foreach (var reservation in reservationList.ToArray())
                    {
                        if (deletePredicate(reservation))
                        {
                            reservationList.Remove(reservation);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates record within ReservedProductUnits if it does not exist (for the given productId) yet.
        /// </summary>
        /// <param name="productId">ID of the product to check.</param>
        private static void EnsureDictionaryRecordIsInitialized(Guid productId)
        {
            using (AvailableUnitsLock.Lock())
            {
                if (!ReservedProductUnits.ContainsKey(productId))
                {
                    ReservedProductUnits.Add(productId, new List<ProductReservationDto>());
                }
            }
        }
    }
}
