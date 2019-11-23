using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Checkout;
using DemoEshop.BusinessLayer.Services.Checkout.PriceCalculators;
using DemoEshop.BusinessLayer.Services.Reservation;
using DemoEshop.BusinessLayer.Services.Sales;
using DemoEshop.BusinessLayer.Tests.FacadesTests.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;
using Moq;
using NUnit.Framework;

namespace DemoEshop.BusinessLayer.Tests.FacadesTests
{
    [TestFixture]
    public class OrderFacadeTests
    {
        [Test]
        public async Task ConfirmOrderAsync_SingleReservation_ExecutesCorrectly()
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            const int storedAmount = 10;
            const int reservedAmount = 3;
            var returnedProduct = new Song{ Id = productId, StoredUnits = storedAmount };
            var returnedProductDto = new ProductDto { Id = productId, StoredUnits = storedAmount };
            var OrderCreateDto = new OrderCreateDto
            {
                RatingDto = new RatingDto { Id = Guid.NewGuid(), CustomerId = customerId, Issued = DateTime.Now, TotalPrice = 8000},
                RateSongs = new List<RateSongDto>
                {
                    new RateSongDto{Id = Guid.NewGuid(), Product = returnedProductDto, Value = reservedAmount}
                }
            };
            var mockManager = new FacadeMockManager();
            var productRepositoryMock = mockManager.ConfigureGetAndUpdateRepositoryMock(returnedProduct, nameof(Song.StoredUnits));
            var customerRepositoryMock = mockManager.ConfigureGetRepositoryMock(new Artist{Id = customerId});
            var OrderRepositoryMock = mockManager.ConfigureCreateRepositoryMock<Rating>(nameof(Rating.CustomerId));
            var RateSongRepositoryMock = mockManager.ConfigureRepositoryMock<RateSong>();
            var OrderQueryMock = mockManager.ConfigureQueryObjectMock<RatingDto, Rating, RatingFilterDto>(null);
            var RateSongQueryMock = mockManager.ConfigureQueryObjectMock<RateSongDto, RateSong, RateSongFilterDto>(null);
            var reservationService = new ReservationService(FacadeMockManager.ConfigureRealMapper(), productRepositoryMock.Object);
            await reservationService.ReserveProduct(new ProductReservationDto
            {
                CustomerId = customerId,
                ProductId = productId,
                ReservedAmount = reservedAmount,
                Expiration = DateTime.Now.Add(new TimeSpan(1, 0, 0))
            });

            var OrderFacade = CreateOrderFacade(productRepositoryMock, RateSongQueryMock, OrderQueryMock, OrderRepositoryMock, RateSongRepositoryMock, customerRepositoryMock, reservationService);

            OrderFacade.ConfirmOrderAsync(OrderCreateDto, () => Debug.WriteLine("Order confirmed.")).Wait();

            Assert.AreEqual(storedAmount - reservedAmount, mockManager.CapturedUpdatedStoredUnits);
            Assert.AreEqual(customerId, mockManager.CapturedCreatedId);
        }

        [Test]
        public void ConfirmOrderAsync_MultipleConcurrentReservations_ExecutesCorrectly()
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            const int storedAmount = 100;
            const int reservedAmount = 1;
            const int numberOfReservations = 10;
            var returnedProduct = new Song { Id = productId, StoredUnits = storedAmount };
            var returnedProductDto = new ProductDto { Id = productId, StoredUnits = storedAmount };
            var OrderCreateDto = new OrderCreateDto
            {
                RatingDto = new RatingDto { Id = Guid.NewGuid(), CustomerId = customerId, Issued = DateTime.Now, TotalPrice = 8000 },
                RateSongs = new List<RateSongDto>
                {
                    new RateSongDto{Id = Guid.NewGuid(), Product = returnedProductDto, Value = reservedAmount}
                }
            };
            
            var mockManager = new FacadeMockManager();
            var productRepositoryMock = mockManager.ConfigureGetAndUpdateRepositoryMock(returnedProduct, nameof(Song.StoredUnits));
            var customerRepositoryMock = mockManager.ConfigureGetRepositoryMock(new Artist { Id = customerId });
            var OrderRepositoryMock = mockManager.ConfigureCreateRepositoryMock<Rating>(nameof(Rating.CustomerId));
            var RateSongRepositoryMock = mockManager.ConfigureRepositoryMock<RateSong>();
            var OrderQueryMock = mockManager.ConfigureQueryObjectMock<RatingDto, Rating, RatingFilterDto>(null);
            var RateSongQueryMock = mockManager.ConfigureQueryObjectMock<RateSongDto, RateSong, RateSongFilterDto>(null);
            var reservationService = new ReservationService(FacadeMockManager.ConfigureRealMapper(), productRepositoryMock.Object);

