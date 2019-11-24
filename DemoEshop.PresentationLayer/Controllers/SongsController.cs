using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.PresentationLayer.Models.Songs;
using X.PagedList;

namespace DemoEshop.PresentationLayer.Controllers
{
    public class SongsController : Controller
    {
        #region SessionKey constants

        public const int PageSize = 9;

        private const string FilterSessionKey = "filter";
        private const string AlbumTreesSessionKey = "albumTrees";

        #endregion

        #region Facades

        public SongFacade SongFacade { get; set; }

        #endregion

        #region ProductsActionMethods

        public async Task<ActionResult> Index(int page = 1)
        {
            var filter = Session[FilterSessionKey] as SongFilterDto ?? new SongFilterDto{ PageSize = PageSize};
            filter.RequestedPageNumber = page;

            //TODO: This is soo much inefficient, why and how could we solve this?
            var allSongs = await SongFacade.GetSongsAsync(new SongFilterDto());
            var result = await SongFacade.GetSongsAsync(filter);

            var albumTrees = Session[AlbumTreesSessionKey] as IList<AlbumDto>;
            var model = await InitializeSongListViewModel(result, (int)allSongs.TotalItemsCount, albumTrees);
            return View("SongListView", model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(SongListViewModel model)
        {
            model.Filter.PageSize = PageSize;
            model.Filter.AlbumIds = ProcessAlbumIds(model);
            Session[FilterSessionKey] = model.Filter;
            Session[AlbumTreesSessionKey] = model.Albums;

            //TODO: This is soo much inefficient, why and how could we solve this?
            var allSongs = await SongFacade.GetSongsAsync(new SongFilterDto());
            var result = await SongFacade.GetSongsAsync(model.Filter);
            var newModel = await InitializeSongListViewModel(result, (int)allSongs.TotalItemsCount, model.Albums);
            return View("SongListView", newModel);
        }

        public ActionResult ClearFilter()
        {
            Session[FilterSessionKey] = null;
            Session[AlbumTreesSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var model = await SongFacade.GetSongAsync(id);
            return View("SongDetailView", model);
        }

        #endregion

        #region Helper methods

        private async Task<SongListViewModel> InitializeSongListViewModel(QueryResultDto<SongDto, SongFilterDto> result, int totalItemsCount, IList<AlbumDto> albums = null)
        {
            return new SongListViewModel
            {
                Songs = new StaticPagedList<SongDto>(result.Items, result.RequestedPageNumber ?? 1, PageSize, totalItemsCount),
                Albums = albums  ?? await SongFacade.GetAllAlbums() as IList<AlbumDto>,
                Filter = result.Filter
            };
        }

        private static Guid[] ProcessAlbumIds(SongListViewModel model)
        {
            var selectedAlbumIds = new List<Guid>();
            foreach (var song in model.Songs)
            {
                foreach (var album in model.Albums)
                {
                    if (song.AlbumId == album.Id)
                    {
                        selectedAlbumIds.Add(album.Id);
                    }
                }
            }
            return selectedAlbumIds.ToArray();
        }

        #endregion

    }
}
