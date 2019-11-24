using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.WebApi.Models.Songs;

namespace DemoEshop.WebApi.Controllers
{
    public class SongsController : ApiController
    {
        public SongFacade SongFacade { get; set; }

        /// <summary>
        /// Query products according to given query parameters, example URL call: http://localhost:56118/api/Products/Query?sort=name&asc=true&name=Samsung&minimalPrice=5000&maximalPrice=23000&category=android&category=ios
        /// </summary>
        /// <param name="sort">Name of the song attribute (e.g. "name", ...) to sort according to</param>
        /// <param name="asc">Sort product collection in ascending manner</param>
        /// <param name="name">Song name (can be only partial: "Stay", "Night", ...)</param>
        /// <param name="album">Song Album name, currently supported are: "YoungBlood", "Sounds Good Feels Good" and "5 Seconds of Summer"</param>
        /// <returns>Collection of songs, satisfying given query parameters.</returns>       
        [HttpGet, Route("api/Songs/Query")]
        public async Task<IEnumerable<SongDto>> Query(string sort = null, bool asc = true, 
            string name = null, [FromUri] string album = null)
        {
            var filter = new SongFilterDto
            {
                SortCriteria = sort,
                SortAscending = asc,
                AlbumName = album,
                SearchedName = name
            };
            var songs = (await SongFacade.GetSongsAsync(filter, false)).Items;
            foreach (var song in songs)
            {
                song.Id = Guid.Empty;
            }
            return songs;
        }

        /// <summary>
        /// Gets song info (including lyrics) for song with given name, 
        /// example URL call: http://localhost:56118/api/Products/Get?name=lg
        /// </summary>
        /// <param name="name">Song name ("Amnesia", ...)</param>
        /// <returns>Complete song info.</returns>
        public async Task<SongDto> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var song = await SongFacade.GetSongAsync(name);         
            if (song == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            song.Id = Guid.Empty;
            return song;
        }

        /// <summary>
        /// Creates song, example URL call can be found in test folder.
        /// </summary>
        /// <param name="model">Created songs details.</param>
        /// <returns>Message describing the action result.</returns>
        public async Task<string> Post([FromBody]SongCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var songId = await SongFacade.CreateSongWithAlbumNameAsync(model.Song, model.AlbumName);
            if (songId.Equals(Guid.Empty))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return $"Created song with id: {songId}, within {model.AlbumName} album.";
        }

        /// <summary>
        /// Updates song with given id, example URL call can be found in test folder.
        /// </summary>
        /// <param name="id">Id of the song to update.</param>
        /// <param name="song">Song to update</param>
        /// <returns>Message describing the action result.</returns>
        public async Task<string> Put(Guid id, [FromBody]SongDto song)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var success = await SongFacade.EditSongAsync(song);
            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return $"Updated song with id: {id}";
        }

        /// <summary>
        /// Deletes song with given id ("aa05dc64-5c07-40fe-a916-175165b9b90f", "aa06dc64-5c07-40fe-a916-175165b9b90f", ...),
        /// example URL call can be found in test folder.
        /// </summary>
        /// <param name="id">Id of the product to delete.</param>
        /// <returns>Message describing the action result.</returns>
        public async Task<string> Delete(Guid id)
        {
            var success = await SongFacade.DeleteSongAsync(id);
            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return $"Deleted song with id: {id}";
        }
    }
}