            async Task ReservationAction(Guid cId, int reservedUnits) => await reservationService.ReserveProduct(new ProductReservationDto
            {
                CustomerId = cId,
                ProductId = productId,
                ReservedAmount = reservedUnits,
                Expiration = DateTime.Now.Add(new TimeSpan(1, 0, 0))
            });

            var tasks = new Task[10];
            for (var i = 0; i < numberOfReservations - 1; i++)
            {
                var amount = i;
                tasks[i] = Task.Run(() => ReservationAction(Guid.NewGuid(), amount));
            }
            tasks[numberOfReservations - 1] = Task.Run(() => ReservationAction(customerId, reservedAmount));
            Task.WaitAll(tasks);
            
            var OrderFacade = CreateOrderFacade(productRepositoryMock, RateSongQueryMock, OrderQueryMock, OrderRepositoryMock, RateSongRepositoryMock, customerRepositoryMock, reservationService);

            OrderFacade.ConfirmOrderAsync(OrderCreateDto, () => Debug.WriteLine("Order confirmed.")).Wait();

            Assert.AreEqual(storedAmount - reservedAmount, mockManager.CapturedUpdatedStoredUnits);
            Assert.AreEqual(customerId, mockManager.CapturedCreatedId);
        }

        [TestCase(2000, 3000, 0, 0, 2, 3)]
        [TestCase(1000,1000,10,0,10,1)]
        public void CalculateTotalPrice_HavingMultipleRateSongs_ReturnsCorrectResult(int firstProductPrice, int secondProductPrice, int firstDiscountPercentage, int secondDiscountPercentage, int firstProductValue, int secondProductValue)
        {
            var RateSongs = new List<RateSongDto>
            {
                new RateSongDto{Product = new ProductDto{DiscountPercentage = firstDiscountPercentage, Price = firstProductPrice}, Value = firstProductValue},
                new RateSongDto{Product = new ProductDto{DiscountPercentage = secondDiscountPercentage, Price = secondProductPrice}, Value = secondProductValue}
            };

            var mockManager = new FacadeMockManager();
            var productRepositoryMock = mockManager.ConfigureRepositoryMock<Song>();
            var customerRepositoryMock = mockManager.ConfigureRepositoryMock<Artist>();
            var OrderRepositoryMock = mockManager.ConfigureRepositoryMock<Rating>();
            var RateSongRepositoryMock = mockManager.ConfigureRepositoryMock<RateSong>();
            var OrderQueryMock = mockManager.ConfigureQueryObjectMock<RatingDto, Rating, RatingFilterDto>(null);
            var RateSongQueryMock = mockManager.ConfigureQueryObjectMock<RateSongDto, RateSong, RateSongFilterDto>(null);

            var OrderFacade = CreateOrderFacade(productRepositoryMock, RateSongQueryMock, OrderQueryMock, OrderRepositoryMock, RateSongRepositoryMock, customerRepositoryMock);

            var totalPrice = OrderFacade.CalculateTotalPrice(RateSongs);
            decimal CalculatePriceForRateSong(int price, int discount, int value)
            {
                return (decimal) (price * (discount > 0 ? 1 - (discount / 100.0) : 1) * value);
            }

            var expectedPrice = CalculatePriceForRateSong(firstProductPrice, firstDiscountPercentage, firstProductValue) +
                                CalculatePriceForRateSong(secondProductPrice, secondDiscountPercentage, secondProductValue);
            Assert.AreEqual(expectedPrice, totalPrice);
        }


        [Test]
        public async Task GetCurrentlyAvailableUnits_WithNoReservations_ReturnsCorrectResult()
        {
            const int storedUnits = 3;
            var productId = Guid.NewGuid();
            var productRepositoryMock = new FacadeMockManager().ConfigureGetRepositoryMock(new Song { StoredUnits = storedUnits });
            var productFacade = CreateOrderFacadeForReservationTesting(productRepositoryMock);
            var actualAvaiableUnits = await productFacade.GetCurrentlyAvailableUnitsAsync(productId);
            Assert.AreEqual(storedUnits, actualAvaiableUnits);
        }

        [TestCase(1, 3, 1)]
        [TestCase(3, 3, 3)]
        [TestCase(3, 1, 0)]
        public async Task ReserveProduct_SingleReservation_ReturnsCorrectResult(int reservedAmount, int storedUnits, int expectedReservedAmount)
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var productReservation = new ProductReservationDto
            {
                CustomerId = customerId,
                ProductId = productId,
                Expiration = DateTime.Now.Add(new TimeSpan(1,0,0)),
                ReservedAmount = reservedAmount
            };
            var productRepositoryMock = new FacadeMockManager().ConfigureGetRepositoryMock(new Song{StoredUnits = storedUnits });
            var productFacade = CreateOrderFacadeForReservationTesting(productRepositoryMock);
            await productFacade.ReserveProductAsync(productReservation);
            var actualReservedUnits = storedUnits - await productFacade.GetCurrentlyAvailableUnitsAsync(productId);
            Assert.AreEqual(expectedReservedAmount, actualReservedUnits);
        }

        [Test]
        public async Task ReleaseReservations_SingleReservation_ReturnsCorrectResult()
        {
            const int reservedAmount = 1;
            const int storedUnits = 3;
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var productReservation = new ProductReservationDto
            {
                CustomerId = customerId,
                ProductId = productId,
                Expiration = DateTime.Now.Add(new TimeSpan(1, 0, 0)),
                ReservedAmount = reservedAmount
            };
            var productRepositoryMock = new FacadeMockManager().ConfigureGetRepositoryMock(new Song { StoredUnits = storedUnits });
            var productFacade = CreateOrderFacadeForReservationTesting(productRepositoryMock);

            await productFacade.ReserveProductAsync(productReservation);
            productFacade.ReleaseReservations(customerId);

            var actualReservedUnits = storedUnits - await productFacade.GetCurrentlyAvailableUnitsAsync(productId);
            Assert.AreEqual(0, actualReservedUnits);
        }

        [Test]
        public async Task OrderProductFromDistributor_Condition_ReturnsCorrectResult()
        {
            const int OrderedAmount = 3;
            const int storedUnits = 7;
            var productId = Guid.NewGuid();
            var productRepositoryMock = new FacadeMockManager().ConfigureGetRepositoryMock(new Song { StoredUnits = storedUnits });
            var productFacade = CreateOrderFacadeForReservationTesting(productRepositoryMock);

            await productFacade.OrderProductFromDistributorAsync(productId, OrderedAmount);
            var actualStoredUnits = await productFacade.GetCurrentlyAvailableUnitsAsync(productId);

            Assert.AreEqual(storedUnits + OrderedAmount, actualStoredUnits);
        }

        private static OrderFacade CreateOrderFacadeForReservationTesting(Mock<IRepository<Song>> productRepositoryMock = null)
        {
            var uowMock = FacadeMockManager.ConfigureUowMock();
            var mapper = FacadeMockManager.ConfigureRealMapper();
            var reservationService = new ReservationService(mapper, productRepositoryMock?.Object ?? new Mock<IRepository<Song>>().Object);
            var OrderFacade = new OrderFacade(uowMock.Object, null, null, reservationService);
            return OrderFacade;
        }

        private static OrderFacade CreateOrderFacade(Mock<IRepository<Song>> productRepositoryMock, Mock<QueryObjectBase<RateSongDto, RateSong, RateSongFilterDto, IQuery<RateSong>>> RateSongQueryMock, Mock<QueryObjectBase<RatingDto, Rating, RatingFilterDto, IQuery<Rating>>> OrderQueryMock, Mock<IRepository<Rating>> OrderRepositoryMock, Mock<IRepository<RateSong>> RateSongRepositoryMock, Mock<IRepository<Artist>> customerRepositoryMock, ReservationService productReservationService = null)
        {
            var uowMock = FacadeMockManager.ConfigureUowMock();
            var mapper = FacadeMockManager.ConfigureRealMapper();
            var salesService = new SalesService(mapper, RateSongQueryMock.Object, OrderQueryMock.Object);
            var checkoutService = new CheckoutService(mapper,new List<IPriceCalculator>{new PercentagePriceCalculator(), new Value3Plus1PriceCalculator()}, OrderRepositoryMock.Object, RateSongRepositoryMock.Object, customerRepositoryMock.Object, productRepositoryMock.Object);
            var reservationService = productReservationService ?? new ReservationService(mapper, productRepositoryMock.Object);
            var OrderFacade = new OrderFacade(uowMock.Object, checkoutService, salesService, reservationService);
            return OrderFacade;
        }
    }
}
