using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Services.Categories
{
    public class CategoryService : CrudQueryServiceBase<Category, CategoryDto, CategoryFilterDto>, ICategoryService
    {
        public CategoryService(IMapper mapper, IRepository<Category> categoryRepository, QueryObjectBase<CategoryDto, Category, CategoryFilterDto, IQuery<Category>> categoryListQuery)
            : base(mapper, categoryRepository, categoryListQuery) { }
        
        protected override async Task<Category> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId, nameof(Category.Parent));
        }
        
        /// <summary>
        /// Gets ids of the categories with the corresponding names
        /// </summary>
        /// <param name="names">names of the categories</param>
        /// <returns>ids of categories with specified name</returns>
        public async Task<Guid[]> GetCategoryIdsByNamesAsync(params string[] names)
        {
            var queryResult = await Query.ExecuteQuery(new CategoryFilterDto { Names = names });
            return queryResult.Items.Select(category => category.Id).ToArray();
        }

        /// <summary>
        /// Gets all parent categories for specified category
        /// </summary>
        /// <param name="categoryId">category id</param>
        /// <returns>all parent categories</returns>
        public async Task<IEnumerable<CategoryDto>> GetCategoryPathAsync(Guid categoryId)
        {
            var list = new List<CategoryDto>();
            var category = await Repository.GetAsync(categoryId);
            list.Add(Mapper.Map<CategoryDto>(category));

            while (category.HasParent)
            {
                list.Add(Mapper.Map<CategoryDto>(Repository.GetAsync(category.Parent.Id)));
                category = category.Parent;
            }
            return list;
        }
    }
}
