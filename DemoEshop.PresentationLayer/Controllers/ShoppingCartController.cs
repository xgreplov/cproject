using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.PresentationLayer.Helpers.Attributes;
using DemoEshop.PresentationLayer.Helpers.Cookies;
using DemoEshop.PresentationLayer.Models.ShoppingCart;

namespace DemoEshop.PresentationLayer.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        #region ConfigurationProperties

        private const int DefaultShoppingItemValue = 1;

        private DateTime RefreshedExpiration => DateTime.Now.AddMinutes(ShoppingCartCookieManager.CookieExpirationInMinutes); 
        
        #endregion

        #region Facades

        public CustomerFacade CustomerFacade { get; set; }

        public OrderFacade OrderFacade { get; set; }

        public ProductFacade ProductFacade { get; set; }

        #endregion
        
        #region ShoppingCartItemsListActionMethods

        public async Task<ActionResult> Index()
        {
            var model = await CreateShoppingCartViewModel();
            return View("ShoppingItemsListView", model);
        }

        public async Task<ActionResult> AddItem(Guid id)
        {
            var product = await ProductFacade.GetProductAsync(id);
            var model = await CreateShoppingCartViewModel();

            // if item for the requested product is already in the shopping cart, take no action  
            if (model.ShoppingCartItems.Any(item => item.Product.Equals(product)))
            {
                return RedirectToAction("Index");
            }

            if (await TryReserveStock(model.Customer, product))
            {
                model.ShoppingCartItems.Add(new RateSongDto
                {
                    Product = product,
                    Value = DefaultShoppingItemValue
                });
                Response.SaveAllShoppingCartItems(User.Identity.Name, model.ShoppingCartItems);
            }

            return RedirectToAction("Index");
        }
    
        public async Task<ActionResult> ClearAllItems()
        {
            var customer = await CustomerFacade.GetCustomerAccordingToUsernameAsync(User.Identity.Name);
            Response.ClearAllShoppingCartItems(User.Identity.Name);
            OrderFacade.ReleaseReservations(customer.Id);
            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "SaveAndContinueShopping")]
        public async Task<ActionResult> SaveAndContinueShopping(ShoppingCartViewModel model)
        {
            await SaveAllShoppingCartItems(model);
            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "Proceed")]
        public async Task<ActionResult> Proceed(ShoppingCartViewModel model)
        {
            await SaveAllShoppingCartItems(model);
            var newModel = await CreateShoppingCartViewModel(model.ShoppingCartItems);
            return View("ShoppingCartCheckoutView", newModel);
        }

        #endregion

        #region CheckoutActionMethods

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "ConfirmOrder")]
        public async Task<ActionResult> ConfirmOrder(ShoppingCartViewModel model)
        {
            var createRatingDto = await CreateOrderCreateViewModel();

            await OrderFacade.ConfirmOrderAsync(createRatingDto, () => Response.ClearAllShoppingCartItems(User.Identity.Name));

            var OrderConfirmModel = await CreateOrderConfirmationViewModel(createRatingDto);

            return View("OrderConfirmation", OrderConfirmModel);
        }

        #endregion

        #region HelperMethods

        private async Task SaveAllShoppingCartItems(ShoppingCartViewModel model)
        {
            var customer = await CustomerFacade.GetCustomerAccordingToUsernameAsync(User.Identity.Name);
            foreach (var item in model.ShoppingCartItems)
            {
                if (item.Value == 0)
                {
                    OrderFacade.ReleaseReservations(customer.Id, item.Product.Id);
                    continue;
                }

                if (!await TryReserveStock(customer, item.Product, item.Value))
                {
                    await RetryStockReservation(item, customer);
                }   
            }
            var itemsToSave = model.ShoppingCartItems.Where(item => item.Value > 0);
            Response.SaveAllShoppingCartItems(User.Identity.Name, itemsToSave);
        }

        #region StockReservationMethods

        private async Task<bool> TryReserveStock(CustomerDto customer, ProductDto product, int? desiredValue = null)
        {
            var reservation = new ProductReservationDto
            {
                CustomerId = customer.Id,
                ProductId = product.Id,
                ReservedAmount = desiredValue ?? DefaultShoppingItemValue,
                Expiration = RefreshedExpiration
            };
            var isReservationSuccessfull = await OrderFacade.ReserveProductAsync(reservation);
            return isReservationSuccessfull;
        }

        private async Task RetryStockReservation(RateSongDto item, CustomerDto customer)
        {
            var maxAvailableUnits = await OrderFacade.GetCurrentlyAvailableUnitsAsync(item.Product.Id);
            if (maxAvailableUnits > 0)
            {
                item.Value = maxAvailableUnits;
                await TryReserveStock(customer, item.Product, maxAvailableUnits);
            }
        }

        #endregion

        #region ViewModelCreationMethods

        /// <summary>
        /// Creates ShoppingCartViewModel
        /// </summary>
        /// <param name="shoppingCartItems">Items in the shopping cart</param>
        /// <returns>ShoppingCartViewModel</returns>
        private async Task<ShoppingCartViewModel> CreateShoppingCartViewModel(IList<RateSongDto> shoppingCartItems = null)
        {
            return new ShoppingCartViewModel
            {
                Customer = await CustomerFacade.GetCustomerAccordingToUsernameAsync(User.Identity.Name),
                ShoppingCartItems = shoppingCartItems ?? Request.GetAllShoppingCartItems(User.Identity.Name),
                TotalPrice = OrderFacade.CalculateTotalPrice(shoppingCartItems)
            };
        }

        /// <summary>
        /// Creates OrderConfirmationViewModel
        /// </summary>
        /// <param name="createRatingDto">ViewModel containing data about Order creation</param>
        /// <returns>OrderConfirmationViewModel</returns>
        private async Task<OrderConfirmationViewModel> CreateOrderConfirmationViewModel(OrderCreateDto createRatingDto)
        {
            return new OrderConfirmationViewModel
            {
                Customer = await CustomerFacade.GetCustomerAccordingToUsernameAsync(User.Identity.Name),
                Order = createRatingDto.RatingDto
            };
        }

        /// <summary>
        /// Creates OrderCreateViewModel
        /// </summary>
        /// <returns>OrderCreateViewModel</returns>
        private async Task<OrderCreateDto> CreateOrderCreateViewModel()
        {
            var newModel = await CreateShoppingCartViewModel();
            
            var Order = new RatingDto
            {
                CustomerId = newModel.Customer.Id,
                Issued = DateTime.Now,
                TotalPrice = OrderFacade.CalculateTotalPrice(newModel.ShoppingCartItems)
            };

            return new OrderCreateDto
            {
                RatingDto = Order,
                RateSongs = newModel.ShoppingCartItems
            };
        }

        #endregion

        #endregion
    }
}
