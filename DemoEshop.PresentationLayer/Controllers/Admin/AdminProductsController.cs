using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.PresentationLayer.Models.AdminProducts;
using X.PagedList;

namespace DemoEshop.PresentationLayer.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : Controller
    {
        public ProductFacade ProductFacade { get; set; }
        public OrderFacade OrderFacade { get; set; }

        // GET: AdminProducts
        public async Task<ActionResult> Index(int page = 1)
        {
            //TODO: very inefficient, why and how could we solve this?
            var result = await ProductFacade.GetProductsAsync(new ProductFilterDto());
            var pageSize = result.TotalItemsCount > 0 ? (int)result.TotalItemsCount : 1;
            var model = new StaticPagedList<ProductDto>(result.Items, page, pageSize, (int)result.TotalItemsCount);
            return View(model);
        }

        // GET: AdminProducts/Details/<Guid>
        public async Task<ActionResult> Details(Guid id)
        {
            var model = await ProductFacade.GetProductAsync(id);         
            return View(model);
        }
        
        // GET: AdminProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminProducts/Create
        [HttpPost]
        public async Task<ActionResult> Create(AdminProductEditModel model)
        {
            try
            {
                await ProductFacade.CreateProductWithCategoryNameAsync(model.Product, model.Category.Name);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminProducts/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            var product = await ProductFacade.GetProductAsync(id);
            var category = await ProductFacade.GetCategoryAsync(product.CategoryId);
            var model = new AdminProductEditModel
            {
                Product = product,
                Category = category
            };
            return View(model);
        }

        // POST: AdminProducts/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, AdminProductEditModel model)
        {
            try
            {
                model.Product.CategoryId = (await ProductFacade.GetProductCategoryIdsByNamesAsync(model.Category.Name)).SingleOrDefault();
                await ProductFacade.EditProductAsync(model.Product);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminProducts/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            await ProductFacade.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> OrderProduct(Guid id, int amount)
        {
            await OrderFacade.OrderProductFromDistributorAsync(id, amount);
            return RedirectToAction("Details", new { id });
        }
    }
}
