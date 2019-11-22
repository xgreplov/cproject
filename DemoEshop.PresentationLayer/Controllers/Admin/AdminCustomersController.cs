using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using X.PagedList;

namespace DemoEshop.PresentationLayer.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCustomersController : Controller
    {
        public CustomerFacade CustomerFacade { get; set; }
        public OrderFacade OrderFacade { get; set; }

        public async Task<ActionResult> Index(int page = 1)
        {
            //TODO: very inefficient, why and how could we solve this?
            var result = await CustomerFacade.GetAllCustomersAsync();
            var pageSize = result.TotalItemsCount > 0 ? (int)result.TotalItemsCount : 1;
            var model = new StaticPagedList<CustomerDto>(result.Items, page, pageSize, (int)result.TotalItemsCount);
            return View(model);
        }

        public async Task<ActionResult> Orders(Guid id)
        {
            var Orders = await OrderFacade.GetOrdersAsync(new RatingFilterDto{ CustomerId = id });
            return View(Orders);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var RateSongs = await OrderFacade.ListItemsFromOrderAsync(new RateSongFilterDto{ RateSongId = id });
            return View(RateSongs);
        }
    }
}
