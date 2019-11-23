using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;
using DemoEshop.BusinessLayer.Services.Checkout.PriceCalculators;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;

namespace DemoEshop.BusinessLayer.Services.Checkout
{
    public class CheckoutService : ServiceBase, ICheckoutService
    {
        private readonly IRepository<Rating> OrderRepository;
        private readonly IRepository<RateSong> RateSongRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Product> productRepository;

        /// <summary>
        /// Registered price calculators
        /// </summary>
        private readonly IEnumerable<IPriceCalculator> priceCalculators;

        public CheckoutService(IMapper mapper, IEnumerable<IPriceCalculator> priceCalculators, IRepository<Rating> OrderRepository,
            IRepository<RateSong> RateSongRepository, IRepository<Customer> customerRepository, IRepository<Product> productRepository) 
            : base(mapper)
        {
            this.OrderRepository = OrderRepository;
            this.RateSongRepository = RateSongRepository;
            this.customerRepository = customerRepository;
            this.productRepository = productRepository;
            this.priceCalculators = priceCalculators;
        }

        /// <summary>
        /// Persists Order together with all related data
        /// </summary>
        /// <param name="createRatingDto">wrapper for Order, RateSongs, customer and coupon</param>
        public async Task<Guid> ConfirmOrderAsync(OrderCreateDto createRatingDto)
        {
            var Order = Mapper.Map<Rating>(createRatingDto.RatingDto);

            var customer = await customerRepository.GetAsync(createRatingDto.RatingDto.CustomerId);
            Order.Customer = customer ?? throw new ArgumentException("CheckoutService - CreateOrder(...) Customer must not be null");
            
            OrderRepository.Create(Order);

            foreach (var RateSongDto in createRatingDto.RateSongs)
            {
                var RateSong = Mapper.Map<RateSong>(RateSongDto);
                RateSong.Product = await productRepository.GetAsync(RateSongDto.Product.Id);
                RateSong.RatingId = Order.Id;
                RateSong.Rating = Order;
                RateSongRepository.Create(RateSong);
            }
            return Order.Id;
            /*  RequestOrderDelivery - for instance a request to https://www.easypost.com/dhl-express-api 
                can be made, corresponding warehouse receives a notification about
                delivery request and schedules delivery
            */
        }

        /// <summary>
        /// Calculates total price for all Order items
        /// </summary>
        /// <param name="RateSongs">all Order items</param>
        /// <returns>Total price for given items</returns>
        public decimal CalculateTotalPrice(ICollection<RateSongDto> RateSongs)
        {
            return RateSongs?.Sum(RateSong => ResolvePriceCalculator(RateSong.Product.DiscountType)
                       .CalculatePrice(RateSong)) ?? 0;
        }
        
        private IPriceCalculator ResolvePriceCalculator(Role discountType)
        {
            return priceCalculators.First(calculator => calculator.DiscountType.Equals(discountType));
        }
    }
}
