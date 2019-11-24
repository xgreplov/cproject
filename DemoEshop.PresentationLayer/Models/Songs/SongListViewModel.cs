using System.Collections.Generic;
using System.Web.Mvc;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using X.PagedList;

namespace DemoEshop.PresentationLayer.Models.Songs
{
    public class SongListViewModel
    {
        public string[] SongSortCriteria => new[]{nameof(SongDto.Name), nameof(SongDto.Album) };

        public IList<AlbumDto> Albums { get; set; }

        public IPagedList<SongDto> Songs { get; set; }

        public SongFilterDto Filter { get; set; }

        public SelectList AllSortCriteria => new SelectList(SongSortCriteria);
    }
}