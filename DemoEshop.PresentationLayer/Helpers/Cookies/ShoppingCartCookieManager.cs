using System;
using System.Collections.Generic;
using System.Web;
using DemoEshop.BusinessLayer.DataTransferObjects;
using Newtonsoft.Json;

namespace DemoEshop.PresentationLayer.Helpers.Cookies
{
    /// <summary>
    /// Manages cookies used to store shopping cart items
    /// </summary>
    public static class ShoppingCartCookieManager
    {
        #region Constants

        private const string BaseCookieName = "ShoppingCart";

        private const string CookieValue = "ShoppingCartItems";

        public const int CookieExpirationInMinutes = 60;

        #endregion

        #region ShoppingItemsManagement

        /// <summary>
        /// Gets all shopping cart items form cookie within request according to given user username
        /// </summary>
        /// <param name="request">HTTP request containing shopping cart cookie for given user</param>
        /// <param name="customerUsername">Username of the cookie owner</param>
        /// <returns>List of current shopping cart items</returns>
        public static IList<RateSongDto> GetAllShoppingCartItems(this HttpRequestBase request, string customerUsername)
        {
            var cookie = GetShoppingCartCookie(request, customerUsername);
            if (cookie == null)
            {
                return new List<RateSongDto>();
            }
            var shoppingCartItemsJson = cookie.Values[CookieValue];
            var items = Deserialize<List<RateSongDto>>(shoppingCartItemsJson);
            return items ?? new List<RateSongDto>();
        }

        /// <summary>
        /// Saves given shopping cart items to corresponding cookie
        /// </summary>
        /// <param name="response">HTTP response to save the shopping cart items cookie to</param>
        /// <param name="shoppingCartItems">shopping cart items to save</param>
        /// <param name="customerUsername">Username of the cookie owner</param>
        public static void SaveAllShoppingCartItems(this HttpResponseBase response, string customerUsername, IEnumerable<RateSongDto> shoppingCartItems = null)
        {
            ShrinkShoppingCartItem(shoppingCartItems);
            var shoppingCartItemsJson = Serialize(shoppingCartItems ?? new List<RateSongDto>());
            var cookie = GetShoppingCartCookie(response, customerUsername);
            cookie.Expires = DateTime.Now.AddMinutes(CookieExpirationInMinutes);
            cookie[CookieValue] = shoppingCartItemsJson;
        }

        /// <summary>
        /// Clears all shopping cart items
        /// </summary>
        /// <param name="response">HTTP response to save the shopping cart items cookie to</param>
        /// <param name="customerUsername">Username of the cookie owner</param>
        public static void ClearAllShoppingCartItems(this HttpResponseBase response, string customerUsername)
        {
            // Saving null collection will cause shopping cart items cookie clearing
            response.SaveAllShoppingCartItems(customerUsername);
        }

        /// <summary>
        /// Reduces shopping item size, so it does not take much space within the cookie
        /// </summary>
        /// <param name="shoppingCartItems">Shopping cart items to reduce</param>
        private static void ShrinkShoppingCartItem(IEnumerable<RateSongDto> shoppingCartItems)
        {
            foreach (var shoppingCartItem in shoppingCartItems ?? new List<RateSongDto>())
            {
                shoppingCartItem.Product.Description = "";
            }
        }

        #endregion

        #region CookieManagement

        /// <summary>
        /// Creates new ShoppingCartCookie for the given HTTP response
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <param name="customerUsername">Username of the cookie owner</param>
        private static void CreateEshopCartCookieCore(this HttpResponseBase response, string customerUsername)
        {  
            var cookieName = BaseCookieName + customerUsername;
            var cookie = new HttpCookie(cookieName)
            {
                Expires = DateTime.Now.AddMinutes(CookieExpirationInMinutes)
            };
            cookie.Values[CookieValue] = Serialize(new List<RateSongDto>());
            response.Cookies.Add(cookie);
        }

        #endregion

        #region CookieRetrieval

        /// <summary>
        /// Gets Cookie from request
        /// </summary>
        /// <param name="request">HTTP request containing shopping cart cookie for given user</param>
        /// <param name="customerUsername">Username of the cookie owner</param>
        /// <returns>Corresponding cookie</returns>
        private static HttpCookie GetShoppingCartCookie(this HttpRequestBase request, string customerUsername)
        {
            if (string.IsNullOrEmpty(customerUsername))
            {
                throw new ArgumentException("Customer username cant be null or empty");
            }
            return request.Cookies[BaseCookieName + customerUsername];
        }

        /// <summary>
        /// Gets Cookie from response
        /// </summary>
        /// <param name="response">HTTP response containing shopping cart cookie for given user</param>
        /// <param name="customerUsername">Username of the cookie owner</param>
        /// <returns>Corresponding cookie</returns>
        private static HttpCookie GetShoppingCartCookie(this HttpResponseBase response, string customerUsername)
        {
            if (string.IsNullOrEmpty(customerUsername))
            {
                throw new ArgumentException("Customer username cant be null or empty");
            }
            var cookie = response.Cookies[BaseCookieName + customerUsername];
            if (cookie == null)
            {
                CreateEshopCartCookieCore(response, customerUsername);
                return response.Cookies[BaseCookieName + customerUsername];
            }
            return cookie;
        }

        #endregion

        #region Serialization

        private static string Serialize<T>(T data) where T : class
        {
            return JsonConvert.SerializeObject(data);
        }

        private static T Deserialize<T>(string data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(data);
        } 

        #endregion
    }
}