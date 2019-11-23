using System;
using System.Threading.Tasks;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.UnitOfWork;
using NUnit.Framework;

namespace DemoEshop.DataAccessLayer.EntityFramework.Tests.RepositoryTests
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private readonly IUnitOfWorkProvider unitOfWorkProvider = Initializer.Container.Resolve<IUnitOfWorkProvider>();

        private readonly IRepository<Album> categoryRepository = Initializer.Container.Resolve<IRepository<Album>>();

        private readonly Guid smartphonesCategoryId = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid androidCategoryId = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid windows10MobileCategoryId = Guid.Parse("aa03dc64-5c07-40fe-a916-175165b9b90f");

        [Test]
        public async Task GetCategoryAsync_AlreadyStoredInDBNoIncludes_ReturnsCorrectCategory()
        {
            // Arrange
            Album androidCategory;

            // Act
            using (unitOfWorkProvider.Create())
            {
                androidCategory = await categoryRepository.GetAsync(androidCategoryId);
            }

            // Assert
            Assert.AreEqual(androidCategory.Id, androidCategoryId);
        }

        [Test]
        public async Task GetCategoryAsync_AlreadyStoredInDBWithIncludes_ReturnsCorrectCategoryWithInitializedParent()
        {
            Album androidCategory;

            using (unitOfWorkProvider.Create())
            {
                androidCategory = await categoryRepository.GetAsync(androidCategoryId, nameof(Album.Parent));
            }

            Assert.IsTrue(androidCategory.Id.Equals(androidCategoryId) && androidCategory.Parent.Id.Equals(smartphonesCategoryId));
        }

        [Test]
        public async Task CreateCategoryAsync_CategoryIsNotPreviouslySeeded_CreatesNewCategory()
        {
            var windows10Mobile = new Album { Name = "Windows 10", ParentId = smartphonesCategoryId };

            using (var uow = unitOfWorkProvider.Create())
            {
                categoryRepository.Create(windows10Mobile);
                await uow.Commit();
                
            }
            Assert.IsTrue(!windows10Mobile.Id.Equals(Guid.Empty));
        }

        [Test]
        public async Task UpdateCategoryAsync_CategoryIsPreviouslySeeded_UpdatesCategory()
        {
            Album updatedAndroidCategory;
            var newAndroidCategory = new Album { Id = androidCategoryId, Name = "Updated Name", ParentId = null };

            using (var uow = unitOfWorkProvider.Create())
            {
                categoryRepository.Update(newAndroidCategory);
                await uow.Commit();
                updatedAndroidCategory = await categoryRepository.GetAsync(androidCategoryId);
            }

            Assert.IsTrue(newAndroidCategory.Name.Equals(updatedAndroidCategory.Name) && newAndroidCategory.ParentId.Equals(null));
        }

        [Test]
        public async Task DeleteCategoryAsync_CategoryIsPreviouslySeeded_DeletesCategory()
        {
            Album deletedAndroidCategory;

            using (var uow = unitOfWorkProvider.Create())
            {
                categoryRepository.Delete(androidCategoryId);
                await uow.Commit();
                deletedAndroidCategory = await categoryRepository.GetAsync(androidCategoryId);
            }

            Assert.AreEqual(deletedAndroidCategory, null);
        }
    }
}
