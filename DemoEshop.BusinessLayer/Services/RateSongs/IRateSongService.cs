using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;

namespace DemoEshop.BusinessLayer.Services.RateSongs
{
    interface IRateSongService
    {
        void RateSong(RateSongDto songRateDTO);

        Task<RateSongDto> GetRatingAccordingToSongIdAsync(Guid songId);
    }
}
