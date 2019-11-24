using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.WebApi.Models.Songs
{
    public class SongCreateModel
    {
        /// <summary>
        /// Song to create.
        /// </summary>
        public SongDto Song { get; set; }

        /// <summary>
        /// Name of the album to assign product to.
        /// </summary>
        [Required, MinLength(1), MaxLength(256)]
        public string AlbumName { get; set; }
    }
}